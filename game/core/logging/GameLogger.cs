using System;

namespace Ashbinders.Core.Logging;

public static class GameLogger
{
    public static void LogInfo(string message)
    {
        Console.WriteLine($"[INFO] [Ashbinders] {message}");
    }

    public static void LogWarning(string message)
    {
        Console.WriteLine($"[WARN] [Ashbinders] {message}");
    }

    public static void LogError(string message)
    {
        Console.Error.WriteLine($"[ERROR] [Ashbinders] {message}");
    }
}
