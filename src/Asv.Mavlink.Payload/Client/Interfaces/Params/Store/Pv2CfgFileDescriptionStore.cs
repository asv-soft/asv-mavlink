using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Asv.IO;
using NLog;

namespace Asv.Mavlink.Payload
{
    public class CfgFileDescriptionMetadata : ISpanSerializable
    {
        public const int MetadataByteOffset = 255;

        public string Comment { get; set; }
        public byte Version { get; set; }
        public uint Hash { get; set; }
        public int ItemsCount { get; set; }

        public void Deserialize(ref ReadOnlySpan<byte> buffer)
        {
            Comment = BinSerialize.ReadString(ref buffer);
            Version = BinSerialize.ReadByte(ref buffer);
            Hash = BinSerialize.ReadUInt(ref buffer);
            ItemsCount = BinSerialize.ReadInt(ref buffer);
        }

        public void Serialize(ref Span<byte> buffer)
        {
            BinSerialize.WriteString(ref buffer, Comment);
            BinSerialize.WriteByte(ref buffer, Version);
            BinSerialize.WriteUInt(ref buffer, Hash);
            BinSerialize.WriteInt(ref buffer, ItemsCount);
        }
    }

    public class Pv2CfgFileDescriptionStore : IPv2CfgDescriptionStore
    {
        private const int FileItemByteOffset = 255;
        private const int CurrentFileVersion = 1;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _folder;
        private readonly object _sync = new();

        public Pv2CfgFileDescriptionStore(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(folder));
            if (Directory.Exists(folder) == false) Directory.CreateDirectory(folder);
            _folder = folder;
        }

        public bool TryGetFromCache(uint hash, uint count, out List<Pv2ParamType> paramsList)
        {
            lock (_sync)
            {
                var fileName = GetFilePathByHash(hash);
                if (File.Exists(fileName) == false)
                {
                    Logger.Info($"RTT description cache file {fileName} not found.");
                    paramsList = null;
                    return false;
                }

                try
                {
                    using var file = File.OpenRead(fileName);
                    var metadata = new CfgFileDescriptionMetadata();
                    metadata.ReadFromStream(file, CfgFileDescriptionMetadata.MetadataByteOffset);
                    if (metadata.Version != CurrentFileVersion)
                        Logger.Warn(
                            $"Unknown version of CFG description hash file {fileName}. Want {CurrentFileVersion}, got {metadata.Version}");

                    if (metadata.Hash != hash)
                    {
                        Logger.Warn($"Found CFG description hash file {fileName} with unknown hash {hash}");
                        paramsList = null;
                        return false;
                    }

                    if (metadata.ItemsCount != count)
                    {
                        Logger.Warn(
                            $"Found CFG description hash file {fileName} with wrong fields count. Want {count}. Got in file {metadata.ItemsCount}");
                        paramsList = null;
                        return false;
                    }

                    paramsList = new List<Pv2ParamType>((int)count);

                    for (var i = 0; i < count; i++)
                    {
                        var item = new Pv2ParamValueAndTypePair();
                        item.ReadFromStream(file, FileItemByteOffset);
                        paramsList.Add(item.Type);
                    }

                    Logger.Info(
                        $"RTT description loaded from cache file {fileName}. Items {count} items.");
                    return true;
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Error to read CFG description cache file {fileName}:{e.Message}");
                    File.Delete(fileName);
                    paramsList = null;
                    return false;
                }
            }
        }

        public void Save(uint hash, List<Pv2ParamType> paramsList)
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

                    var metadata = new CfgFileDescriptionMetadata
                    {
                        Version = CurrentFileVersion,
                        Comment = $"This file is CFG description cache Version {CurrentFileVersion}",
                        ItemsCount = paramsList.Count,
                        Hash = hash
                    };
                    metadata.WriteToStream(file, RttFileDescriptionMetadata.MetadataByteOffset);


                    foreach (var item in paramsList.Select((field, inx) =>
                                 new Pv2ParamValueAndTypePair(field, null, (uint)inx)))
                    {
                        item.WriteToStream(file, FileItemByteOffset);
                    }
                        
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Error to save CFG to local hash file {fileName}");
                    throw;
                }
            }
        }

        private string GetFilePathByHash(uint hash)
        {
            return Path.Combine(_folder, $"CFG_CACHE_{hash}.bin");
        }

       
    }
}
