using System;

namespace BinaryMessageEncodingScheme.Codec.Utils
{
    public static class ByteArrayExtensions
    {
        public static byte[] Concat(this byte[] array1, byte[] array2)
        {
            byte[] result = new byte[array1.Length + array2.Length];
            Array.Copy(array1, 0, result, 0, array1.Length);
            Array.Copy(array2, 0, result, array1.Length, array2.Length);
            return result;
        }
    }
}
