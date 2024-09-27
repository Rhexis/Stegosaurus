using System.Text;

namespace Stegosaurus;

public static class Bits
{
    public static string ToMessage(string bits)
    {
        var bytes = new byte[bits.Length / 8];

        for (int i = 0; i < bits.Length; i += 8)
        {
            bytes[i / 8] = Convert.ToByte(bits.Substring(i, 8), 2);
        }

        return Encoding.UTF8.GetString(bytes);
    }
    
    public static char[] FromMessage(string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var messageBits = new StringBuilder();

        foreach (var b in messageBytes)
        {
            messageBits.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
        }

        return messageBits.ToString().ToCharArray();
    }

    public static char[] FromFile(string path)
    {
        var data = File.ReadAllBytes(path);
        var buf = new StringBuilder();

        foreach (var b in data)
        {
            var binaryStr = Convert.ToString(b, 2);
            var padStr = binaryStr.PadLeft(8, '0');
            buf.Append(padStr);
        }

        return buf.ToString().ToCharArray();
    }

    public static void ToFile(string bits, string outputFileName)
    {
        var bytes = new byte[bits.Length / 8];

        for (int i = 0; i < bits.Length; i += 8)
        {
            bytes[i / 8] = Convert.ToByte(bits.Substring(i, 8), 2);
        }
        
        File.WriteAllBytes(outputFileName, bytes);
    }
}