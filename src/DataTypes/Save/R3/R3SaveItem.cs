namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// Save item in a Rayman 3 save file
    /// </summary>
    public class R3SaveItem : BinarySerializable
    {
        public byte KeyLength { get; set; }
        public string Key { get; set; }

        public R3SaveValueType ValueType { get; set; }
        public byte Unknown { get; set; } // Padding?

        public byte ValuesCount { get; set; }

        public R3SaveValue[] Values { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            KeyLength = s.Serialize<byte>(KeyLength, name: nameof(KeyLength));

            if (KeyLength == 0)
                return;

            Key = s.SerializeString(Key, KeyLength, name: nameof(Key));
            
            s.DoBits<byte>(b =>
            {
                ValueType = b.SerializeBits<R3SaveValueType>(ValueType, 4, name: nameof(ValueType));
                Unknown = b.SerializeBits<byte>(Unknown, 4, name: nameof(Unknown));
            });

            ValuesCount = s.Serialize<byte>(ValuesCount, name: nameof(ValuesCount));
            Values = s.SerializeObjectArray<R3SaveValue>(Values, ValuesCount, x => x.Pre_Type = ValueType, name: nameof(Values));
        }
    }
}