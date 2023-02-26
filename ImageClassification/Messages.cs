using static ImageClassification.Program;

namespace ImageClassification;

public class Messages
{

    public static void PrintArgumentError()
    {
        Console.WriteLine("Invalid argument! Type classify --help to get more information.");
    }

    public static void PrintArgumentError(string message)
    {
        Console.WriteLine("Invalid argument! Type classify --help to get more information.");
    }

    public static void PrintFilesystemAlternationMessage(string directory)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Filesystem error: Missing directory \"{0}\", recreating", directory);
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