using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sorter_For_Test_File_Generator;

public static class Display
{
    public static void PrintHeader(string? filePath, string defaultFileName, int processorCount, long totalMemoryLimit,
        double dataPerThreadRatio, bool showRamUsage)
    {
        Console.Clear();
        PrintLineBlueText("Sorter");
        PrintRunArguments(processorCount, totalMemoryLimit, dataPerThreadRatio, showRamUsage);

        if (!string.IsNullOrEmpty(filePath))
        {
            PrintFilePath(filePath);
        }
        else
        {
            Console.Write($"Enter target file name (Or just press Enter to use the default file {defaultFileName}): ");
        }
    }

    public static void PrintFilePath(string filePath)
    {
        Console.Write("- File Full Path: ");
        PrintLineGreenText($"{filePath}");
    }

    public static void PrintPeakMemoryUsage()
    {
        using var currentProcess = Process.GetCurrentProcess();

        var peakMemoryBytes = currentProcess.PeakWorkingSet64;
        var peakMemoryMb = peakMemoryBytes / 1024.0 / 1024.0;

        PrintLineBlueText("\nMemory Performance:");
        Console.Write("- Peak Working Set (Max RAM used): ");

        PrintLineYellowText($"{peakMemoryMb:F2} MB");
    }

    public static void PrintRunArguments(int processorCount, long totalMemoryLimit, double dataPerThreadRatio,
        bool showRamUsage)
    {
        Console.Write("Processor Count: ");
        PrintLineYellowText($"{processorCount}");
        Console.Write("Memory Limit: ");
        PrintLineYellowText($"{totalMemoryLimit}MB");
        Console.Write("Data Per Thread Ratio: ");
        PrintLineYellowText($"{dataPerThreadRatio}");
        Console.Write("Show Ram Usage: ");
        PrintLineYellowText($"{showRamUsage}");
    }

    public static void PrintProgressBar(long current, long total, bool notOnlyProgress)
    {
        var ratio = total > 0 ? (double)current / total : 0;

        Console.Write("\rProgress: ");
        PrintYellowText($"{ratio:P1}");

        if (!notOnlyProgress) return;

        var memoryMb = GC.GetTotalMemory(false) / 1024 / 1024;
        var threadCount = Process.GetCurrentProcess().Threads.Count;

        Console.Write(" | RAM: ");
        PrintYellowText($"{memoryMb}MB");
        Console.Write(" | Threads: ");
        PrintYellowText($"{threadCount}");
    }

    public static void PrintSummary(Stopwatch stopwatchSortedByChunks)
    {
        PrintLineBlueText("\nExecution Summary:");

        PrintLineYellowText($"Sorting by chunks completed in {stopwatchSortedByChunks.Elapsed.TotalSeconds:F2} sec");
        PrintPeakMemoryUsage();

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    public static void PrintTempFiles(List<string> tempFiles)
    {
        foreach (var tempFile in tempFiles)
        {
            Console.Write("- ");
            PrintLineYellowText($"{tempFile}");
        }
    }

    public static void PrintLineBlueText(string text)
    {
        SetBlueConsoleColor();
        Console.WriteLine(text);
        ResetConsoleColor();
    }

    public static void PrintLineRedText(string text)
    {
        SetRedConsoleColor();
        Console.WriteLine(text);
        ResetConsoleColor();
    }

    private static void PrintLineGreenText(string text)
    {
        SetGreenConsoleColor();
        Console.WriteLine(text);
        ResetConsoleColor();
    }

    private static void PrintLineYellowText(string text)
    {
        SetYellowConsoleColor();
        Console.WriteLine(text);
        ResetConsoleColor();
    }

    private static void PrintYellowText(string text)
    {
        SetYellowConsoleColor();
        Console.Write(text);
        ResetConsoleColor();
    }

    private static void ResetConsoleColor() => Console.ResetColor();

    private static void SetBlueConsoleColor() => SetConsoleColor(ConsoleColor.Blue);

    private static void SetGreenConsoleColor() => SetConsoleColor(ConsoleColor.Green);

    private static void SetRedConsoleColor() => SetConsoleColor(ConsoleColor.Red);

    private static void SetYellowConsoleColor() => SetConsoleColor(ConsoleColor.Yellow);

    private static void SetConsoleColor(ConsoleColor color) => Console.ForegroundColor = color;
}