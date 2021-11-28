using System;

namespace BinaryMessageEncodingScheme.Model.Exceptions
{
    public class DecodingException : Exception
    {
        public DecodingException(string message) : base(message) { }
    }
}
