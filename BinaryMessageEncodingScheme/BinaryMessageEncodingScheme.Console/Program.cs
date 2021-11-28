namespace BinaryMessageEncodingScheme.Console
{
    using BinaryMessageEncodingScheme.Codec;
    using BinaryMessageEncodingScheme.Codec.Utils;
    using BinaryMessageEncodingScheme.Model;
    using BinaryMessageEncodingScheme.Model.Interfaces;
    using System;

    internal class Program
    {
        static void Main(string[] args)
        {
            IMessageCodec messageCodec = new MessageCodec();

            Console.WriteLine("Do you want to Decode or Encode a message? (D/E)");

            var decodeOrEncode = Console.ReadKey();

            Console.WriteLine();

            if (decodeOrEncode.Key.ToString() == "D")
            {
                var messageForDecoding = Console.ReadLine();

                var message = messageCodec.Decode(messageForDecoding.ToByteArray());

                Console.WriteLine(message.ToString());
            }
            else if (decodeOrEncode.Key.ToString() == "E")
            {
                var message = new Message { Headers = new System.Collections.Generic.Dictionary<string, string>() };

                while (true)
                {
                    Console.WriteLine("Enter a new header name (Blank for entering payload)");

                    var headerName = Console.ReadLine();

                    if (string.IsNullOrEmpty(headerName))
                    {
                        break;
                    }

                    Console.WriteLine("Enter a new header value");

                    var headerValue = Console.ReadLine();

                    message.Headers.Add(headerName, headerValue);
                }

                Console.WriteLine("Enter payload");

                var payload = Console.ReadLine();

                message.Payload = payload.ToByteArray();

                Console.WriteLine("Here is the encoded message");

                var encoded = messageCodec.Encode(message);

                Console.WriteLine(encoded.ToStringValue());

                Console.WriteLine("Now decoding the message");

                var decoded = messageCodec.Decode(encoded);

                Console.WriteLine("Decoded message");

                Console.WriteLine(decoded.ToString());
            }
            else
            {
                Console.WriteLine("Please type only D or E");
            }
        }
    }
}
