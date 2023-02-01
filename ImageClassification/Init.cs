namespace Classifier;

public class Init
{
    public static void InitProject()
    {
        var wd = Directory.GetCurrentDirectory();
        Directory.CreateDirectory(Path.Combine(wd, "assetsToGetNameOfPikachu"));
        const string dataFolder = "data";
        var assetsFolder = Path.Combine(dataFolder, "assets");
        
        
    }
    
    public static void RenameAssets(string folder)
    {
        string path = Path.Combine();
        string namingConvention = "RuriLapisL";
        string extension;
        try
        {
            DirectoryInfo info = new DirectoryInfo(path);

            var i = 1;
            foreach (var file in info.GetFiles("*.jpg"))
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

}