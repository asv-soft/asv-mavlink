using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Asv.Mavlink.AsvRsga;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ConsoleAppFramework;
using LiteDB;
using MasterMemory;
using MessagePack;

namespace Asv.Mavlink.Shell;

public class LiteDbTest
{
    private string _fileName = "test";
    /// <summary>
    /// Litedb tests
    /// </summary>
    /// <param name="fileName">-f, File name</param>
    [Command("litedb-test")]
    public async Task RunLiteDbTest(string fileName = @"test")
    {
        _fileName = fileName;
        /*using (var db = new LiteDatabase($"000_file.litedb"))
        {
            var coll = db.GetCollection<AsvRsgaRttGnssPayload>();
            for (int i = 0; i < 100_000; i++)
            {
                coll.Insert(new AsvRsgaRttGnssPayload());
            }
        }*/
        
        var builder = new DatabaseBuilder();
        builder.Append(GetDataSource);

        var strm = File.OpenWrite("000_file2.bin");
        builder.WriteToStream(strm);
        strm.Dispose();
        var file = File.OpenRead("000_file2.bin");
        var buff = ArrayPool<byte>.Shared.Rent((int)file.Length);
        file.ReadExactly(buff, 0, (int)file.Length);
        MemoryDatabase db = new MemoryDatabase(buff);
        
        
        ArrayPool<byte>.Shared.Return(buff);
    }

    public IEnumerable<AsvRsgaRttGnssPayloadMm> GetDataSource
    {
        get
        {
            for (int i = 0; i < 100_000; i++)
            {
                yield return new AsvRsgaRttGnssPayloadMm{Id = i};
            }
        }
    }

}


[MemoryTable("person"), MessagePackObject(true)]
public class AsvRsgaRttGnssPayloadMm : AsvRsgaRttGnssPayload
{
    [PrimaryKey]
    public required int Id { get; set; }
}