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
        //var path = Path.Combine(Program.WD, options.InputFolder);
        var path = IMAGES_TO_PROCESS;
        try
        {
            DirectoryInfo info = new DirectoryInfo(path);

            var i = 1;
            foreach (var file in info.GetFiles())
            {
                File.Move(file.FullName, Path.Combine(path, options.Convention + "_" + i));
                i++;
            }
/*
            if (options.Move)
            {
                MoveImagesToTrainingFolder(options.InputFolder);
            }
            else
            {
                Console.WriteLine("Do you wish to move renamed images to training folder? [Y/n] ");
                var key = Console.ReadKey(false);
                if (key.ToString()?.ToLower() == "y" || key.Key == ConsoleKey.Enter)
                {
                    MoveImagesToTrainingFolder(options.InputFolder);
                }
            }*/
        }
        catch (DirectoryNotFoundException e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            Done("== Renaming assets in " + Path.GetDirectoryName(path) + " to " + options.Convention +
                 " completed. ==");
        }
    }

    private static void MoveImagesToTrainingFolder(string input)
    {
        foreach (var file in new DirectoryInfo(input).GetFiles())
        {
            File.Move(file.FullName, Path.Combine(TRAINING_IMAGES, file.FullName));
        }
    }

    public static void InitTags(TagOptions options)
    {
        foreach (var file in new DirectoryInfo(TRAINING_IMAGES).GetFiles())
        {
            if (!file.FullName.Contains(options.Convention)) continue;
            using StreamWriter writer = new StreamWriter(TAGS, true);
            writer.WriteLine("{0}	{1}", file.FullName, options.Tag);
        }
    }
}