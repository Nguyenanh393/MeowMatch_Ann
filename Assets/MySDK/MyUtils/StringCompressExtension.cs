using System.IO.Compression;
using System.IO;
using System.Text;
using CompressionLevel = System.IO.Compression.CompressionLevel;
using System;

public static class StringCompressExtension
{
    public static Encoding Encoding = Encoding.UTF8;

#if Support_Brotli //If has lib support for Brolti - better compress size - not support all platform
    public static string Compress(this string value)
    {
        byte[] inputBytes = encoding.GetBytes(value);
        using (var outputStream = new MemoryStream())
        {
            using (var compressStream = new BrotliStream(outputStream, CompressionLevel.Optimal, true))
            {
                compressStream.Write(inputBytes, 0, inputBytes.Length);
            }
            return Convert.ToBase64String(outputStream.ToArray());
        }
    }

    public static string Decompress(this string compressedValue)
    {
        byte[] bytes = Convert.FromBase64String(compressedValue);

        using (var input = new MemoryStream(bytes))
        {
            using (var outputStream = new MemoryStream())
            {
                using (var compressStream = new BrotliStream(input, CompressionMode.Decompress))
                {
                    compressStream.CopyTo(outputStream);
                }
                byte[] result = outputStream.ToArray();
                return encoding.GetString(result);
            }
        }
    }
#else //Native Support Gzip
    public static string Compress(this string value)
    {
        byte[] inputBytes = Encoding.GetBytes(value);
        using (var outputStream = new MemoryStream())
        {
            using (var compressStream = new GZipStream(outputStream, CompressionLevel.Optimal, true))
            {
                compressStream.Write(inputBytes, 0, inputBytes.Length);
            }
            return Convert.ToBase64String(outputStream.ToArray());
        }
    }

    public static string Decompress(this string compressedValue)
    {
        byte[] bytes = Convert.FromBase64String(compressedValue);

        using (var input = new MemoryStream(bytes))
        {
            using (var outputStream = new MemoryStream())
            {
                using (var compressStream = new GZipStream(input, CompressionMode.Decompress))
                {
                    compressStream.CopyTo(outputStream);
                }
                byte[] result = outputStream.ToArray();
                return Encoding.GetString(result);
            }
        }
    }
#endif
}
