namespace BinaryMessageEncodingScheme.Model
{
    public static class Constants
    {
        public const string HeaderNameValueDelimiter = ":";

        public const string HeaderSeparator = "\r\n";

        public const string HeaderPayloadSeparator = "\n\r";

        public const int MaxNumberOfHeaders = 63;

        public const int HeaderNameMaxLength = 1023;

        public const int HeaderValueMaxLength = 1023;

        public const string PayloadSeparatorHeaderName = "PayloadSeparator";

        public const int MaxPayloadLength = 256 * 1024;
    }
}
