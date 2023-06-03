using static ImageClassification.Messages;

namespace ImageClassification;

using Microsoft.ML;
using static Program;

public class Train
{
    public static void TrainModel(MLContext mlContext, TrainOptions options)
    {
        if (!File.Exists(Path.Combine(INCEPTION, "tensorflow_inception_graph.pb")))
        {
            PrintFilesystemError(
                "Critical error: Missing Inception model!");
            Initialization.CheckFilesystem();
        }
        var modelPath = Path.Combine(MODELS, options.OutputModel);
        var model = Model.GenerateModel(mlContext);
        mlContext.Model.Save(model.model, model.data.Schema, modelPath);
        Done($"Model {options.OutputModel} saved to models directory.");
    }
}