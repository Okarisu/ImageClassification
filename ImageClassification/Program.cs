using Classifier;

namespace ImageClassification;

using Microsoft.ML;
using Microsoft.ML.Data;

class Program
{
    public static readonly string WD = Directory.GetCurrentDirectory();
    public static readonly string ASSETS = Path.Combine(WD, "assets");
    public static readonly string TRAINING_IMAGES = Path.Combine(ASSETS, "training_images");
    public static readonly string DATA = Path.Combine(ASSETS, "data");
    public static readonly string TAGS = Path.Combine(DATA, "tags.tsv");
    public static readonly string INPUT = Path.Combine(WD, "INPUT");

    static void Main(string[] args)
    {
        Console.WriteLine(Directory.GetCurrentDirectory());

        var mlContext = new MLContext();

        Console.WriteLine(args[1]);
        if (args.Length != 0)
        {
            switch (args[1])
            {
                case "--help" or "-h":
                {
                    Terminal.GetHelp();
                    break;
                }
                case "--init":
                {
                    Init.InitProject();
                    break;
                }
                case "--train" or "-t":
                {
                    if (!(args.Contains("-i") && args.Contains("-o")))
                    {
                        Terminal.PrintArgumentError();
                        break;
                    }

                    var inputIndex = Array.IndexOf(args, "-i");
                    var inputDirectory = args[inputIndex + 1];
                    var outputIndex = Array.IndexOf(args, "-o");
                    var modelOutput = args[outputIndex + 1];
                    Controller.TrainModel(mlContext, inputDirectory, modelOutput);
                    break;
                }
                case "--classify" or "-c":
                {
                    if (!(args.Contains("-m") && args.Contains("-i") && args.Contains("-o")))
                    {
                        Terminal.PrintArgumentError();
                        break;
                    }

                    try
                    {
                        var modelIndex = Array.IndexOf(args, "-m");
                        var modelInput = args[modelIndex + 1];
                        var inputIndex = Array.IndexOf(args, "-i");
                        var imageInput = args[inputIndex + 1];
                        var outputIndex = Array.IndexOf(args, "-o");
                        var inputDirectory = args[outputIndex + 1];
                        Controller.Classify(mlContext, modelInput, imageInput, inputDirectory);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    break;
                }
            }
        }
        else
        {
            Terminal.PrintArgumentError();
            Terminal.GetHelp();
        }


        Console.ReadKey();
    }
}