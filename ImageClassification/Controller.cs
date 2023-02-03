using System.Collections;
using Microsoft.ML;
using static ImageClassification.Program;
using CsvHelper;
using System.Globalization;

namespace ImageClassification;

public class Controller
{
    public static void TrainModel(MLContext mlContext, TrainOptions options)
    {
        var modelPath = Path.Combine(MODELS, options.OutputModel);
        var model = Model.GenerateModel(mlContext);
        mlContext.Model.Save(model.model, model.data.Schema, modelPath);
        Terminal.Done($"Model {options.OutputModel} saved to models directory.");
    }

    public static void Classify(MLContext mlContext, ClassifyOptions options)
    {
        var model = Path.Combine(MODELS, options.InputModel);
        ITransformer trainedModel = mlContext.Model.Load(model, out _);
        var dInfo = Directory.GetFiles(INPUT);
        var records = new List<Record>();

        foreach (var file in dInfo)
        {
            var exp = Model.ClassifySingleImage(mlContext, trainedModel, file);
            records.Add(new Record {Name = exp.image, Label = exp.label, Score = exp.score});
        }

        ExportClassification(options.OutputFile, records);
        Terminal.Done("Classification finished.");
    }

    public static void ExportClassification(string of, List<Record> records)
    {
        string filename = Path.Combine(OUTPUT, of+ ".csv");

        if (!File.Exists(filename))
        {
            File.Create(filename);
        }
        else
        {
            filename += "_1";
        }
        
        using StreamWriter writer = new StreamWriter(filename);
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csv.WriteRecords(records);
        }

        
    }

    public class Record
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public double Score { get; set; }
    }
}