using System.Text;

namespace BinarySerializer.OpenSpace
{
    public class CNT_String : BinarySerializable
    {
        public byte Pre_XORKey { get; set; }

        public string Value { get; set; }

        public static implicit operator string(CNT_String path) => path.Value;
        public static implicit operator CNT_String(string value) => new CNT_String() { Value = value };

        public override void SerializeImpl(SerializerObject s)
        {
            ChecksumProcessor processor = s.GetProcessor<ChecksumProcessor>();

            int length = 0;

            // Serialize the length (not included in the checksum)
            if (processor == null)
                length = s.Serialize<int>(Value?.Length ?? 0, name: $"{nameof(Value)}.Length");
            else
                processor.DoInactive(() => length = s.Serialize<int>(Value?.Length ?? 0, name: $"{nameof(Value)}.Length"));

            // Serialize the string value using the xor key
            s.DoProcessed(new Xor8Processor(Pre_XORKey), () =>
            {
                Value = s.SerializeString(Value, length, encoding: Encoding.GetEncoding(1252), name: nameof(Value));
            });
        }
    }
}