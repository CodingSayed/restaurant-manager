namespace Restaurant.Cli.Services;

public static class ConsoleHelpers
{
    public static string Prompt(string label, string? defaultValue = null)
    {
        Console.Write($"{label}{(defaultValue is null ? "" : $" [{defaultValue}]")}: ");
        var s = Console.ReadLine();
        return string.IsNullOrWhiteSpace(s) ? (defaultValue ?? "") : s.Trim();
    }

    public static int PromptInt(string label, int? defaultValue = null, int? min = null, int? max = null)
    {
        while (true)
        {
            var raw = Prompt(label, defaultValue?.ToString());
            if (int.TryParse(raw, out var value) &&
                (min is null || value >= min) &&
                (max is null || value <= max))
                return value;

            Console.WriteLine("Please enter a valid integer value.");
        }
    }

    public static bool Confirm(string label, bool defaultYes = true)
    {
        var suffix = defaultYes ? " [Y/n]" : " [y/N]";
        while (true)
        {
            Console.Write($"{label}{suffix}: ");
            var s = (Console.ReadLine() ?? "").Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(s)) return defaultYes;
            if (s is "y" or "yes") return true;
            if (s is "n" or "no") return false;
        }
    }

    public static void Pause()
    {
        Console.Write("\nPress Enter to continue...");
        Console.ReadLine();
    }

    public static void PrintHeader(string title)
    {
        Console.WriteLine($"\n=== {title} ===");
    }
}