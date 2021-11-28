using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BinaryMessageEncodingScheme.Model
{
    public class Message
    {
        public Dictionary<string, string> Headers { get; set; }

        public byte[] Payload { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Here are the message headers:");

            foreach (var header in Headers)
            {
                stringBuilder.AppendLine("Header Name: " + header.Key);
                stringBuilder.AppendLine("Header Value: " + header.Value);
                stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine("Payload:");
            stringBuilder.AppendLine(System.Text.Encoding.ASCII.GetString(Payload));

            return stringBuilder.ToString();
        }
    }
}
