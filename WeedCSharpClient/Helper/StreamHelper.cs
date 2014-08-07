using System.IO;

public static class StreamHelper
{
    /// <summary>
    /// Convert stream to byte array
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static byte[] StreamToBytes(Stream stream)
    {
        var bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        stream.Seek(0, SeekOrigin.Begin);
        return bytes;
    }

    /// <summary>
    /// Convert byte array to stream
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static Stream BytesToStream(byte[] bytes)
    {
        return new MemoryStream(bytes);
    }
}
