namespace ImageClassification;

using static Messages;
using static Program;
using CommandLine;

public class Initialization
{
    public static void CheckFilesystem()
    {
        var filesystem = new[] {ASSETS, TRAINING_IMAGES, DATA, MODELS, INPUT, OUTPUT, IMAGES_TO_PROCESS, CLASSIFY};

        if (!Directory.Exists(INCEPTION))
        {
            PrintFilesystemError(
                "Critical error: Missing Inception model! Download it at https://storage.googleapis.com/download.tensorflow.org/models/inception5h.zip");
            Directory.CreateDirectory(INCEPTION);
        }

        foreach (var dir in filesystem)
        {
            if (Directory.Exists(dir)) continue;
            Directory.CreateDirectory(dir);
            PrintFilesystemAlternationMessage(dir);
        }

        if (!File.Exists(TAGS))
            File.Create(TAGS);
    }

    public static void RenameAssets(RenameOptions options)
    {
        Prompt("== Renaming assets to " + options.Convention + ". ==\n");

        var path = IMAGES_TO_PROCESS;
        try
        {
            DirectoryInfo info = new DirectoryInfo(path);

            var i = 1;
            foreach (var file in info.GetFiles())
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
            Done("== Completed renaming assets to " + options.Convention + ". ==\n");
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
        foreach (var file in new DirectoryInfo(TRAINING_IMAGES).GetFiles())
        {
            if (!file.FullName.Contains(options.Convention)) continue;
            using StreamWriter writer = new StreamWriter(TAGS, true);
            writer.WriteLine("{0}	{1}", file.FullName, options.Tag);
        }

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
        }
    }
}