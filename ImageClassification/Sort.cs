using static ImageClassification.Program;

namespace ImageClassification;

using CsvHelper;
using System.Globalization;
using CommandLine;

public class Sort
{
    public static void SortImages(SortOptions options)
    {
        /*
               using (var reader = new StreamReader("path\\to\\file.csv"))
               using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
               {
                   var records = csv.GetRecords<Record>();
               }*/
        /*
        var name = new Queue<string>();
        var label = new Queue<string>();
        var score = new Queue<double>();

        using (var reader = new StreamReader(filename))
        {
            while (!reader.EndOfStream)
            {
                var splits = reader.ReadLine().Split(',');
                name.Enqueue(splits[0]);
                label.Enqueue(splits[1]);
                score.Enqueue(double.Parse(splits[2]));
            }
        }*/

        var input = Path.Combine(OUTPUT, options.InputFile);
        using (var reader = new StreamReader(input))
        {
            while (!reader.EndOfStream)
            {
                var splits = reader.ReadLine().Split(',');

                if (double.Parse(splits[2]) >= options.MinumumScore)
                {
                    MoveImage(splits[0], splits[1]);
                }
            }
        }
    }

    public static void MoveImage(string file, string label)
    {
        var dest = Path.Combine(OUTPUT, label);
        if (!Directory.Exists(dest))
        {
            Directory.CreateDirectory(dest);
        }
        
        File.Move(file, Path.Combine(dest, file));
    }
}