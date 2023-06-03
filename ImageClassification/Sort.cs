using static ImageClassification.Program;

namespace ImageClassification;

using CsvHelper;
using System.Globalization;

public abstract class Sort
{
    //TODO sort
    public static void SortImages(SortOptions options)
    {
        var input = Path.Combine(OUTPUT, options.InputFile);
        if (!File.Exists(input))
        {
            Messages.PrintFilesystemError("Input file not found, aborting.");
            return;
        }
        Queue<Record> records = new();
        using (var reader = new StreamReader(input))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            foreach (var record in csv.GetRecords<Record>()) records.Enqueue(record);
        }

        for(var i = 0; i < records.Count; i++)
        {/*
            if (rec.Score >= double.Parse(options.MinumumScore))
            {
                MoveImage(rec.Name, rec.Label);
            }*/

            var r = records.Dequeue();
            if (r.Score >= double.Parse(options.MinumumScore))
            {
                MoveImage(r.Name, r.Label);

            }
        }


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
/*
        using (var reader = new StreamReader(input))
        {
            while (!reader.EndOfStream)
            {
                var splits = reader.ReadLine().Split(',');


                //Console.WriteLine($"Splits 0: {splits[0]} | splits 1: {splits[1]} | splits 2: {splits[2]}");
                //if (double.Parse(splits[2], CultureInfo.InvariantCulture) >= double.Parse(options.MinumumScore, CultureInfo.InvariantCulture))
                if (0.96 >= double.Parse(options.MinumumScore, CultureInfo.InvariantCulture))
                //if (double.Parse(splits[2], CultureInfo.InvariantCulture) >= 0.9)
                {
                    MoveImage(splits[0], splits[1]);
                }
            }
        }*/
    }

    public static void MoveImage(string file, string label)
    {
        var src = Path.Combine(INPUT, file);
        var dest = Path.Combine(OUTPUT, label);
        if (!Directory.Exists(dest))
        {
            Directory.CreateDirectory(dest);
        }

        File.Copy(src, Path.Combine(dest, file));
    }
}