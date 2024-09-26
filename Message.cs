using System.Text;

namespace Stegosaurus;

public static class Message
{
    public static string FromBits(string bits)
    {
        var bytes = new byte[bits.Length / 8];

        for (int i = 0; i < bits.Length; i += 8)
        {
            bytes[i / 8] = Convert.ToByte(bits.Substring(i, 8), 2);
        }

        return Encoding.UTF8.GetString(bytes);
    }
    
    public static char[] ToBits(string message)
    {
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var messageBits = new StringBuilder();

        foreach (var b in messageBytes)
        {
            messageBits.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
        }

        return messageBits.ToString().ToCharArray();
    }
}