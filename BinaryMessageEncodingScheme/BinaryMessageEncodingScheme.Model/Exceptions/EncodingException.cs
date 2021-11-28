using System;

namespace BinaryMessageEncodingScheme.Model.Exceptions
{
    public class EncodingException : Exception
    {
        public EncodingException(string message) : base(message) { }
    }
}
