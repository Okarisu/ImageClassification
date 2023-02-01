namespace ImageClassification;

public class Terminal
{
    public static void GetHelp()
    {
    }

    public static void PrintArgumentError()
    {
        Console.WriteLine("Invalid argument! Type classify --help to get more information.");
    }

    public static void PrintFilesystemAlternationMessage(string directory)
    {
        var cc = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Filesystem error: Missing directory \"{0}\", recreating", directory);
        Console.ForegroundColor = cc;
    }

    public static void PrintFilesystemError()
    {

    }
}