using System.IO;
using ManyConsole;

namespace Asv.Mavlink.Shell
{
    public class ExampleCommand:ConsoleCommand
    {
        public ExampleCommand()
        {
            IsCommand("example", "Generate files for example usage");
        }

        public override int Run(string[] remainingArguments)
        {
            Directory.CreateDirectory("in");
            File.WriteAllText("in/common.xml",Templates.common);
            File.WriteAllText("in/standard.xml", Templates.standard);
            File.WriteAllBytes("csharp.tpl", Templates.csharp);
            File.WriteAllText("generate.bat", Templates.generate);
            File.WriteAllBytes("README.md", Templates.README);
            return 0;
        }
    }
}
