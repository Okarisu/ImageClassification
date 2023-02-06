namespace ImageClassification;

using CommandLine;

[Verb("rename")]
public class RenameOptions
{
    [Option('i', "input-folder", Required = true, SetName = "rename")]
    public string InputFolder { get; set; }

    [Option('c', "naming-convention", Required = true, SetName = "rename")]
    public string Convention { get; set; }

    [Option("move", SetName = "rename")] public bool Move { get; set; }
}

[Verb("tag")]
public class TagOptions
{
    [Option('c', "naming-convention", Required = true, SetName = "tag")]
    public string Convention { get; set; }

    [Option('t', "tag-value", Required = true, SetName = "tag")]
    public string Tag { get; set; }
}

[Verb("train")]
public class TrainOptions
{
    [Option('o', "output-model", Required = true, SetName = "train")]
    public string OutputModel { get; set; }
}

[Verb("classify", isDefault: true)]
public class ClassifyOptions
{
    [Option('m', "input-model", Required = true, SetName = "classify")]
    public string InputModel { get; set; }

    [Option('o', "output-file", Required = true, SetName = "classify")]
    public string OutputFile { get; set; }
}