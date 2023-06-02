namespace ImageClassification;

using CommandLine;

[Verb("init", isDefault: true)]
public class Init
{
}

[Verb("rename")]
public class RenameOptions
{
    [Option('f', "filename", Required = true, SetName = "rename")]
    public string Convention { get; set; }
}

[Verb("tag")]
public abstract class TagOptions
{
    [Option('f', "filename", Required = true, SetName = "tag")]
    public string Convention { get; set; }

    [Option('t', "tag", Required = true, SetName = "tag")]
    public string Tag { get; set; }
    
    [Option("move", SetName = "rename")] public bool Move { get; set; }

}

[Verb("train")]
public abstract class TrainOptions
{
    [Option('o', "output-model", Required = true, SetName = "train")]
    public string OutputModel { get; set; }
}

[Verb("classify")]
public abstract class ClassifyOptions
{
    [Option('m', "input-model", Required = true, SetName = "classify")]
    public string InputModel { get; set; }

    [Option('o', "output-file", Required = true, SetName = "classify")]
    public string OutputFile { get; set; }
    
    [Option("external-location", SetName = "classify")]
    public string ExternalLocationInput { get; set; }
}

[Verb("sort")]
public abstract class SortOptions
{
    [Option('i', "input-file", Required = true, SetName = "sort")]
    public string InputFile { get; set; } 
    [Option('s', "score", Required = true, SetName = "sort")]
    public string MinumumScore { get; set; } 
}