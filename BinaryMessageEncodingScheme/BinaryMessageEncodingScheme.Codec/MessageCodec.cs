using BinaryMessageEncodingScheme.Codec.Utils;
using BinaryMessageEncodingScheme.Model;
using BinaryMessageEncodingScheme.Model.Exceptions;
using BinaryMessageEncodingScheme.Model.Interfaces;
using System;
using System.Collections.Generic;

namespace BinaryMessageEncodingScheme.Codec
{
    public class MessageCodec : IMessageCodec
    {
        private readonly byte[] HeaderNameValueDelimiterBytes = Constants.HeaderNameValueDelimiter.ToByteArray();
        private readonly byte[] HeaderSeparatorBytes = Constants.HeaderSeparator.ToByteArray();


        public Message Decode(byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] Encode(Message message)
        {
            var payloadSeparator = DateTime.Now.Ticks.ToString("X").ToByteArray();

            var encodedHeaders = EncodeHeaders(message.Headers, payloadSeparator);

            var encodedPayload = EncodePayload(message.Payload, payloadSeparator);

            return encodedHeaders.Concat(encodedPayload);
        }

        private byte[] EncodePayload(byte[] payload, byte[] payloadSeparator)
        {
            if (payload.Length > Constants.MaxPayloadLength)
            {
                throw new EncodingException($"Payload can't be bigger than {Constants.MaxPayloadLength}");
            }

            return payloadSeparator.Concat(payload).Concat(payload);
        }

        private byte[] EncodeHeaders(Dictionary<string, string> headers, byte[] payloadSeparator)
        {
            if (headers.Count > Constants.MaxNumberOfHeaders)
            {
                throw new EncodingException($"Number of headers can't be bigger than {Constants.MaxNumberOfHeaders}");
            }

            byte[] result = new byte[0];

            foreach (var header in headers)
            {
                var headerName = header.Key.ToByteArray();

                if (headerName.Length > Constants.HeaderNameMaxLength)
                {
                    throw new EncodingException($"Header name can't be bigger than {Constants.HeaderNameMaxLength} bytes");
                }

                var headerValue = header.Value.ToByteArray();

                if (headerValue.Length > Constants.HeaderValueMaxLength)
                {
                    throw new EncodingException($"Header value can't be bigger than {Constants.HeaderValueMaxLength} bytes");
                }

                result = result.Concat(ConstructHeader(headerValue, headerName));
            }

            result = result.Concat(ConstructHeader(Constants.PayloadSeparatorHeaderName.ToByteArray(), payloadSeparator));

            result = result.Concat(HeaderSeparatorBytes);

            return result;
        }

        private byte[] ConstructHeader(byte[] name, byte[] value)
        {
            return name.Concat(HeaderNameValueDelimiterBytes).Concat(value).Concat(HeaderSeparatorBytes);
        }
    }
}
