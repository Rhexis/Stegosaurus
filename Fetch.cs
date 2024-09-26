using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Stegosaurus;

public static class Fetch
{
    public static string Text(string imagePath)
    {
        using var image = Image.Load<Rgba32>(imagePath);
        var messageLength = ExtractEmbeddedBitsLength(image);
        var messageBits = ExtractMessageFromImage(image, messageLength);

        return Message.FromBits(messageBits);
    }
    
    private static int ExtractEmbeddedBitsLength(Image<Rgba32> image)
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
    
    private static string ExtractMessageFromImage(Image<Rgba32> image, int messageLength)
    {
        StringBuilder messageBits = new StringBuilder();

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
}