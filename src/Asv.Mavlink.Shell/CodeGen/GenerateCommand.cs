using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        private string _ext;

        /// <summary>
        /// Generate file form MAVLink XML message definitions.
        /// Example: gen -t=common.xml -i=in -o=out -e=cs csharp.tpl"
        /// </summary>
        /// <param name="targetFile">OnTarget file name</param>
        /// <param name="inputFolder">Folder with source XML files</param>
        /// <param name="outputFolder">Output folder with results</param>
        /// <param name="ext">Output files extentions</param>
        /// <param name="templateFile">Liquid syntax template file, that used for generation</param>
        [Command("gen")]
        public async Task RunGenerate(string targetFile, string inputFolder, string outputFolder, string ext,
            string templateFile)
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
                    if (generatedFiles.Contains(model.FileName))
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