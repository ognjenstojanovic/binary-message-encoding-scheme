using BinaryMessageEncodingScheme.Codec.Utils;
using BinaryMessageEncodingScheme.Model;
using System;
using System.Collections.Generic;
using Xunit;

namespace BinaryMessageEncodingScheme.Codec.Tests
{
    public class EncodeDecodeTests
    {
        private const string DummyPayload = "The quick brown fox jumps over the lazy dog.";

        [Fact]
        public void MessageCodec_Encode_Decode_MessageShouldRemainTheSame()
        {
            // Arrange
            var message = new Message
            {
                Headers = new Dictionary<string, string> { { Guid.NewGuid().ToString(), Guid.NewGuid().ToString() } },
                Payload = DummyPayload.ToByteArray()
            };

            var codec = new MessageCodec();

            // Act
            var encodedMessage = codec.Encode(message);
            var decodedMessage = codec.Decode(encodedMessage);

            // Assert
            Assert.NotNull(decodedMessage);
            Assert.NotEmpty(decodedMessage.Headers);
            Assert.Equal(message.Headers.Count, decodedMessage.Headers.Count);
            
            foreach (var header in message.Headers)
            {
                var decodedHeaderValue = decodedMessage.Headers[header.Key];
                Assert.Equal(header.Value, decodedHeaderValue);
            }

            Assert.Equal(message.Payload, decodedMessage.Payload);
        }
    }
}
