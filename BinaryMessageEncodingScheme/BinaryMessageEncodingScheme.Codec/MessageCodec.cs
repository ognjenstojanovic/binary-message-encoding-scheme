using BinaryMessageEncodingScheme.Model;
using BinaryMessageEncodingScheme.Model.Interfaces;
using System;

namespace BinaryMessageEncodingScheme.Codec
{
    public class MessageCodec : IMessageCodec
    {
        public Message Decode(byte[] data)
        {
            throw new NotImplementedException();
        }

        public byte[] Encode(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
