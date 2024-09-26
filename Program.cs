using System.Text;
using CommandLine;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

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
                    HideTextInImage(options.Input, options.Output, options.Content);
                }
                else
                {
                    Console.WriteLine("Unsupported type for hiding.");
                }
                break;

            case Mode.fetch:
                var hiddenMessage = RetrieveTextFromImage(options.Source);
                Console.WriteLine($"Hidden Message: [{hiddenMessage}]");
                break;

            default:
                Console.WriteLine("Invalid mode specified.");
                break;
        }
    }

    private static void HideTextInImage(string inputImagePath, string outputImagePath, string message)
    {
        using var image = Image.Load<Rgba32>(inputImagePath);
        var messageBits = ConvertMessageToBits(message);

        if (messageBits.Length > (image.Width * image.Height) * 3)
        {
            Console.WriteLine("Message is too long to hide in this image.");
            return;
        }

        EmbedMessageInImage(image, messageBits);
        EmbedMessageLength(image, messageBits.Length);
        image.Save(outputImagePath, new PngEncoder());
        Console.WriteLine("Message hidden in image.");
    }

    private static string RetrieveTextFromImage(string imagePath)
    {
        using var image = Image.Load<Rgba32>(imagePath);
        var messageLength = ExtractMessageLength(image);
        var messageBits = ExtractMessageFromImage(image, messageLength);

        return ConvertBitsToMessage(messageBits);
    }

    private static void EmbedMessageInImage(Image<Rgba32> image, char[] messageBits)
    {
        int messageCounter = 0;

        for (int x = 0; x < image.Width && messageCounter < messageBits.Length; x++)
        {
            for (int y = 0; y < image.Height && messageCounter < messageBits.Length; y++)
            {
                var pixel = image[x, y];
                int r = (pixel.R & ~1) | (messageBits[messageCounter++] - '0');
                int g = (pixel.G & ~1) | (messageCounter < messageBits.Length ? (messageBits[messageCounter++] - '0') : 0);
                int b = (pixel.B & ~1) | (messageCounter < messageBits.Length ? (messageBits[messageCounter++] - '0') : 0);

                image[x, y] = new Rgba32((byte)r, (byte)g, (byte)b);
            }
        }
    }

    private static char[] ConvertMessageToBits(string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var messageBits = new StringBuilder();

        foreach (var b in messageBytes)
        {
            messageBits.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
        }

        return messageBits.ToString().ToCharArray();
    }

    private static string ExtractMessageFromImage(Image<Rgba32> image, int messageLength)
    {
        StringBuilder messageBits = new StringBuilder();

        // TODO :: Store this in the image and fetch it.
        // var messageLength = 88;

        var messageCounter = 0;
        for (int x = 0; x < image.Width; x++)
        {
            if (messageCounter >= messageLength) break;
            for (int y = 0; y < image.Height; y++)
            {
                if (messageCounter >= messageLength) break;
                var pixel = image[x, y];
                messageBits.Append(pixel.R % 2);
                messageCounter++;
                if (messageCounter >= messageLength) break;
                messageBits.Append(pixel.G % 2);
                messageCounter++;
                if (messageCounter >= messageLength) break;
                messageBits.Append(pixel.B % 2);
                messageCounter++;
            }
        }

        return messageBits.ToString();
    }
    
    private static void EmbedMessageLength(Image<Rgba32> image, int length)
    {
        var lengthBits = Convert.ToString(length, 2).PadLeft(24, '0').ToCharArray();

        int lengthCounter = 0;

        // Embed length in the last 8 pixels
        for (int x = image.Width - 9; x < image.Width && lengthCounter < lengthBits.Length; x++)
        {
            for (int y = image.Height - 2; y < image.Height && lengthCounter < lengthBits.Length; y++)
            {
                var pixel = image[x, y];
                int r = (pixel.R & ~1) | (lengthBits[lengthCounter++] - '0');
                int g = (pixel.G & ~1) | (lengthCounter < lengthBits.Length ? (lengthBits[lengthCounter++] - '0') : 0);
                int b = (pixel.B & ~1) | (lengthCounter < lengthBits.Length ? (lengthBits[lengthCounter++] - '0') : 0);

                image[x, y] = new Rgba32((byte)r, (byte)g, (byte)b);
            }
        }
    }

    private static int ExtractMessageLength(Image<Rgba32> image)
    {
        StringBuilder lengthBits = new StringBuilder();

        // Extract length from the last 8 pixels
        int lengthCounter = 0;
        int maxByteLength = 24;

        for (int x = image.Width - 9; x < image.Width && lengthCounter < maxByteLength; x++)
        {
            for (int y = image.Height - 2; y < image.Height && lengthCounter < maxByteLength; y++)
            {
                if (lengthCounter >= maxByteLength) break;
                var pixel = image[x, y];
                lengthBits.Append(pixel.R % 2);
                lengthCounter++;
                if (lengthCounter >= maxByteLength) break;
                lengthBits.Append(pixel.G % 2);
                lengthCounter++;
                if (lengthCounter >= maxByteLength) break;
                lengthBits.Append(pixel.B % 2);
                lengthCounter++;
            }
        }

        return Convert.ToInt32(lengthBits.ToString(), 2);
    }
    
    private static string ConvertBitsToMessage(string bits)
    {
        var bytes = new byte[bits.Length / 8];

        for (int i = 0; i < bits.Length; i += 8)
        {
            bytes[i / 8] = Convert.ToByte(bits.Substring(i, 8), 2);
        }

        return Encoding.UTF8.GetString(bytes);
    }

    private static void HandleError(IEnumerable<Error> errors)
    {
        foreach (var error in errors)
        {
            Console.WriteLine($"Error: {error.Tag}");
        }
    }
}
