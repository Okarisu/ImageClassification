using static ImageClassification.Program;

namespace ImageClassification;

public abstract class Messages
{
    public static void PrintFilesystemAlternationMessage(string directory)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Filesystem warning: Missing \"{0}\", creating.", directory);
        Console.ForegroundColor = CC;
    }
    public static void PrintModelDownloadMessage(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine(msg);
        Console.ForegroundColor = CC;
    }

    public static void PrintFilesystemError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ForegroundColor = CC;
    }

    public static void Prompt(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(message);
        Console.ForegroundColor = CC;

    }

    public static void Done(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ForegroundColor = CC;
    }
}