namespace BinarySerializer.OpenSpace
{
    // Temporary class to serialize an unknown object type
    public class RawIdObj : IdObj
    {
        public long Pre_Length { get; set; }

        public byte[] Data { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            base.SerializeImpl(s);

            Data = s.SerializeArray<byte>(Data, Pre_Length, name: nameof(Data));
        }
    }
}