using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ManyConsole;

namespace Asv.Mavlink.Shell
{
    public class GenerateCommand:ConsoleCommand
    {
        private string _in = "in";
        private string _targetFileName = "standard.xml";
        private string _out = "out";
        private string _ext;

        public GenerateCommand()
        {
            IsCommand("gen", "Generate file form MAVLink XML message definitions");
            HasLongDescription("Generate any text files from MAVLink XML message definitions (https://mavlink.io/en/messages/)\n" +
                            "Used 'liquid' syntax as template engine (http://dotliquidmarkup.org/)" +
                            "Example: gen -t=common.xml -i=in -o=out -e=cs csharp.tpl");
            HasOption("t|target=", $"Target file name. By default '{_targetFileName}'", _ => _targetFileName = _);
            HasOption("i|input=", $"Folder with source XML files. By default '{_in}'", _=> _in = _);
            HasOption("o|output=", $"Output folder with results. By default '{_out}'", _ => _out = _);
            HasOption("e|extention=", $"Output files extentions. By default used template file ext", _ => _ext = _);
            HasAdditionalArguments(1, "Liquid syntax template file, that used for generation");
        }

        public override int Run(string[] remainingArguments)
        {
            if (!Directory.Exists(_in))
            {
                Directory.CreateDirectory(_in);
            }
            if (!Directory.Exists(_out))
            {
                Directory.CreateDirectory(_out);
            }
            _ext = _ext ?? Path.GetExtension(remainingArguments.First());
            var generatedFiles = new HashSet<string>();
            foreach (var model in Generate(_targetFileName))
            {
                try
                {
                    if (generatedFiles.Contains(model.FileName))
                    {
                        Console.WriteLine($"Skip {model.FileName}");
                        continue;
                    }
                    var template = File.ReadAllText(remainingArguments.First());
                    var data = new LiquidGenerator().Generate(template, model);
                    var file = Path.GetFileNameWithoutExtension(model.FileName);
                    var path = Path.Combine(_out, file + "." + _ext);
                    generatedFiles.Add(model.FileName);
                    File.WriteAllText(path, data);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error to generate code file '{_targetFileName}':{e.Message}");
                }
                
            }
            return 0;
        }

        private IEnumerable<MavlinkProtocolModel> Generate(string name)
        {
            var path = Path.Combine(_in, name);
            Console.WriteLine(path);
            MavlinkProtocolModel model;
            try
            {
                model = MavlinkGenerator.ParseXml(name, File.OpenRead(path));
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error to parse XML file '{path}':{e.Message}");
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
                    Console.WriteLine($"Error to generate include code file '{include}':{e.Message}");
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
