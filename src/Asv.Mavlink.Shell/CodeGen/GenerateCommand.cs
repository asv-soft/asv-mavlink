using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ConsoleAppFramework;
using Spectre.Console;

namespace Asv.Mavlink.Shell
{
    public class GenerateCommand
    {
        private string _in = "in";
        private string _targetFileName = "standard.xml";
        private string _out = "out";
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private string _ext;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        /// <summary>
        /// Generate file form MAVLink XML message definitions.
        /// Example: gen -t=common.xml -i=in -o=out -e=cs csharp.tpl"
        /// </summary>
        /// <param name="ext">-e, Output files extensions</param>
        /// <param name="templateFile">-template, Liquid syntax template file, that used for generation</param>
        /// <param name="targetFile">-t, [Optional] OnTarget file name. By default standard.xml</param>
        /// <param name="inputFolder">-i, [Optional] Folder with source XML files. By default "in" folder</param>
        /// <param name="outputFolder">-o, [Optional] Output folder with results. By default "out" folder</param>
        [Command("gen")]
        public async Task RunGenerate(string ext,
            string templateFile, string? targetFile = null, string? inputFolder = null, string? outputFolder = null)
        {
            _targetFileName = targetFile ?? _targetFileName;
            _in = inputFolder ?? _in;
            _out = outputFolder ?? _out;
            _ext = ext ?? Path.GetExtension(templateFile);

            if (!Directory.Exists(_in))
            {
                Directory.CreateDirectory(_in);
            }

            if (!Directory.Exists(_out))
            {
                Directory.CreateDirectory(_out);
            }

            var generatedFiles = new HashSet<string>();

            foreach (var model in Generate(_targetFileName))
            {
                try
                {
                    if (generatedFiles.Contains(model.FileName ?? throw new InvalidOperationException()))
                    {
                        AnsiConsole.MarkupLine($"[yellow]Skip[/] {model.FileName}");
                        continue;
                    }

                    var template = await File.ReadAllTextAsync(templateFile);
                    var data = new LiquidGenerator().Generate(template, model);
                    var file = Path.GetFileNameWithoutExtension(model.FileName);
                    var path = Path.Combine(_out, file + "." + _ext);

                    generatedFiles.Add(model.FileName);
                    await File.WriteAllTextAsync(path, data);

                    AnsiConsole.MarkupLine($"[green]Generated:[/] {path}");
                }
                catch (Exception e)
                {
                    AnsiConsole.MarkupLine($"[red]Error generating file '{_targetFileName}':[/] {e.Message}");
                }
            }
        }

        private IEnumerable<MavlinkProtocolModel> Generate(string name)
        {
            var path = Path.Combine(_in, name);
            AnsiConsole.MarkupLine($"[blue]Processing:[/] {path}");
            MavlinkProtocolModel model;

            try
            {
                model = MavlinkGenerator.ParseXml(name, File.OpenRead(path));
            }
            catch (Exception e)
            {
                AnsiConsole.MarkupLine($"[red]Error parsing XML file '{path}':[/] {e.Message}");
                throw;
            }

            yield return model;

            foreach (var include in model.Include)
            {
                IEnumerable<MavlinkProtocolModel> protoModel;
                try
                {
                    protoModel = Generate(include);
                }
                catch (Exception e)
                {
                    AnsiConsole.MarkupLine($"[red]Error generating include file '{include}':[/] {e.Message}");
                    throw;
                }

                foreach (var protocolModel in protoModel)
                {
                    yield return protocolModel;
                }
            }
        }
    }
}