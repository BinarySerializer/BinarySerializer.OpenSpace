namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// Save item in a Rayman 3 save file
    /// </summary>
    public class R3SaveElement : BinarySerializable
    {
        public byte ElementNameLength { get; set; }
        public string ElementName { get; set; }

        public R3SaveDataType DataType { get; set; }
        public byte Unknown { get; set; } // Padding?

        public byte ValuesCount { get; set; }
        public R3SaveValue[] Values { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ElementNameLength = s.Serialize<byte>(ElementNameLength, name: nameof(ElementNameLength));

            if (ElementNameLength == 0)
                return;

            ElementName = s.SerializeString(ElementName, ElementNameLength, name: nameof(ElementName));
            
            s.DoBits<byte>(b =>
            {
                DataType = b.SerializeBits<R3SaveDataType>(DataType, 4, name: nameof(DataType));
                Unknown = b.SerializeBits<byte>(Unknown, 4, name: nameof(Unknown));
            });

            ValuesCount = s.Serialize<byte>(ValuesCount, name: nameof(ValuesCount));
            Values = s.SerializeObjectArray<R3SaveValue>(Values, ValuesCount, x => x.Pre_Type = DataType, name: nameof(Values));
        }
    }
}