    using System;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Asv.IO;
    using ConsoleAppFramework;

    namespace Asv.Mavlink.Shell;

    class Program
    { 
        static async Task Main(string[] args)
        {
            Assembly.GetExecutingAssembly().PrintWelcomeToConsole();
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;
            
            var app = ConsoleApp.Create();
            app.Add<ExampleCommand>();
            app.Add<FtpTreeDirectory>();
            app.Add<FtpBrowserDirectory>();
            await app.RunAsync(["ftp-browser"]); //TODO: убрать перед ПР
            //await app.RunAsync(args);
        }
    }

