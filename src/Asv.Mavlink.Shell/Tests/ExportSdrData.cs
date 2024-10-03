using System;
using System.IO;
using Asv.Mavlink.V2.AsvSdr;
using ConsoleAppFramework;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class ExportSdrData
{

    /// <summary>
    /// Export sdt data to csv format
    /// </summary>
    /// <param name="inputFile">-i, Input file</param>
    /// <param name="outputFile">-o, Output file</param>
    [Command("export-sdr")]
    public void RunExportSdrData(string inputFile, string outputFile = "out.csv")
    {
            using var file = File.OpenRead(inputFile);
            using var outFile = new StreamWriter(File.OpenWrite(outputFile));
            var data = new byte[256];
            
            while (file.Position < file.Length)
            {
                file.Read(data, 0, data.Length);
                AsvSdrRecordDataLlzPayload payload = new AsvSdrRecordDataLlzPayload();
                var span = new ReadOnlySpan<byte>(data);
                payload.Deserialize(ref span);
                outFile.WriteLine($"{payload.Alt}\t{payload.TotalAm90}\t{payload.TotalAm150}\t{payload.TotalPower}\t{payload.ClrAm90}\t{payload.ClrAm150}\t{payload.ClrPower}\t{payload.CrsAm90}\t{payload.CrsAm150}\t{payload.CrsPower}".Replace(",", "."));
            }
            AnsiConsole.MarkupLine($"[green]Export completed[/]. Data saved to [yellow]{outputFile}[/]");
    }
}