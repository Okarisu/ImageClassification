# Classifier
This program let's you train your own image classification model and then sort your gallery.
You can have multiple models trained and use the most adequate one based on the genre of images you want to process.

#### Running pre-compiled binary file
Download the binary file from release, place it to your desired folder and follow the instructions below.

#### Compiling yourself 
//TODO
### Workflow
This program composes several modules that are to be used in specific order:

- creating filesystem and downloading Tensorflow model
- renaming the training images
- assigning tags to the images
- training the model
- classifying images
- sorting images

#### Initialization
Run the program by executing `./classifier` command in the project root folder. It first creates its filesystem and downloads the [inception5h](https://storage.googleapis.com/download.tensorflow.org/models/inception5h.zip) model to its data folder.

#### Renaming images
Program uses filenames to assign tags to images in the later step. Because of this, images of the same category/tag need to be named the same.
For example, all images with bear need to be named `bear` and the ones with car need to be named `car`.
As this module of program renames all images in given folder, you need to rename them in groups! E.g. first renaming one group to `bear` and then the second one to `car`.

Copy the images you want to rename to the `assets/images_to_process` folder. **COPY ONLY THE ONES THAT ARE TO BE NAMED AND TAGGED THE SAME.**
Then, run the program using `./classifier rename -f <filename>` command.

#### Tagging images
Issue the tagging process with `./classifier tag -f <filename> -t <tag>` command. When tagging is finished, program asks you whether you want to move tagged
images to the training folder. Default option is yes, although you can choose not to, for example when you mess the tag spelling. Be aware that you need to delete
the misspelled tags from the `assets/data/tags.tsv` file in this case.

Repeat these two steps for each tag you want your model train to recognize.

#### Training the model
Training process is started using `./classifier train -o <output model name>` command. It creates classification model and saves it to `assets/trained_models` folder.

#### Classifying 
You have two options on where to classify the images - that is in project dedicated folder or in external location.
Classification module takes your trained model, classifies the images in location of your choice and prints the metrics to file you specify located in
the OUTPUT folder.

##### Using project INPUT folder
Copy/move images to the `INPUT` folder located in the root folder of the project. Then, run `./classifier classify -m <model name> -o <output file>`.

##### Specifying external location
Run the command above with `--external-location <location>` flag, `location` given complete path to the folder containing your images.

#### Sorting images
//TODO

### Resources
- Major part of the code used in this project comes from the following source:

    QUINTANILLA , Luis, WARREN, Genevieve and others. *Tutorial: ML.NET classification model to categorize images - ML.NET.* | Microsoft Learn [online]. 27 October 2022. [Accessed&nbsp;5&nbsp;May&nbsp;2023].

    Available from: https://learn.microsoft.com/en-us/dotnet/machine-learning/tutorials/image-classification

- Project uses `inception5h` model provided by [Tensorflow](https://www.tensorflow.org/).
- #### NuGet Packages
  - [Microsoft.ML](https://www.nuget.org/packages/Microsoft.ML)
  - [Microsoft.ML.ImageAnalysis](https://www.nuget.org/packages/Microsoft.ML.ImageAnalytics)
  - [Microsoft.ML.TensorFlow](https://www.nuget.org/packages/Microsoft.ML.TensorFlow)
  - [CommandLineParses](https://www.nuget.org/packages/CommandLineParser)
  - [SciSharp.TensorFlow.Redist](https://www.nuget.org/packages/SciSharp.TensorFlow.Redist)
  - Needed on Linux systems only: [SciSharp.TensorFlow.Redist-Linux.GPU](https://www.nuget.org/packages/SciSharp.TensorFlow.Redist-Linux-GPU)