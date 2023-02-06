namespace ImageClassification;
using CsvHelper;
using System.Globalization;

public class Sort
{
    public static void SortImages(string filename)
    { /*
        using (var reader = new StreamReader("path\\to\\file.csv"))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<Record>();
        }*/
        
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
        }
    }
}