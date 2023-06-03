namespace ImageClassification;

using Microsoft.ML;
using CsvHelper;
using System.Globalization;
using static Program;

public abstract class Classification
{
    public static void Classify(MLContext mlContext, ClassifyOptions options)
    {
        string pathToClassify;
        if (options.ExternalLocationInput != "")
        {
            if (Path.Exists(options.ExternalLocationInput))
            {
                pathToClassify = options.ExternalLocationInput;
            }
            else
            {
                Messages.PrintFilesystemError("Path does not exist, aborting.");
                return;
            }
        }
        else
        {
            pathToClassify = CLASSIFY;
        }

        var modelPath = Path.Combine(MODELS, options.InputModel);
        if (!File.Exists(modelPath))
        {
            Messages.PrintFilesystemError("Model not found, aborting.");
            return;
        }
        ITransformer trainedModel = mlContext.Model.Load(modelPath, out _);
        var dInfo = Directory.GetFiles(pathToClassify);

        var outputFile = CheckFilename(options.OutputFile);
        foreach (var file in dInfo)
        {
            var exp = Model.ClassifySingleImage(mlContext, trainedModel, file);
            ExportClassification(outputFile, exp.image, exp.label, exp.score);
        }

        Messages.Done("Classification finished.");
    }

    //Works
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

    public static void ExportClassification(string filename, string name, string label, double score)
    {
        using StreamWriter writer = new StreamWriter(filename, true);
        writer.WriteLine($"{name},{label},{score}");
    }
}

public class Record
{
    public string Name { get; set; } = null!;
    public string Label { get; set; } = null!;
    public double Score { get; set; }
}