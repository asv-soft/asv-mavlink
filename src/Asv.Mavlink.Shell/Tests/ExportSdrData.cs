using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;
using Asv.Mavlink.AsvSdr;
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
    public async Task RunExportSdrDataAsync(string inputFile, string outputFile = "out.csv")
    {
        var recordSize = new AsvSdrRecordDataLlzPayload().GetMaxByteSize(); // = 186
        var buffer = ArrayPool<byte>.Shared.Rent(recordSize);

        try
        {
            await using var file = File.OpenRead(inputFile);
            if (file.Length == 0)
            {
                AnsiConsole.MarkupLine($"[red]Error:[/] Input file [yellow]{inputFile}[/] is empty.");
                return;
            }

            await using var outFile = new StreamWriter(File.OpenWrite(outputFile));
            var memory = new Memory<byte>(buffer, 0, recordSize);

            while (true)
            {
                int bytesRead = await file.ReadAsync(memory);
                if (bytesRead <= 0) break;

                if (bytesRead < recordSize)
                {
                    AnsiConsole.MarkupLine("[red]Warning:[/] Incomplete record detected at the end of file. Skipping.");
                    break;
                }

                var payload = Deserialize(memory.Span);

                await outFile.WriteLineAsync(
                    $"{payload.Alt}\t{payload.TotalAm90}\t{payload.TotalAm150}\t{payload.TotalPower}\t" +
                    $"{payload.ClrAm90}\t{payload.ClrAm150}\t{payload.ClrPower}\t" +
                    $"{payload.CrsAm90}\t{payload.CrsAm150}\t{payload.CrsPower}".Replace(",", "."));
            }

            AnsiConsole.MarkupLine($"[green]Export completed[/]. Data saved to [yellow]{outputFile}[/]");
        }
        catch (FileNotFoundException)
        {
            AnsiConsole.MarkupLine($"[red]Error:[/] Input file [yellow]{inputFile}[/] not found.");
        }
        catch (UnauthorizedAccessException ex)
        {
            AnsiConsole.MarkupLine($"[red]Access denied:[/] {ex.Message}");
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Unexpected error:[/] {ex.Message}");
        }
        finally
        {
           ArrayPool<byte>.Shared.Return(buffer);
        }
    }
    
    private AsvSdrRecordDataLlzPayload Deserialize(ReadOnlySpan<byte> span)
    {
        var payload = new AsvSdrRecordDataLlzPayload();
        payload.Deserialize(ref span);
        return payload;
    }
}