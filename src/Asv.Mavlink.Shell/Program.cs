using System;
using System.Reflection;
using System.Text;
using Asv.IO;
using ManyConsole;

namespace Asv.Mavlink.Shell
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            typeof(Program).Assembly.PrintWelcomeToConsole();

            try
            {
                var commands = ConsoleCommandDispatcher.FindCommandsInAssembly(Assembly.GetExecutingAssembly());
                return ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Unhandled exception: {0}", ex);
                return -1;
            }

        }
    }
}
