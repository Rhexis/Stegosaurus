using CommandLine;

namespace Stegosaurus;

public abstract class Program
{
    private static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(Run)
            .WithNotParsed(HandleError);
    }

    private static void Run(Options options)
    {
        switch (options.Mode)
        {
            case Mode.hide:
                if (options.Type == Type.text)
                {
                    var messageBits = Message.ToBits(options.Content);
                    Hide.Bits(messageBits, options.Input, options.Output);
                }
                else
                {
                    Console.WriteLine("Unsupported type for hiding.");
                }
                break;

            case Mode.fetch:
                var hiddenMessage = Fetch.Text(options.Source);
                Console.WriteLine($"Hidden Message: [{hiddenMessage}]");
                break;

            default:
                Console.WriteLine("Invalid mode specified.");
                break;
        }
    }
    
    private static void HandleError(IEnumerable<Error> errors)
    {
        foreach (var error in errors)
        {
            Console.WriteLine($"Error: {error.Tag}");
        }
    }
}
