using static ImageClassification.Initialization;
using static ImageClassification.Terminal;
using CommandLine;
using static ImageClassification.Controller;

namespace ImageClassification;

using Microsoft.ML;
using Microsoft.ML.Data;

class Program
{
    public static readonly string WD = Directory.GetCurrentDirectory();
    public static readonly string ASSETS = Path.Combine(WD, "assets");
    public static readonly string INCEPTION = Path.Combine(ASSETS, "inception");
    public static readonly string TRAINING_IMAGES = Path.Combine(ASSETS, "training_images");
    public static readonly string DATA = Path.Combine(ASSETS, "data");
    public static readonly string MODELS = Path.Combine(ASSETS, "models");
    public static readonly string TAGS = Path.Combine(DATA, "tags.tsv");
    public static readonly string INPUT = Path.Combine(WD, "INPUT");
    public static readonly string OUTPUT = Path.Combine(WD, "OUTPUT");

    static void Main(string[] args)
    {
        CheckFilesystem();
        var mlContext = new MLContext();


        Parser.Default.ParseArguments<RenameOptions, TagOptions, TrainOptions, ClassifyOptions>(args)
            .WithParsed<RenameOptions>(RenameAssets)
            .WithParsed<TagOptions>(InitTags)
            .WithParsed<TrainOptions>(options => TrainModel(mlContext, options))
            .WithParsed<ClassifyOptions>(options => Classify(mlContext, options))
            .WithNotParsed(errors => ErrHandler(errors));
    }

    public static void ErrHandler(IEnumerable<Error> errs)
    {
        foreach (var err in errs)
        {
            Console.WriteLine(err.ToString());
        }
    }
}