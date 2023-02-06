namespace ImageClassification;

using Microsoft.ML;
using CsvHelper;
using System.Globalization;
using static Program;

public class Classification
{
    public static void Classify(MLContext mlContext, ClassifyOptions options)
    {
        var model = Path.Combine(MODELS, options.InputModel);
        ITransformer trainedModel = mlContext.Model.Load(model, out _);
        var dInfo = Directory.GetFiles(INPUT);
        var records = new Queue<Record>();

        //string outputFile = CheckFilename(options.OutputFile);
        foreach (var file in dInfo)
        {
            var exp = Model.ClassifySingleImage(mlContext, trainedModel, file);
            //ExportClassification(outputFile, new Record{Name = exp.image, Label = exp.label, Score = exp.score});
            records.Enqueue(new Record {Name = exp.image, Label = exp.label, Score = exp.score});
        }

        ExportClassification(options.OutputFile, records);
        Messages.Done("Classification finished.");
    }

    public static void ExportClassification(string of, Queue<Record> records)
    {
        string filename = Path.Combine(OUTPUT, of + ".csv");

        if (File.Exists(filename))
        {
            filename = Path.Combine(OUTPUT, of + "_1.csv");
        }


        using StreamWriter writer = new StreamWriter(filename);
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }
    }

    public static string CheckFilename(string of)
    {
        string filename = Path.Combine(OUTPUT, of + ".csv");

        if (File.Exists(filename))
        {
            filename = Path.Combine(OUTPUT, of + "_1.csv");
        }

        return filename;
    }

    public static void ExportClassification(string filename, Record record)
    {
        using StreamWriter writer = new StreamWriter(filename);
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(new[] {record});
        }
    }
}

public class Record
{
    public string Name { get; set; }
    public string Label { get; set; }
    public double Score { get; set; }
}