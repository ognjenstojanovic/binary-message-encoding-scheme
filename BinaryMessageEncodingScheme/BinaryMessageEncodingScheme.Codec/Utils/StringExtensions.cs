namespace BinaryMessageEncodingScheme.Codec.Utils
{
    public static class StringExtensions
    {
        public static byte[] ToByteArray(this string @string)
        {
            return System.Text.Encoding.ASCII.GetBytes(@string);
        }
    }
}
