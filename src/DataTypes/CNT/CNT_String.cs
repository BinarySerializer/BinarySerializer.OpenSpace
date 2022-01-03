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
            IChecksumCalculator c = s.PauseCalculateChecksum();
            
            // Serialize the length
            int length = s.Serialize<int>(Value?.Length ?? 0, name: $"{nameof(Value)}.Length");

            s.BeginCalculateChecksum(c);

            // Serialize the string value using the xor key
            s.DoXOR(Pre_XORKey, () => Value = s.SerializeString(Value, length, name: $"{nameof(Value)}"));
        }
    }
}