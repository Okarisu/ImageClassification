using ImageClassification;
using static ImageClassification.Terminal;
using static ImageClassification.Program;

namespace Classifier;

public class Init
{
    public static void CheckFilesystem()
    {
        var filesystem = new[] {ASSETS, TRAINING_IMAGES, DATA, MODELS, INPUT};
        
        foreach (var dir in filesystem)
        {
            if (Directory.Exists(dir)) continue;
            Directory.CreateDirectory(dir);
            PrintFilesystemAlternationMessage(dir);
        }
    }

    public static void RenameAssets(string folder, string extension, string namingConvention)
    {
        var path = Path.Combine(Program.WD, folder);
        try
        {
            DirectoryInfo info = new DirectoryInfo(path);

            var i = 1;
            foreach (var file in info.GetFiles("*." + extension))
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
            Console.WriteLine("== Renaming assets in {0} to {1} completed. ==", Path.GetDirectoryName(path),
                namingConvention);
        }
    }

    public static void InitTags()
    {
        
    }
}