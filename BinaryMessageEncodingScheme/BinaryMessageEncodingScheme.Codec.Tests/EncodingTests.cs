using BinaryMessageEncodingScheme.Codec.Utils;
using BinaryMessageEncodingScheme.Model;
using BinaryMessageEncodingScheme.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BinaryMessageEncodingScheme.Codec.Tests
{

    public class EncodingTests
    {
        private const string DummyPayload = "The quick brown fox jumps over the lazy dog.";

        [Fact]
        public void MessageCodec_EncodeMessage_TooManyHeaders_ShouldThrowEncodingException()
        {
            // Arrange
            var headers = new Dictionary<string, string>();

            for(int i = 0; i < Constants.MaxNumberOfHeaders + 1; i++)
            {
                headers.Add(i.ToString(), i.ToString());
            }

            var message = new Message { Headers = headers, Payload = DummyPayload.ToByteArray() };

            var codec = new MessageCodec();

            // Act and Assert
            Assert.Throws<EncodingException>(() => codec.Encode(message));            
        }

        [Fact]
        public void MessageCodec_EncodeMessage_HeaderNameTooBig_ShouldThrowEncodingException()
        {
            // Arrange
            var headers = new Dictionary<string, string>();
            var headerName = new StringBuilder();

            for (int i = 0; i < Constants.HeaderNameMaxLength + 1; i++)
            {
                headerName.Append("1");
            }

            headers.Add(headerName.ToString(), Guid.NewGuid().ToString());

            var message = new Message { Headers = headers, Payload = DummyPayload.ToByteArray() };

            var codec = new MessageCodec();

            // Act and Assert
            Assert.Throws<EncodingException>(() => codec.Encode(message));
        }

        [Fact]
        public void MessageCodec_EncodeMessage_HeaderValueTooBig_ShouldThrowEncodingException()
        {
            // Arrange
            var headers = new Dictionary<string, string>();
            var headerValue = new StringBuilder();

            for (int i = 0; i < Constants.HeaderNameMaxLength + 1; i++)
            {
                headerValue.Append("1");
            }

            headers.Add(Guid.NewGuid().ToString(), headerValue.ToString());

            var message = new Message { Headers = headers, Payload = DummyPayload.ToByteArray() };

            var codec = new MessageCodec();

            // Act and Assert
            Assert.Throws<EncodingException>(() => codec.Encode(message));
        }

        [Fact]
        public void MessageCodec_EncodeMessage_PayloadTooBig_ShouldThrowEncodingException()
        {
            // Arrange
            var headers = new Dictionary<string, string>();
            var payloadValue = new StringBuilder();

            for (int i = 0; i < Constants.MaxPayloadLength + 1; i++)
            {
                payloadValue.Append("1");
            }

            var message = new Message { Headers = headers, Payload = payloadValue.ToString().ToByteArray() };

            var codec = new MessageCodec();

            // Act and Assert
            Assert.Throws<EncodingException>(() => codec.Encode(message));
        }

        [Fact]
        public void MessageCodec_EncodeMessage_ShouldEncodeWithNoErrors()
        {
            // Arrange
            var headers = new Dictionary<string, string>();

            headers.Add(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            var message = new Message { Headers = headers, Payload = DummyPayload.ToByteArray() };

            var codec = new MessageCodec();

            // Act
            var encodedMessage = codec.Encode(message);

            // Assert
            Assert.NotNull(encodedMessage);
            Assert.True(encodedMessage.Length > 0);
        }
    }
}
