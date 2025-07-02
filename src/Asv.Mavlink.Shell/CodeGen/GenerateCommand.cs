using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using ConsoleAppFramework;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class GenerateCommand
{
    private string _inputFolder = "in";
    private string _outputFolder = "out";
    private string _targetFileName = "standard.xml";
    private string _extension = string.Empty;

    /// <summary>
    /// Generate files from MAVLink XML message definitions.
    /// </summary>
    /// <param name="ext">-e, Output file extension</param>
    /// <param name="templateFile">-template, Template file (Liquid syntax)</param>
    /// <param name="targetFile">-t, [Optional] Target file name (default: standard.xml)</param>
    /// <param name="inputFolder">-i, [Optional] Input folder (default: "in")</param>
    /// <param name="outputFolder">-o, [Optional] Output folder (default: "out")</param>
    [Command("gen")]
    public async Task RunGenerate(
        string ext,
        string templateFile,
        string? targetFile = null,
        string? inputFolder = null,
        string? outputFolder = null)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        cancellationToken.ThrowIfCancellationRequested();                

        _extension = string.IsNullOrWhiteSpace(ext)
            ? Path.GetExtension(templateFile).TrimStart('.')
            : ext;

        _targetFileName = targetFile ?? _targetFileName;
        _inputFolder = inputFolder ?? _inputFolder;
        _outputFolder = outputFolder ?? _outputFolder;
            
        if (!Directory.Exists(_inputFolder))
            Directory.CreateDirectory(_inputFolder);

        if (!Directory.Exists(_outputFolder))
            Directory.CreateDirectory(_outputFolder);
            
        var generatedFiles = new HashSet<string>();
        var templateContent = await File.ReadAllTextAsync(templateFile, cancellationToken);

        foreach (var model in GenerateModels(_targetFileName, cancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrEmpty(model.FileName))
            {
                AnsiConsole.MarkupLine("[red]error[/]: Model FileName is null or empty");
                continue;
            }

            if (generatedFiles.Contains(model.FileName))
            {
                AnsiConsole.MarkupLine($"[yellow]Skipping[/] '{model.FileName}'");
                continue;
            }

            try
            {
                var outputPath = await GenerateFileAsync(templateContent, model, cancellationToken);
                generatedFiles.Add(model.FileName);
                AnsiConsole.MarkupLine($"[blue]info[/]: Generated '{outputPath}'");
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
            }
        }
    }

    private async Task<string> GenerateFileAsync(string template, MavlinkProtocolModel model, CancellationToken cancellationToken)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(model.FileName);
        var outputPath = Path.Combine(_outputFolder, $"{fileNameWithoutExtension}.{_extension}");

        var data = new LiquidGenerator().Generate(template, model);
        await File.WriteAllTextAsync(outputPath, data, cancellationToken);

        return outputPath;
    }

    private IEnumerable<MavlinkProtocolModel> GenerateModels(string fileName, CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(_inputFolder, fileName);
        AnsiConsole.MarkupLine($"[blue]info[/]: Processing '{filePath}'");

        cancellationToken.ThrowIfCancellationRequested();

        MavlinkProtocolModel model;
        try
        {
            using var stream = File.OpenRead(filePath);
            model = MavlinkGenerator.ParseXml(fileName, stream);
            AnsiConsole.MarkupLine($"[blue]info[/]: Parsed '{model.FileName}', Enums: {model.Enums.Count}");
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e);
            yield break;
        }

        yield return model;

        foreach (var include in model.Include)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var includedModel in GenerateModels(include, cancellationToken))
            {
                yield return includedModel;
            }
        }
    }
}