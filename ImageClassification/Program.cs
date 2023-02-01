using static Classifier.Init;
using static ImageClassification.Terminal;

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
    public static readonly string MODELS = Path.Combine(ASSETS, "data");
    public static readonly string TAGS = Path.Combine(DATA, "tags.tsv");
    public static readonly string INPUT = Path.Combine(WD, "INPUT");

    static void Main(string[] args)
    {
        CheckFilesystem();
        var mlContext = new MLContext();

        Console.WriteLine(args[1]);
        if (args.Length != 0)
        {
            switch (args[1])
            {
                case "--help" or "-h":
                {
                    GetHelp();
                    break;
                }
                case "--init":
                {
                    if (!(args.Contains("-r") || args.Contains("--rename") || args.Contains("-t") || args.Contains("--tag")))
                    {
                        PrintArgumentError("Missing --rename or --tag argument");
                        break;
                    }

                    if (args.Contains("-r") || args.Contains("--rename"))
                    {
                        int inputIndex;
                        if (args.Contains("-if"))
                        {
                            inputIndex = Array.IndexOf(args, "-if");
                        }
                        else if (args.Contains("--input-folder"))
                        {
                            inputIndex = Array.IndexOf(args, "--input-folder");
                        }
                        else
                        {
                            PrintArgumentError("Missing --input-folder");
                            break;
                        }
                        var inputFolder = args[inputIndex + 1];

                        int conventionIndex;
                        if (args.Contains("-nc"))
                        {
                            conventionIndex = Array.IndexOf(args, "-nc");
                        }
                        else if (args.Contains("--naming-convention"))
                        {
                            conventionIndex = Array.IndexOf(args, "--naming-convention");
                        }
                        else
                        {
                            PrintArgumentError("Missing --naming-convention argument!");
                            break;
                        }
                        var namingConvention = args[conventionIndex + 1];


                        RenameAssets(inputFolder, namingConvention);
                        
                        Console.WriteLine("Do you wish to move renamed images to training folder? [Y/n]");
                        var key = Console.ReadKey();
                        if (key.ToString()?.ToLower() == "y" || key.Key == ConsoleKey.Enter)
                        {
                            MoveImagesToTrainingFolder(inputFolder);
                        }
                    }

                    if (args.Contains("-t") || args.Contains("--tag"))
                    {
                        if (args.Contains("-nc") || args.Contains("--naming-convention"))
                        {
                            int conventionIndex;
                            if (args.Contains("-nc"))
                            {
                                conventionIndex = Array.IndexOf(args, "-nc");
                            }
                            else if (args.Contains("--naming-convention"))
                            {
                                conventionIndex = Array.IndexOf(args, "--naming-convention");
                            }
                            else
                            {
                                PrintArgumentError("Missing --naming-convention argument!");
                                break;
                            }
                            var namingConvention = args[conventionIndex + 1];

                            int tagIndex;
                            if (args.Contains("-t"))
                            {
                                tagIndex = Array.IndexOf(args, "-t");
                            }
                            else if (args.Contains("--tag"))
                            {
                                tagIndex = Array.IndexOf(args, "--tag");
                            }
                            else
                            {
                                PrintArgumentError("Missing --tag argument!");
                                break;
                            }
                            var tag = args[tagIndex + 1];
                            
                            InitTags(namingConvention, tag);
                        }

                    }

                    break;
                }
                case "--train":
                {
                    if (!(args.Contains("-i") && args.Contains("-o")))
                    {
                        PrintArgumentError();
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
                        PrintArgumentError();
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
            PrintArgumentError();
            GetHelp();
        }


        Console.ReadKey();
    }
}