namespace ImageClassification;
using static Terminal;
using static Program;



public class Initialization
{
    public static void CheckFilesystem()
    {
        var filesystem = new[] {ASSETS, TRAINING_IMAGES, DATA, MODELS, INPUT};

        if (!Directory.Exists(INCEPTION))
        {
            PrintFilesystemError("Critical error: Missing Inception model! Download it at https://storage.googleapis.com/download.tensorflow.org/models/inception5h.zip");
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
    public static void RenameAssets(string folder, string namingConvention)
    {
        var path = Path.Combine(Program.WD, folder);
        try
        {
            DirectoryInfo info = new DirectoryInfo(path);

            var i = 1;
            foreach (var file in info.GetFiles())
            {
                File.Move(file.FullName, Path.Combine(path, namingConvention + "_" + i));
                i++;
            }
        }
        catch (DirectoryNotFoundException e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            Done("== Renaming assets in "+ Path.GetDirectoryName(path) +" to "+namingConvention+"_N completed. ==");
        }
    }
    public static void MoveImagesToTrainingFolder(string input)
    {
        foreach (var file in new DirectoryInfo(input).GetFiles())
        {
            File.Move(file.FullName, Path.Combine(TRAINING_IMAGES, file.FullName));
        }
    }
    public static void InitTags(string convention, string tag)
    {
        foreach (var file in new DirectoryInfo(TRAINING_IMAGES).GetFiles())
        {
            if (!file.FullName.Contains(convention)) continue;
            using StreamWriter writer = new StreamWriter(TAGS, true);
            writer.WriteLine("{0}	{1}", file.FullName, tag);
        }
    }
}