using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Asv.IO;
using NLog;

namespace Asv.Mavlink.Payload
{
    public class RttFileDescriptionMetadata : ISpanSerializable
    {
        public const int MetadataByteOffset = 255;

        public byte Version { get; set; }
        public string Comment { get; set; }
        public uint Hash { get; set; }
        public int RecordsCount { get; set; }
        public int FieldsCount { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Comment = BinSerialize.ReadString(ref buffer);
            Version = BinSerialize.ReadByte(ref buffer);
            Hash = BinSerialize.ReadUInt(ref buffer);
            RecordsCount = BinSerialize.ReadInt(ref buffer);
            FieldsCount = BinSerialize.ReadInt(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteString(ref buffer, Comment);
            BinSerialize.WriteByte(ref buffer, Version);
            BinSerialize.WriteUInt(ref buffer, Hash);
            BinSerialize.WriteInt(ref buffer, RecordsCount);
            BinSerialize.WriteInt(ref buffer, FieldsCount);
        }
    }

    public class Pv2RttFileDescriptionStore : IPv2RttDescriptionStore
    {
        private const int FileItemByteOffset = 255;
        private const int CurrentFileVersion = 1;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _folder;
        private readonly object _sync = new();

        public Pv2RttFileDescriptionStore(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(folder));
            if (Directory.Exists(folder) == false) Directory.CreateDirectory(folder);
            _folder = folder;
        }

        public bool TyGetFromCache(uint hash, uint recordsCount, uint fieldsCount, out List<Pv2RttRecordDesc> records,
            out List<Pv2RttFieldDesc> fields)
        {
            lock (_sync)
            {
                var fileName = GetFilePathByHash(hash);
                if (File.Exists(fileName) == false)
                {
                    Logger.Info($"RTT description cache file {fileName} not found.");
                    records = null;
                    fields = null;
                    return false;
                }

                try
                {
                    using var file = File.OpenRead(fileName);
                    var metadata = new RttFileDescriptionMetadata();
                    metadata.ReadFromStream(file,  RttFileDescriptionMetadata.MetadataByteOffset);
                    if (metadata.Version != CurrentFileVersion)
                        Logger.Warn(
                            $"Unknown version of RTT description hash file {fileName}. Want {CurrentFileVersion}, got {metadata.Version}");

                    if (metadata.Hash != hash)
                    {
                        Logger.Warn($"Found RTT description hash file {fileName} with unknown hash {hash}");
                        records = null;
                        fields = null;
                        return false;
                    }

                    if (metadata.FieldsCount != fieldsCount)
                    {
                        Logger.Warn(
                            $"Found RTT description hash file {fileName} with wrong fields count. Want {fieldsCount}. Got in file {metadata.FieldsCount}");
                        records = null;
                        fields = null;
                        return false;
                    }

                    if (metadata.RecordsCount != recordsCount)
                    {
                        Logger.Warn(
                            $"Found RTT description hash file {fileName} with wrong records count. Want {recordsCount}. Got in file {metadata.RecordsCount}");
                        records = null;
                        fields = null;
                        return false;
                    }

                    records = new List<Pv2RttRecordDesc>((int)recordsCount);
                    fields = new List<Pv2RttFieldDesc>((int)fieldsCount);
                    for (var i = 0; i < recordsCount; i++)
                    {
                        var item = new Pv2RttRecordDesc();
                        item.ReadFromStream(file,  FileItemByteOffset);
                        records.Add(item);
                    }

                    for (var i = 0; i < fieldsCount; i++)
                    {
                        var item = new Pv2RttFieldResult();
                        item.ReadFromStream(file, FileItemByteOffset);
                        fields.Add(item.Desc);
                    }

                    Logger.Info(
                        $"RTT description loaded from cache file {fileName}. Records {recordsCount} items. Fields {fieldsCount} items.");
                    return true;
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Error to read RTT description cache file {fileName}:{e.Message}");
                    File.Delete(fileName);
                    records = null;
                    fields = null;
                    return false;
                }
            }
        }

        public void Save(uint hash, List<Pv2RttRecordDesc> records, List<Pv2RttFieldDesc> fields)
        {
            lock (_sync)
            {
                var fileName = GetFilePathByHash(hash);
                try
                {
                    if (File.Exists(fileName))
                    {
                        Logger.Debug($"Delete RTT cache file {fileName}");
                        File.Delete(fileName);
                    }

                    Logger.Debug($"Write RTT cache file {fileName}");
                    using var file = File.OpenWrite(fileName);

                    var metadata = new RttFileDescriptionMetadata
                    {
                        Version = CurrentFileVersion,
                        Comment = $"This file is RTT description cache Version {CurrentFileVersion}",
                        FieldsCount = fields.Count,
                        RecordsCount = records.Count,
                        Hash = hash
                    };
                    metadata.WriteToStream(file, RttFileDescriptionMetadata.MetadataByteOffset);
                    foreach (var record in records)
                    {
                        record.WriteToStream(file, FileItemByteOffset);
                    }

                    foreach (var item in fields.Select(field => new Pv2RttFieldResult(field)))
                    {
                        item.WriteToStream(file, FileItemByteOffset);
                    }
                        
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Error to save RTT config to local hash file {fileName}");
                    throw;
                }
            }
        }


        private string GetFilePathByHash(uint hash)
        {
            return Path.Combine(_folder, $"RTT_CACHE_{hash}.bin");
        }

    }
}
