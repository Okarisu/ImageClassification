using Microsoft.ML;

namespace ImageClassification;

public class Controller
{
    public static void TrainModel(MLContext mlContext, string inputDir, string modelPath)
    {
        var source = Path.Combine();
        var _model = Model.GenerateModel(mlContext);
        mlContext.Model.Save(_model.model, _model.data.Schema, modelPath);
    }

    public static void Classify(MLContext mlContext, string modelInput, string imageToClassify, string output)
    {
        ITransformer trainedModel = mlContext.Model.Load(modelInput, out _);

        Model.ClassifySingleImage(mlContext, trainedModel);

    }
}