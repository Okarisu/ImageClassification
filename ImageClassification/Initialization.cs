namespace ImageClassification;

using static Messages;
using static Program;
using CommandLine;

public class Initialization
{
    public static void CheckFilesystem()
    {
        var filesystem = new[] {ASSETS, TRAINING_IMAGES, DATA, MODELS, INPUT, OUTPUT, IMAGES_TO_PROCESS, CLASSIFY};


        foreach (var dir in filesystem)
        {
            if (Directory.Exists(dir)) continue;
            Directory.CreateDirectory(dir);
            PrintFilesystemAlternationMessage(dir);
        }
    }

    public static void RenameAssets(RenameOptions options)
    {
        Prompt("== Renaming assets to " + options.Convention + ". ==\n");

        var path = IMAGES_TO_PROCESS;
        var i = 1;
        try
        {
            var info = new DirectoryInfo(path).GetFiles();

            var ext = Directory.Exists(path);

            foreach (var file in info)
            {
                File.Move(file.FullName, Path.Combine(path, options.Convention + "_" + i + file.Extension));
                i++;
            }
        }
        catch (DirectoryNotFoundException e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            Done($"== Completed renaming {i} assets to " + options.Convention + ". ==\n");
        }
    }

    private static void MoveImagesToTrainingFolder()
    {
        Prompt("== Moving tagged assets to training folder. ==\n");

        foreach (var file in new DirectoryInfo(IMAGES_TO_PROCESS).GetFiles())
        {
            File.Move(file.FullName, Path.Combine(TRAINING_IMAGES, file.Name));
        }

        Done("== Completed moving tagged assets. ==\n");
    }

    public static void InitTags(TagOptions options)
    {
        if (!File.Exists(TAGS))
            File.Create(TAGS);

        if (!File.Exists(TEST_TAGS))
            File.Create(TEST_TAGS);
        Thread.Sleep(1500);

        Queue<string> files = new Queue<string>();
        foreach (var file in new DirectoryInfo(IMAGES_TO_PROCESS).GetFiles())
        {
            if (!file.FullName.Contains(options.Convention)) continue;
            files.Enqueue(file.Name);
        }

        for (var count = 0; count < Math.Round(Convert.ToDouble(9 * files.Count / 10)); count++)
        {
            using StreamWriter writer = new StreamWriter(TAGS, true);
            writer.WriteLine("{0}\t{1}", files.Dequeue(), options.Tag);
        }

        while (files.Count > 0)
        {
            using StreamWriter writer = new StreamWriter(TEST_TAGS, true);
            writer.WriteLine("{0}\t{1}", files.Dequeue(), options.Tag);
        }
        
        Done("== Completed tagging assets. ==\n");


        if (options.Move)
        {
            MoveImagesToTrainingFolder();
        }
        else
        {
            Prompt("Do you wish to move tagged assets to training folder? [Y/n]\n");
            var inp = Console.ReadKey();
            if (inp.Key is ConsoleKey.Y or ConsoleKey.Enter)
                MoveImagesToTrainingFolder();
            else
                Prompt("Aborting...");
        }
    }
}