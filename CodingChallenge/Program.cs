using System.Xml;
using Microsoft.Extensions.Configuration;

namespace CodingChallenge;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
        var config = builder.Build();
        
        var inputDirectoryPath = config["InputDirectory"];
        if (!Directory.Exists(inputDirectoryPath))
        {
            Console.WriteLine("Could not find specified directory.");
            Console.WriteLine("Please make sure you have set the input file directory name in appsettings.json.");
            Console.WriteLine("Exiting...");
            return;
        }
        
        var outputDirectoryPath = config["OutputDirectory"] ?? "./";
        if (!Directory.Exists(outputDirectoryPath))
        {
            Directory.CreateDirectory(outputDirectoryPath);
        }

        Console.WriteLine("Found input directory, start checking for .xml files...");

        var handledFiles = new List<string>();
        
        while (true)
        {
            var filePaths = Directory.GetFiles(inputDirectoryPath);
            foreach (var filePath in filePaths.Where(filePath => !handledFiles.Contains(filePath)))
            {
                if (!filePath.ToLower().EndsWith(".xml")) continue;

                var genReportCalculator = new GenerationReportCalculator(filePath);
                genReportCalculator.ParseGenerationReport(filePath);
                
                // TODO: use values to generate output
                
                handledFiles.Add(filePath);
            }
        }
    }
}
