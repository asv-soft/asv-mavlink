using System.IO;
using ConsoleAppFramework;
using ManyConsole;

namespace Asv.Mavlink.Shell
{
    public class ExampleCommand
    {
        [Command("example")]
        public  int Run()
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
