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
            var (headers, currentIndex) = DecodeHeaders(data);

            var payloadSeparator = headers.GetValueOrDefault(Constants.PayloadSeparatorHeaderName);

            var payload = DecodePayload(data, currentIndex, payloadSeparator);

            return new Message
            {
                Headers = headers,
                Payload = payload
            };
        }

        private byte[] DecodePayload(byte[] data, int currentIndex, string payloadSeparator)
        {
            byte[] currentValueBytes = new byte[0];

            byte[] payload = new byte[0];

            bool payloadReadStarted = false;

            while (currentIndex < data.Length)
            {
                currentValueBytes = currentValueBytes.Concat(new byte[] { data[currentIndex++] });

                if (currentValueBytes.Length > Constants.MaxPayloadLength)
                {
                    throw new DecodingException($"Payload can't be longer than {Constants.MaxPayloadLength} bytes");
                }

                var currentValueString = currentValueBytes.ToStringValue();

                if (currentValueString.EndsWith(payloadSeparator) && !payloadReadStarted)
                {
                    currentValueBytes = new byte[0];
                    payloadReadStarted = true;
                }
                else if (currentValueString.EndsWith(payloadSeparator) && payloadReadStarted)
                {
                    payload = currentValueString.Substring(0, currentValueString.Length - payloadSeparator.Length).ToByteArray();
                    break;
                }
            }

            if (payload.Length == 0)
            {
                throw new DecodingException("Message is incomplete");
            }

            return payload;
        }

        private (Dictionary<string, string>, int) DecodeHeaders(byte[] data)
        {
            var headers = new Dictionary<string, string>();

            byte[] currentValueBytes = new byte[0];
            (string name, string value) currentHeader = ("", "");

            var currentIndex = 0;

            while (currentIndex < data.Length)
            {
                if (headers.Count > Constants.MaxNumberOfHeaders)
                {
                    throw new DecodingException($"Maximum number of headers is {Constants.MaxNumberOfHeaders}");
                }

                currentValueBytes = currentValueBytes.Concat(new byte[] { data[currentIndex++] });

                var currentValueString = currentValueBytes.ToStringValue();

                if (currentValueString.EndsWith(Constants.HeaderNameValueDelimiter))
                {
                    currentHeader.name = currentValueString.Substring(0, currentValueString.Length - Constants.HeaderNameValueDelimiter.Length);
                    currentValueBytes = new byte[0];
                }
                else if (currentValueString.EndsWith(Constants.HeaderSeparator))
                {
                    currentHeader.value = currentValueString.Substring(0, currentValueString.Length - Constants.HeaderSeparator.Length);
                    headers.Add(currentHeader.name, currentHeader.value);
                    currentValueBytes = new byte[0];
                }
                else if (currentValueString.EndsWith(Constants.HeaderPayloadSeparator))
                {
                    currentHeader.value = currentValueString.Substring(0, currentValueString.Length - Constants.HeaderPayloadSeparator.Length);
                    headers.Add(currentHeader.name, currentHeader.value);
                    break;
                }
                else if (currentHeader.name == string.Empty && currentValueBytes.Length > Constants.HeaderNameMaxLength)
                {
                    throw new DecodingException($"Header name can't be longer than {Constants.HeaderNameMaxLength}");
                }
                else if (currentHeader.value == string.Empty && currentValueBytes.Length > Constants.HeaderValueMaxLength)
                {
                    throw new DecodingException($"Header value can't be longer than {Constants.HeaderValueMaxLength}");
                }
            }

            if (currentIndex >= data.Length)
            {
                throw new DecodingException("Message is incomplete");
            }

            if (!headers.ContainsKey(Constants.PayloadSeparatorHeaderName))
            {
                throw new DecodingException("Headers must contain a separator");
            }

            return (headers, currentIndex);
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

            return payloadSeparator.Concat(payload).Concat(payloadSeparator);
        }

        private byte[] EncodeHeaders(Dictionary<string, string> headers, byte[] payloadSeparator)
        {
            if (headers.Count > Constants.MaxNumberOfHeaders)
            {
                throw new EncodingException($"Number of headers can't be bigger than {Constants.MaxNumberOfHeaders - 1}");
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

                result = result.Concat(ConstructHeader(headerName, headerValue));
            }

            result = result.Concat(ConstructFinalHeader(payloadSeparator));

            return result;
        }

        private byte[] ConstructHeader(byte[] name, byte[] value)
        {
            return name.Concat(HeaderNameValueDelimiterBytes).Concat(value).Concat(HeaderSeparatorBytes);
        }

        private byte[] ConstructFinalHeader(byte[] value)
        {
            return Constants.PayloadSeparatorHeaderName.ToByteArray()
                    .Concat(HeaderNameValueDelimiterBytes)
                    .Concat(value)
                    .Concat(Constants.HeaderPayloadSeparator.ToByteArray());
        }
    }
}
