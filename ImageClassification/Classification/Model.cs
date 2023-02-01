using static ImageClassification.Program;

namespace ImageClassification;

using Microsoft.ML;
using Microsoft.ML.Data;

public class Model
{
    private static readonly string _assetsPath = ASSETS;
    private static readonly string _imagesFolder = TRAINING_IMAGES;
    private static readonly string _trainTagsTsv = TAGS;
    private static readonly string _testTagsTsv = Path.Combine(DATA, "test-tags.tsv");
    private static readonly string _predictSingleImage = Path.Combine(_imagesFolder, "girl.jpg");

    private static readonly string _inceptionTensorFlowModel =
        Path.Combine(_assetsPath, "inception", "tensorflow_inception_graph.pb");

    

    public static (ITransformer model, IDataView data) GenerateModel(MLContext mlContext)
    {
        IEstimator<ITransformer> pipeline = mlContext.Transforms.LoadImages(outputColumnName: "input",
                imageFolder: _imagesFolder, inputColumnName: nameof(ImageData.ImagePath))
            .Append(mlContext.Transforms.ResizeImages(outputColumnName: "input",
                imageWidth: InceptionSettings.ImageWidth, imageHeight: InceptionSettings.ImageHeight,
                inputColumnName: "input"))
            .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input",
                interleavePixelColors: InceptionSettings.ChannelsLast, offsetImage: InceptionSettings.Mean))
            .Append(mlContext.Model.LoadTensorFlowModel(_inceptionTensorFlowModel).ScoreTensorFlowModel(
                outputColumnNames: new[] {"softmax2_pre_activation"}, inputColumnNames: new[] {"input"},
                addBatchDimensionInput: true))
            .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelKey",
                inputColumnName: "Label"))
            .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "LabelKey",
                featureColumnName: "softmax2_pre_activation"))
            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabelValue", "PredictedLabel"))
            .AppendCacheCheckpoint(mlContext);
        IDataView trainingData = mlContext.Data.LoadFromTextFile<ImageData>(path: _trainTagsTsv, hasHeader: false);
        Console.WriteLine("=============== Training classification model ===============");
        ITransformer model = pipeline.Fit(trainingData);
        IDataView testData = mlContext.Data.LoadFromTextFile<ImageData>(path: _testTagsTsv, hasHeader: false);
        IDataView predictions = model.Transform(testData);
        IEnumerable<ImagePrediction> imagePredictionData =
            mlContext.Data.CreateEnumerable<ImagePrediction>(predictions, true);
        DisplayResults(imagePredictionData);
        Console.WriteLine("=============== Classification metrics ===============");
        MulticlassClassificationMetrics metrics =
            mlContext.MulticlassClassification.Evaluate(predictions,
                labelColumnName: "LabelKey",
                predictedLabelColumnName: "PredictedLabel");
        Console.WriteLine($"LogLoss is: {metrics.LogLoss}");
        Console.WriteLine(
            $"PerClassLogLoss is: {String.Join(" , ", metrics.PerClassLogLoss.Select(c => c.ToString()))}");
        return (model, trainingData);
    }

    public static void ClassifySingleImage(MLContext mlContext, ITransformer model)
    {
        var imageData = new ImageData()
        {
            ImagePath = _predictSingleImage
        };
        var predictor = mlContext.Model.CreatePredictionEngine<ImageData, ImagePrediction>(model);
        var prediction = predictor.Predict(imageData);
        Console.WriteLine("=============== Making single image classification ===============");
        Console.WriteLine(
            $"Image: {Path.GetFileName(imageData.ImagePath)} predicted as: {prediction.PredictedLabelValue} with score: {prediction.Score.Max()} ");
    }
    

    private static void DisplayResults(IEnumerable<ImagePrediction> imagePredictionData)
    {
        foreach (ImagePrediction prediction in imagePredictionData)
        {
            Console.WriteLine(
                $"Image: {Path.GetFileName(prediction.ImagePath)} predicted as: {prediction.PredictedLabelValue} with score: {prediction.Score.Max()} ");
        }
    }

    private struct InceptionSettings
    {
        public const int ImageHeight = 224;
        public const int ImageWidth = 224;
        public const float Mean = 117;
        public const float Scale = 1;
        public const bool ChannelsLast = true;
    }

    public class ImageData
    {
        [LoadColumn(0)] public string ImagePath;

        [LoadColumn(1)] public string Label;
    }

    public class ImagePrediction : ImageData
    {
        public float[] Score;

        public string PredictedLabelValue;
    }
}