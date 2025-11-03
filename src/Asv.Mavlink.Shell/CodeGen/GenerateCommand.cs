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
    private class TargetFileNotFoundException() : Exception("Target file not found") { }
    
    private string _inputFolder = "in";
    private string _outputFolder = "out";
    private string _targetFileName = "standard.xml";
    private string _extension = string.Empty;
    private readonly LiquidGenerator _generator = new();

    /// <summary>
    /// Generate files from MAVLink XML message definitions.
    /// </summary>
    /// <param name="ext">-e, Output file extension</param>
    /// <param name="templateFile">-tmpl, Template file (Liquid syntax)</param>
    /// <param name="targetFile">-t, [Optional] Target file name (default: standard.xml)</param>
    /// <param name="inputFolder">-i, [Optional] Input folder (default: "in")</param>
    /// <param name="outputFolder">-o, [Optional] Output folder (default: "out")</param>
    [Command("gen")]
    public async Task<int> RunGenerate(
        string ext,
        string templateFile,
        string? targetFile = null,
        string? inputFolder = null,
        string? outputFolder = null)
    {
        try
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                // ReSharper disable once AccessToDisposedClosure
                cancellationTokenSource.Cancel();
            };

            cancellationToken.ThrowIfCancellationRequested();

            _extension = string.IsNullOrWhiteSpace(ext)
                ? Path.GetExtension(templateFile).TrimStart('.')
                : ext;

            _targetFileName = targetFile ?? _targetFileName;
            _inputFolder = inputFolder ?? _inputFolder;
            _outputFolder = outputFolder ?? _outputFolder;

            if (!Directory.Exists(_inputFolder) || !File.Exists(Path.Combine(_inputFolder, _targetFileName)))
            {
                throw new TargetFileNotFoundException();
            }

            if (!Directory.Exists(_outputFolder))
            {
                Directory.CreateDirectory(_outputFolder);
            }

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
                    AnsiConsole.MarkupLine($"[yellow]warning[/]: Skipping '{model.FileName}'");
                    continue;
                }

                var outputPath = await GenerateFileAsync(templateContent, model, cancellationToken);
                generatedFiles.Add(model.FileName);
                AnsiConsole.MarkupLine($"[blue]info[/]: Generated '{outputPath}'");
            }
        }
        catch (OperationCanceledException)
        {
            AnsiConsole.MarkupLine("[yellow]warning[/]: Operation Cancelled by the user. Exiting gracefully...");
            return 1;
        }
        catch (TargetFileNotFoundException e)
        {
            AnsiConsole.MarkupLine("[white]note[/]: You must specify a target file to use this command. " +
                                  $"If you use this command by default than you have to run {nameof(ExampleCommand)} first.");
            AnsiConsole.WriteException(e, ExceptionFormats.ShortenEverything);
            return -2;
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e, ExceptionFormats.ShortenEverything);
            return -1;
        }
        
        return 0;
    }

    private async Task<string> GenerateFileAsync(string template, MavlinkProtocolModel model, CancellationToken cancellationToken)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(model.FileName);
        var outputPath = Path.Combine(_outputFolder, $"{fileNameWithoutExtension}.{_extension}");

        var data = _generator.Generate(template, model);
        await File.WriteAllTextAsync(outputPath, data, cancellationToken);

        return outputPath;
    }

    private IEnumerable<MavlinkProtocolModel> GenerateModels(string fileName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        var filePath = Path.Combine(_inputFolder, fileName);
        AnsiConsole.MarkupLine($"[blue]info[/]: Processing '{filePath}'");

        using var stream = File.OpenRead(filePath);
        var model = MavlinkGenerator.ParseXml(fileName, stream);
        AnsiConsole.MarkupLine($"[blue]info[/]: Parsed '{model.FileName}', Enums: {model.Enums.Count}");
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