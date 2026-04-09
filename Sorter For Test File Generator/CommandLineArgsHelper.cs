using System;
using System.Globalization;

namespace Sorter_For_Test_File_Generator;

public static class CommandLineArgsHelper
{
    private const string ChunkSize = "ChunkSize";
    private const long DefaultChunkSizeInMegabytes = 256;
    private const long MinChunkSizeInMegabytes = 16;
    private const long MaxChunkSizeInMegabytes = 16384;

    private const string DataPerThreadRatio = "DPTR";
    private const double DefaultDataPerThreadRatio = 0.8;
    private const double MinDataPerThreadRatio = 0.1;
    private const double MaxDataPerThreadRatio = 1;

    private const string ShowRamUsage = "ShowRamUsage";
    private const bool DefaultShowRamUsage = false;

    public static long GetChunkSizeInMb(string[] args)
    {
        return GetArgumentValue(args, ChunkSize, DefaultChunkSizeInMegabytes)
            .CheckArgumentValue(MinChunkSizeInMegabytes, MaxChunkSizeInMegabytes);
    }

    public static double GetDataPerThreadRatio(string[] args)
    {
        return GetArgumentValue(args, DataPerThreadRatio, DefaultDataPerThreadRatio)
            .CheckArgumentValue(MinDataPerThreadRatio, MaxDataPerThreadRatio);
    }

    public static bool GetShowRamUsage(string[] args)
    {
        return GetArgumentValue(args, ShowRamUsage, DefaultShowRamUsage);
    }

    private static T GetArgumentValue<T>(string[] args, string argumentName, T defaultValue)
    {
        var prefix = $"{argumentName}=";

        foreach (var arg in args)
        {
            if (arg.StartsWith(prefix))
            {
                var value = arg.AsSpan(prefix.Length);

                try
                {
                    return (T)Convert.ChangeType(value.ToString(), typeof(T), CultureInfo.InvariantCulture);
                }
                catch
                {
                    return defaultValue;
                }
            }
        }

        return defaultValue;
    }

    private static T CheckArgumentValue<T>(this T value, T minValue, T maxValue) where T : IComparable<T>
    {
        if (value.CompareTo(minValue) < 0) return minValue;

        if (value.CompareTo(maxValue) > 0) return maxValue;

        return value;
    }
}