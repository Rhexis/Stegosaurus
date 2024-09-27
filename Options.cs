using CommandLine;

namespace Stegosaurus;

public enum Mode
{
    hide,
    fetch
}

public enum Type
{
    image,
    text
}

public class Options
{
    [Option('m', "mode", Required = true, HelpText = "The mode of operation, 'hide' or 'fetch'.")]
    public Mode Mode { get; set; }
    
    [Option('t', "type", Required = false, HelpText = "Type of content to hide, 'text' or 'image'.")]
    public Type Type { get; set; }
    
    // For mode "fetch"
    [Option('s', "src", Required = false, HelpText = "Source of the image containing the hidden data.")]
    public string? Source { get; set; }
    
    // For mode "hide"
    [Option('c', "content", Required = false, HelpText = "Location of image or text to hide.")]
    public string? Content { get; set; }
    [Option('i', "input", Required = false, HelpText = "The image to hide content in.")]
    public string? Input { get; set; }
    [Option('o', "output", Required = false, HelpText = "The location & name of the desired output file.")]
    public string? Output { get; set; }
}