using static ImageClassification.Sort;

namespace ImageClassification;

using CommandLine;
using Microsoft.ML;
using static Initialization;
using static Classification;
using static Train;

class Program
{
    public static readonly string WD = Directory.GetCurrentDirectory();
    public static readonly string ASSETS = Path.Combine(WD, "assets");
    public static readonly string INCEPTION = Path.Combine(ASSETS, "inception");
    public static readonly string TRAINING_IMAGES = Path.Combine(ASSETS, "training_images");
    public static readonly string IMAGES_TO_PROCESS = Path.Combine(ASSETS, "images_to_process");
    public static readonly string DATA = Path.Combine(ASSETS, "data");
    public static readonly string MODELS = Path.Combine(ASSETS, "models");
    public static readonly string TAGS = Path.Combine(DATA, "tags.tsv");
    public static readonly string TEST_TAGS = Path.Combine(DATA, "test_tags.tsv");
    public static readonly string INPUT = Path.Combine(WD, "INPUT");
    public static readonly string CLASSIFY = Path.Combine(INPUT, "CLASSIFY");
    public static readonly string OUTPUT = Path.Combine(WD, "OUTPUT");
    
    public static ConsoleColor CC;


    static void Main(string[] args)
    {
        CC = Console.ForegroundColor;
        CheckFilesystem();
        var mlContext = new MLContext();


        Parser.Default.ParseArguments<RenameOptions, TagOptions, TrainOptions, ClassifyOptions, SortOptions>(args)
            .WithParsed<RenameOptions>(RenameAssets)
            .WithParsed<TagOptions>(InitTags)
            .WithParsed<TrainOptions>(options => TrainModel(mlContext, options))
            .WithParsed<ClassifyOptions>(options => Classify(mlContext, options))
            .WithParsed<SortOptions>(SortImages)
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