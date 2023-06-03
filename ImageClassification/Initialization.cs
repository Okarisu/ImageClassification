namespace ImageClassification;

using System.Net;
using System.IO.Compression;
using static Messages;
using static Program;

public abstract class Initialization
{
    public static void CheckFilesystem()
    {
        var folders = new[]
            {ASSETS, TRAINING_IMAGES, IMAGES_TO_PROCESS, DATA, MODELS, INPUT, OUTPUT, CLASSIFY};

        var files = new[] {TAGS, TEST_TAGS};
        foreach (var fol in folders)
        {
            if (Directory.Exists(fol)) continue;
            Directory.CreateDirectory(fol);
            PrintFilesystemAlternationMessage(fol+" directory");
        }

        foreach (var file in files)
        {
            if (File.Exists(file)) continue;
            File.Create(file);
            PrintFilesystemAlternationMessage(file);

        }

        if (Directory.Exists(INCEPTION)) return;
        Directory.CreateDirectory(INCEPTION);

        DownloadInceptionModel();
        UnzipInceptionModel();
    }
    [Obsolete("Obsolete")]
    private static void DownloadInceptionModel()
    {
        PrintModelDownloadMessage("Warning: Missing inception model, downloading...");
        using var client = new WebClient();
        client.DownloadFile("https://storage.googleapis.com/download.tensorflow.org/models/inception5h.zip",
            "inception5h.zip");
        PrintModelDownloadMessage("Downloading model completed.");
    }

    private static void UnzipInceptionModel()
    {
        PrintModelDownloadMessage("Extracting model...");
        ZipFile.ExtractToDirectory("inception5h.zip", INCEPTION);
        File.Delete("inception5h.zip");
        PrintModelDownloadMessage("Extracting completed.");
    }

    public static void RenameAssets(RenameOptions options)
    {
        Prompt("== Renaming assets to " + options.Convention + ". ==\n");

        var path = IMAGES_TO_PROCESS;
        var i = 1;
        try
        {
            var info = new DirectoryInfo(path).GetFiles();

            foreach (var file in info)
            {
                File.Move(file.FullName, Path.Combine(path, options.Convention + "_" + i + file.Extension));
                i++;
            }
            Done($"== Completed renaming {i} assets to " + options.Convention + ". ==\n");
        }
        catch (DirectoryNotFoundException e)
        {
            Console.WriteLine(e);
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

        Task.Delay(500).Wait();
        if (!File.Exists(TEST_TAGS))
            File.Create(TEST_TAGS);
        Task.Delay(500).Wait();

        Queue<string> files = new();
        foreach (var file in new DirectoryInfo(IMAGES_TO_PROCESS).GetFiles())
        {
            if (!file.FullName.Contains(options.Convention)) continue;
            files.Enqueue(file.Name);
        }

        for (var count = 0; count < Math.Round(Convert.ToDouble(9 * files.Count / 10)); count++)
        {
            using var writer = new StreamWriter(TAGS, true);
            writer.WriteLine("{0}\t{1}", files.Dequeue(), options.Tag);
        }

        while (files.Count > 0)
        {
            using var writer = new StreamWriter(TEST_TAGS, true);
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