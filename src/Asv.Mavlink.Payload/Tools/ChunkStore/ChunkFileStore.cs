using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NLog;

namespace Asv.Mavlink.Payload
{
    public delegate void FieldWriteCallback(ref Span<byte> data);

    public delegate void FieldReadCallback(ref ReadOnlySpan<byte> data);

    public class ChunkFileStore: IChunkStore
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string MetadataFileName = "_info.json";
        private const string RecordFileExt = "rtt";
        private readonly string _rootFolder;
        private readonly object _sync = new();
        private string _recordFolderPath;
        private readonly Dictionary<uint, (SessionRecordMetadata, FileStream)> _files = new();

        public ChunkFileStore(string rootFolder)
        {
            _rootFolder = rootFolder;
            if (Directory.Exists(rootFolder) == false)
            {
                Directory.CreateDirectory(_rootFolder);
            }
        }

        public SessionMetadata Start(SessionSettings settings,IEnumerable<SessionFieldSettings> records)
        {
            CheckNotStarted();
            lock (_sync)
            {
                CheckNotStarted();
                var id = Guid.NewGuid();
                var metadata = new SessionMetadata()
                {
                    SessionId = new SessionId(id),
                    Settings = settings,
                };
                _recordFolderPath = GetSessionFolderName(metadata.SessionId);
                if (Directory.Exists(_recordFolderPath))
                {
                    Logger.Warn($"Directory for new recording already exist {_recordFolderPath}. Remove all data.");
                    Directory.Delete(_recordFolderPath);
                }
                var cnt = 0U;
                Directory.CreateDirectory(_recordFolderPath);
                foreach (var rec in records)
                {
                    ++cnt;
                    var recordMetadata = new SessionRecordMetadata
                    {
                        Settings = new SessionFieldSettings(rec.Id,rec.Name,rec.Offset)
                    };
                    var file = File.OpenWrite(GetRecordFileName(metadata.SessionId, rec.Id));
                    var metadataArr = ArrayPool<byte>.Shared.Rent(SessionRecordMetadata.MetadataFileOffset);
                    var span = new Span<byte>(metadataArr,0, SessionRecordMetadata.MetadataFileOffset);
                    try
                    {
                        recordMetadata.Serialize(ref span);
                        if (span.IsEmpty == false)
                        {
                            for (int i = 0; i < span.Length; i++)
                            {
                                span[i] = 0;
                            }
                        }
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(metadataArr);
                    }
                    file.Write(metadataArr,0, SessionRecordMetadata.MetadataFileOffset);
                    _files.Add(rec.Id, (recordMetadata, file));
                }

                File.WriteAllText(Path.Combine(_recordFolderPath,MetadataFileName), JsonConvert.SerializeObject(metadata));
                IsStarted = true;
                return Current = metadata;
            }
        }

        public void Stop()
        {
            if (IsStarted == false) return;
            lock (_sync)
            {
                if (IsStarted == false) return;
                IsStarted = false;
                Current = null;
                foreach (var item in _files.Values)
                {
                    item.Item2.Flush();
                    item.Item2.Dispose();
                }
                _files.Clear();
            }
        }

        public IEnumerable<SessionId> GetSessions()
        {
            foreach (var dir in Directory.EnumerateDirectories(_rootFolder))
            {
                var path = Path.GetFileName(dir);
                if (!Guid.TryParse(path, out var guid)) continue;
                var sesId = new SessionId(guid);
                var metadataFile = GetMetadataFileName(sesId);
                try
                {
                    var metadata = File.Exists(metadataFile) ? JsonConvert.DeserializeObject<SessionMetadata>(File.ReadAllText(metadataFile)) : null;
                    if (metadata == null)
                    {
                        Logger.Warn($"Found unexpected RTT folder {path} without metadata file {metadataFile}. Ignore it.");
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Logger.Warn(e,$"Found unexpected RTT folder {path} with bad metadata file {metadataFile}:{e.Message}");
                    continue;
                }
                yield return sesId;
            }
        }

        public SessionInfo GetSessionInfo(SessionId sessionId)
        {
            var metadataFile = GetMetadataFileName(sessionId);
            var created = new FileInfo(metadataFile).CreationTime;
            var metadata = File.Exists(metadataFile) ? JsonConvert.DeserializeObject<SessionMetadata>(File.ReadAllText(metadataFile)): null;
            if (metadata == null)
            {
                Logger.Warn("");
            }
            var files = Directory.EnumerateFiles(GetSessionFolderName(sessionId), $"*.{RecordFileExt}");
            var enumerable = files as string[] ?? files.ToArray();
            var itemsCount = 0U;
            if (enumerable.Length > 0)
            {
                var fieldId = GetFieldsIds(sessionId).First();
                var info = GetFieldInfo(sessionId, fieldId);
                itemsCount = info.Count;
            }
            var totalSize = enumerable.Select(_ => (int)(new FileInfo(_).Length - SessionRecordMetadata.MetadataFileOffset)).Sum();

            return new SessionInfo(metadata, (uint)enumerable.Length, itemsCount, (uint)totalSize, created);

        }

        public IEnumerable<uint> GetFieldsIds(SessionId sessionId)
        {
            foreach (var filePath in Directory.EnumerateFiles(GetSessionFolderName(sessionId),$"*.{RecordFileExt}"))
            {
                yield return uint.Parse(Path.GetFileNameWithoutExtension(Path.GetFileName(filePath)));
            }
        }

        public SessionFieldInfo GetFieldInfo(SessionId sessionId, uint recordId)
        {
            CheckNotStarted();
            lock (_sync)
            {
                CheckNotStarted();
                using var file = File.OpenRead(GetRecordFileName(sessionId, recordId));
                var metadata = InternalReadRecordMetadata(file);
                return new SessionFieldInfo
                {
                    SizeInBytes = (uint)file.Length,
                    Count = (uint)((file.Length - SessionRecordMetadata.MetadataFileOffset)/ metadata.Settings.Offset),
                    Metadata = metadata,
                };
            }
        }

        private SessionRecordMetadata InternalReadRecordMetadata(FileStream file)
        {
            var recordMetadata = new SessionRecordMetadata();
            var recordMetadataArray = ArrayPool<byte>.Shared.Rent(SessionRecordMetadata.MetadataFileOffset);
            try
            {
                file.Position = 0;
                var read = file.Read(recordMetadataArray, 0, SessionRecordMetadata.MetadataFileOffset);
                if (read != SessionRecordMetadata.MetadataFileOffset)
                {
                    throw new Exception($"Error to read record {file.Name} metadata");
                }

                var recordMetadataSpan = new ReadOnlySpan<byte>(recordMetadataArray, 0, SessionRecordMetadata.MetadataFileOffset);
                recordMetadata.Deserialize(ref recordMetadataSpan);
                return recordMetadata;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(recordMetadataArray);
            }
        }

        public bool IsStarted { get; private set; }
        public SessionMetadata Current { get; set; }

        public uint Append(uint fieldId, FieldWriteCallback writeWriteCallback)
        {
            CheckIsStarted();
            lock (_sync)
            {
                CheckIsStarted();

                var file = _files[fieldId];
                var data = ArrayPool<byte>.Shared.Rent(file.Item1.Settings.Offset);
                try
                {
                    var span = new Span<byte>(data, 0, file.Item1.Settings.Offset);
                    writeWriteCallback(ref span);
                    if (span.IsEmpty == false)
                    {
                        for (var i = 0; i < span.Length; i++)
                        {
                            span[i] = 0;
                        }
                    }
                    file.Item2.Position = file.Item2.Length;
                    file.Item2.Write(data,0, file.Item1.Settings.Offset);
                    file.Item2.Flush();
                    return (uint)((file.Item2.Position - SessionRecordMetadata.MetadataFileOffset) / file.Item1.Settings.Offset);
                }
                catch (Exception e)
                {
                    Logger.Error($"Error to append record {fieldId}:{e.Message}");
                    throw;
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(data);
                }
            }
        }

        private void CheckIsStarted()
        {
            if (IsStarted == false)
            {
                throw new Exception("Record not started");
            }
        }

        private void CheckNotStarted()
        {
            if (IsStarted == true)
            {
                throw new Exception("Record is started");
            }
        }


        public bool ReadRecord(SessionId sessionId, uint recordId, uint index, FieldReadCallback readCallback)
        {
            CheckNotStarted();
            lock (_sync)
            {
                CheckNotStarted();
                
                using var file = File.OpenRead(GetRecordFileName(sessionId, recordId));
                var recordMetadata = InternalReadRecordMetadata(file);
                var recordDataArray = ArrayPool<byte>.Shared.Rent(recordMetadata.Settings.Offset);
                try
                {
                    var position = SessionRecordMetadata.MetadataFileOffset + recordMetadata.Settings.Offset * index;
                    if (position >= file.Length) return false;
                    file.Position = position;
                    var read = file.Read(recordDataArray, 0, recordMetadata.Settings.Offset);
                    if (read != recordMetadata.Settings.Offset)
                    {
                        throw new Exception($"Error to read record {recordId} data");
                    }
                    var recordDataSpan = new ReadOnlySpan<byte>(recordDataArray, 0, recordMetadata.Settings.Offset);
                    readCallback(ref recordDataSpan);
                    return true;
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(recordDataArray);
                }
            }
        }

        public bool Delete(SessionId sessionId)
        {
            CheckNotStarted();
            var folder = GetSessionFolderName(sessionId);
            if (!Directory.Exists(folder)) return false;
            Directory.Delete(folder,true);
            return true;
        }

        private string GetMetadataFileName(SessionId sessionId)
        {
            return Path.Combine(GetSessionFolderName(sessionId), MetadataFileName);
        }

        private string GetSessionFolderName(SessionId id)
        {
            return Path.Combine(_rootFolder, id.Guid.ToString());
        }

        private string GetRecordFileName(SessionId sessionId,uint id)
        {
            return Path.Combine(GetSessionFolderName(sessionId), $"{id}.{RecordFileExt}");
        }

        public void Dispose()
        {
            Stop();
        }
    }

   
}
