using System.CommandLine;
using System.CommandLine.Parsing;

namespace DeepFryerCli;

internal static class Program
{
    private static async Task<int> Main(string[] args)
    {
        Option<FileInfo> inputFileOption = new("--input", "-i")
        {
            Description = "File to deep fry",
            Required = true
        };

        Option<string> outputPathOption = new("--output", "-o")
        {
            Description = "Output file",
            Required = true
        };

        RootCommand rootCommand = new("Deep Fryer made for CLI")
        {
            inputFileOption,
            outputPathOption
        };

        ParseResult parseResult = rootCommand.Parse(args);
        if (parseResult.Errors.Count != 0)
        {
            foreach (ParseError error in parseResult.Errors) 
                await Console.Error.WriteLineAsync(error.Message);

            return 1;
        }

        if (parseResult.GetValue(inputFileOption) is not { } inputFile ||
            parseResult.GetValue(outputPathOption) is not { } outputPath)
        {
            await Console.Error.WriteLineAsync("Please enter a valid file input.");
            return 1;
        }

        FileCompressor compressor = new(inputFile.FullName, outputPath);
        bool success = await compressor.CompressAndOutputAsync();

        if (!success)
        {
            await Console.Error.WriteLineAsync("There was an error processing the file.");
            return 1;
        }
            
        Console.WriteLine("File has been deep fried!");
        return 0;
    }
}