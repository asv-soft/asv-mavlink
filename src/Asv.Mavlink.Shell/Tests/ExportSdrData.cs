using System;
using System.IO;
using Asv.Mavlink.V2.AsvSdr;
using ManyConsole;

namespace Asv.Mavlink.Shell;

public class ExportSdrData: ConsoleCommand
{
    public ExportSdrData()
    {
        IsCommand("export-sdr");
        AllowsAnyAdditionalArguments();
    }
    
    
    public override int Run(string[] remainingArguments)
    {
        
        using (var file = File.OpenRead(remainingArguments[0]))
        using (var outFile = new StreamWriter(File.OpenWrite("out.csv")))
        {
            var data = new byte[256];
            while (file.Position < file.Length)
            {
                file.Read(data, 0, data.Length);
                AsvSdrRecordDataLlzPayload payload = new AsvSdrRecordDataLlzPayload();
                var span = new ReadOnlySpan<byte>(data);
                payload.Deserialize(ref span);
                outFile.WriteLine($"{payload.Alt}\t{payload.TotalAm90}\t{payload.TotalAm150}\t{payload.TotalPower}\t{payload.ClrAm90}\t{payload.ClrAm150}\t{payload.ClrPower}\t{payload.CrsAm90}\t{payload.CrsAm150}\t{payload.CrsPower}".Replace(",", "."));
            }
        }
        return 0;
    }
}