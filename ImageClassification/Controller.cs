using Microsoft.ML;

namespace ImageClassification;

public class Controller
{
    public static void TrainModel(MLContext mlContext, string modelName)
    {
        var modelPath = Path.Combine(Program.MODELS, modelName);
        var model = Model.GenerateModel(mlContext);
        mlContext.Model.Save(model.model, model.data.Schema, modelPath);
    }

    public static void Classify(MLContext mlContext, string modelInput, string imageToClassify, string output)
    {
        ITransformer trainedModel = mlContext.Model.Load(modelInput, out _);

        Model.ClassifySingleImage(mlContext, trainedModel);

    }
}