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
                "Critical error: Missing Inception model! Download it at https://storage.googleapis.com/download.tensorflow.org/models/inception5h.zip and place it to the assets folder");
        }
        var modelPath = Path.Combine(MODELS, options.OutputModel);
        var model = Model.GenerateModel(mlContext);
        mlContext.Model.Save(model.model, model.data.Schema, modelPath);
        Done($"Model {options.OutputModel} saved to models directory.");
    }
}