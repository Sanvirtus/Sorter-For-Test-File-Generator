using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sorter_For_Test_File_Generator;

public static class Program
{
    public static async Task Main(string[] args)
    {
        const string defaultFileName = "412KB.txt";
        var processorCount = Environment.ProcessorCount;
        var chunkSizeInMb = CommandLineArgsHelper.GetChunkSizeInMb(args);
        var dataPerThreadRatio = CommandLineArgsHelper.GetDataPerThreadRatio(args);
        var showRamUsage = CommandLineArgsHelper.GetShowRamUsage(args);

        var totalMemoryLimitInBytes = chunkSizeInMb * 1024 * 1024;
        var chunkSizeForThread = totalMemoryLimitInBytes / processorCount;
        var dataPerThread = (long)(chunkSizeForThread * dataPerThreadRatio);

        var filePath = GetFilePath();

        while (true)
        {
            Display.PrintHeader(filePath, defaultFileName, processorCount, chunkSizeInMb, dataPerThreadRatio,
                showRamUsage);

            if (!string.IsNullOrEmpty(filePath)) break;

            var input = Console.ReadLine();
            input = !string.IsNullOrEmpty(input) ? input : defaultFileName;

            if (!string.IsNullOrWhiteSpace(input))
            {
                var newPath = Path.Combine(Directory.GetCurrentDirectory(), input);

                if (File.Exists(newPath))
                {
                    filePath = newPath;
                    Display.PrintFilePath(filePath);
                    break;
                }
            }
        }

        var stopwatchSortedByChunks = await SortByChunks(dataPerThread, processorCount, showRamUsage, filePath);

        Display.PrintSummary(stopwatchSortedByChunks);
    }

    private static string? GetFilePath()
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        return Directory.GetFiles(currentDirectory, "*GB_*.txt")
            .OrderByDescending(File.GetCreationTime)
            .FirstOrDefault();
    }

    private static async Task<Stopwatch> SortByChunks(long dataPerThread, int processorCount, bool showRamUsage,
        string filePath)
    {
        var stopwatch = Stopwatch.StartNew();

        var sorterByChunks = new FileSorterByChunks(dataPerThread, processorCount, showRamUsage);
        await sorterByChunks.SortFileByChunksAsync(filePath, Path.ChangeExtension(filePath, ".SortedByChunks.txt"));

        stopwatch.Stop();

        return stopwatch;
    }
}