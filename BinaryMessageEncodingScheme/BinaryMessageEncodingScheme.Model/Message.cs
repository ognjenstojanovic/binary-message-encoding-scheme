using System.Collections.Generic;

namespace BinaryMessageEncodingScheme.Model
{
    public class Message
    {
        public Dictionary<string, string> Headers { get; set; }

        public byte[] Payload { get; set; }
    }
}
