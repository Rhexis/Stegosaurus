using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;

namespace Stegosaurus;

public static class Hide
{
    public static void Bits(char[] bits, string inputImagePath, string outputImagePath)
    {
        using var image = Image.Load<Rgba32>(inputImagePath);

        if (bits.Length > (image.Width * image.Height) * 3)
        {
            Console.WriteLine("Message is too long to hide in this image.");
            return;
        }

        EmbedBitsInImage(image, bits);
        EmbedBitsLength(image, bits.Length);
        image.Save(outputImagePath, new PngEncoder());
        Console.WriteLine("Message hidden in image.");
    }
    
    private static void EmbedBitsInImage(Image<Rgba32> image, char[] bits)
    {
        int bitCounter = 0;

        for (int x = 0; x < image.Width && bitCounter < bits.Length; x++)
        {
            for (int y = 0; y < image.Height && bitCounter < bits.Length; y++)
            {
                var pixel = image[x, y];
                // Least significant bit is used to hide/represent 0 or 1 for each bit we are hiding.
                int r = (pixel.R & ~1) | (bits[bitCounter++] - '0');
                int g = (pixel.G & ~1) | (bitCounter < bits.Length ? (bits[bitCounter++] - '0') : 0);
                int b = (pixel.B & ~1) | (bitCounter < bits.Length ? (bits[bitCounter++] - '0') : 0);

                image[x, y] = new Rgba32((byte)r, (byte)g, (byte)b);
            }
        }
    }
    
    private static void EmbedBitsLength(Image<Rgba32> image, int length)
    {
        var lengthBits = Convert.ToString(length, 2).PadLeft(24, '0').ToCharArray();

        int lengthCounter = 0;

        // Embed length in the last 8 pixels
        // (8 + 1) and (1 + 1) because of 0-based indexing 
        for (int x = image.Width - 9; x < image.Width && lengthCounter < lengthBits.Length; x++)
        {
            for (int y = image.Height - 2; y < image.Height && lengthCounter < lengthBits.Length; y++)
            {
                var pixel = image[x, y];
                // Least significant bit is used to hide/represent 0 or 1 for each bit we are hiding.
                int r = (pixel.R & ~1) | (lengthBits[lengthCounter++] - '0');
                int g = (pixel.G & ~1) | (lengthCounter < lengthBits.Length ? (lengthBits[lengthCounter++] - '0') : 0);
                int b = (pixel.B & ~1) | (lengthCounter < lengthBits.Length ? (lengthBits[lengthCounter++] - '0') : 0);

                image[x, y] = new Rgba32((byte)r, (byte)g, (byte)b);
            }
        }
    }
}