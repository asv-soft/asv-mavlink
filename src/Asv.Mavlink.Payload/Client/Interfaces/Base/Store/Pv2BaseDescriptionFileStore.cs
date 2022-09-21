using System;
using System.Collections.Generic;
using System.IO;
using Asv.IO;
using NLog;

namespace Asv.Mavlink.Payload
{

    public class WorkModeFileDescriptionMetadata : ISpanSerializable
    {
        public const int MetadataByteOffset = 255;

        public byte Version { get; set; }
        public string Comment { get; set; }
        public uint Hash { get; set; }
        public int WorkModeCount { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Comment = BinSerialize.ReadString(ref buffer);
            Version = BinSerialize.ReadByte(ref buffer);
            Hash = BinSerialize.ReadUInt(ref buffer);
            WorkModeCount = BinSerialize.ReadInt(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteString(ref buffer, Comment);
            BinSerialize.WriteByte(ref buffer, Version);
            BinSerialize.WriteUInt(ref buffer, Hash);
            BinSerialize.WriteInt(ref buffer, WorkModeCount);
        }
    }

    public class Pv2BaseDescriptionFileStore : IPv2BaseDescriptionStore
    {
        private readonly string _folder;
        private readonly object _sync = new object();
        private const int FileItemByteOffset = 255;
        private const int CurrentFileVersion = 1;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Pv2BaseDescriptionFileStore(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(folder));
            if (Directory.Exists(folder) == false) Directory.CreateDirectory(folder);
            _folder = folder;
        }

        public bool TryGetFromCache(uint hash, uint count, out IList<(Pv2WorkModeInfo, IList<Pv2WorkModeStatusInfo>)> list)
        {
            lock (_sync)
            {
                var fileName = GetFilePathByHash(hash);
                if (File.Exists(fileName) == false)
                {
                    Logger.Info($"Work mode description cache file {fileName} not found.");
                    list = null;
                    return false;
                }

                try
                {
                    using var file = File.OpenRead(fileName);
                    var metadata = new WorkModeFileDescriptionMetadata();
                    metadata.ReadFromStream(file, WorkModeFileDescriptionMetadata.MetadataByteOffset);
                    if (metadata.Version != CurrentFileVersion)
                        Logger.Warn(
                            $"Unknown version of WorkMode description hash file {fileName}. Want {CurrentFileVersion}, got {metadata.Version}");

                    if (metadata.Hash != hash)
                    {
                        Logger.Warn($"Found WorkMode description hash file {fileName} with unknown hash {hash}");
                        list = null;
                        return false;
                    }

                    if (metadata.WorkModeCount != count)
                    {
                        Logger.Warn(
                            $"Found WorkMode description hash file {fileName} with wrong WorkMode count. Want {count}. Got in file {metadata.WorkModeCount}");
                        list = null;
                        return false;
                    }


                    list = new List<(Pv2WorkModeInfo, IList<Pv2WorkModeStatusInfo>)>();

                    for (var i = 0; i < count; i++)
                    {
                        var workMode = new Pv2WorkModeInfo();
                        workMode.ReadFromStream(file,FileItemByteOffset);
                        var statusCount = new SpanPacketUnsignedIntegerType();
                        statusCount.ReadFromStream(file,FileItemByteOffset);
                        var items = new List<Pv2WorkModeStatusInfo>((int)statusCount.Value);
                        for (var j = 0; j < statusCount; j++)
                        {
                            var status = new Pv2WorkModeStatusInfo();
                            status.ReadFromStream(file,FileItemByteOffset);
                            items.Add(status);
                        }
                        list.Add((workMode,items));
                    }

                    Logger.Info(
                        $"RTT description loaded from cache file {fileName}. Items {count} items.");
                    return true;
                }
                catch (Exception e)
                {
                    File.Delete(fileName);
                    Logger.Error(e, $"Error to read WorkMode description cache file {fileName}:{e.Message}");
                    list = null;
                    return false;
                }
            }
        }

        public void Save(uint hash, IList<(Pv2WorkModeInfo, IList<Pv2WorkModeStatusInfo>)> paramsList)
        {
            lock (_sync)
            {
                var fileName = GetFilePathByHash(hash);
                try
                {
                    if (File.Exists(fileName))
                    {
                        Logger.Debug($"Delete CFG cache file {fileName}");
                        File.Delete(fileName);
                    }

                    Logger.Debug($"Write CFG cache file {fileName}");
                    using var file = File.OpenWrite(fileName);

                    var metadata = new WorkModeFileDescriptionMetadata
                    {
                        Version = CurrentFileVersion,
                        Comment = $"This file is WorkMode description cache Version {CurrentFileVersion}",
                        WorkModeCount = paramsList.Count,
                        Hash = hash
                    };
                    metadata.WriteToStream(file, RttFileDescriptionMetadata.MetadataByteOffset);


                    foreach (var mode in paramsList)
                    {
                        mode.Item1.WriteToStream(file,FileItemByteOffset);
                        var statusCount = new SpanPacketUnsignedIntegerType((uint)mode.Item2.Count);
                        statusCount.WriteToStream(file,FileItemByteOffset);
                        foreach (var status in mode.Item2)
                        {
                            status.WriteToStream(file,FileItemByteOffset);
                        }
                    }
                        
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Error to save WorkMode to local cash file {fileName}");
                }
            }
        }

        private string GetFilePathByHash(uint hash)
        {
            return Path.Combine(_folder, $"WORK_MODE_CACHE_{hash}.bin");
        }

    }
}
