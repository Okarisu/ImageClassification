namespace ImageClassification;

using Microsoft.ML;
using static Program;

public class Train
{
    public static void TrainModel(MLContext mlContext, TrainOptions options)
    {
        var modelPath = Path.Combine(MODELS, options.OutputModel);
        var model = Model.GenerateModel(mlContext);
        mlContext.Model.Save(model.model, model.data.Schema, modelPath);
        Messages.Done($"Model {options.OutputModel} saved to models directory.");
    }
}