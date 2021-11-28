namespace BinaryMessageEncodingScheme.Model.Interfaces
{
    public interface IMessageCodec
    {
        byte[] Encode(Message message);

        Message Decode(byte[] data);
    }
}
