using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using ConsoleAppFramework;
using Spectre.Console;

namespace Asv.Mavlink.Shell;

public class ExampleCommand
{
    private const string BaseDirectory = "in";
    private const string CommonFileName = "common.xml";
    private const string StandardFileName = "standard.xml";
    private const string CsharpFileName = "csharp.liquid";
    private const string GenerateFileName = "generate.bat";
    private const string ReadmeFileName = "README.md";

    /// <summary>
    /// Example command for the code generator
    /// </summary>
    /// <param name="directory">-d, directory where the files should be generated in the root folder</param>
    /// <param name="virtual">-v, use this parameter if you want to use a virtual file system</param>
    /// <returns></returns> 
    [Command("code-gen-example")]
    public int Run(string directory = BaseDirectory, bool @virtual = false)
    {
        try
        {
            IFileSystem fileSystem = @virtual
                ? new MockFileSystem()
                : new FileSystem();

            var commonFilePath = fileSystem.Path.Combine(directory, CommonFileName);
            var standardFilePath = fileSystem.Path.Combine(directory, StandardFileName);

            fileSystem.Directory.CreateDirectory(directory);
            fileSystem.File.WriteAllText(commonFilePath, Templates.common);
            fileSystem.File.WriteAllText(standardFilePath, Templates.standard);
            fileSystem.File.WriteAllBytes(CsharpFileName, Templates.csharp);
            fileSystem.File.WriteAllText(GenerateFileName, Templates.generate);
            fileSystem.File.WriteAllBytes(ReadmeFileName, Templates.README);
                
            if (fileSystem.Directory.Exists(directory))
            {
                LogToConsoleDirectoryCreationInfo(directory);
            }

            if (fileSystem.File.Exists(commonFilePath))
            {
                LogToConsoleFileCreationInfo(CommonFileName);
            }
            
            if (fileSystem.File.Exists(standardFilePath))
            {
                LogToConsoleFileCreationInfo(StandardFileName);
            }

            if (fileSystem.File.Exists(CsharpFileName))
            {
                LogToConsoleFileCreationInfo(CsharpFileName);
            }
            
            if (fileSystem.File.Exists(GenerateFileName))
            {
                LogToConsoleFileCreationInfo(GenerateFileName);
            }
            
            if (fileSystem.File.Exists(ReadmeFileName))
            {
                LogToConsoleFileCreationInfo(ReadmeFileName);
            }
                
            return 0;
        }
        catch (Exception e)
        {
            AnsiConsole.WriteException(e, ExceptionFormats.ShortenEverything);
            return 1;
        }
    }
        
    private static void LogToConsoleDirectoryCreationInfo(string name)
    {
        AnsiConsole.MarkupLineInterpolated($"[blue]info[/]: Directory with name [bold yellow]|{name}|[/] has been created successfully!");
    }
        
    private static void LogToConsoleFileCreationInfo(string name)
    {
        AnsiConsole.MarkupLineInterpolated($"[blue]info[/]: File with name [bold blue]|{name}|[/] has been created successfully!");
    }
}