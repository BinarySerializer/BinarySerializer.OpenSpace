namespace BinarySerializer.OpenSpace
{
    public class TTGameTableValue : BinarySerializable
    {
        public TTPointerData Pointer { get; set; }
        public TTSAIFlags Flags { get; set; }
        public byte Byte_08 { get; set; } // Unknown - always 0?
        public TTDataType DataType { get; set; }

        public ulong IntegerValue { get; set; }
        public uint ArrayValueLength { get; set; }
        public TTPointerData PointerValue { get; set; }
        public byte[] ArrayValue { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Pointer = s.SerializeObject<TTPointerData>(Pointer, name: nameof(Pointer));
            Flags = s.Serialize<TTSAIFlags>(Flags, name: nameof(Flags));
            Byte_08 = s.Serialize<byte>(Byte_08, name: nameof(Byte_08));
            DataType = s.Serialize<TTDataType>(DataType, name: nameof(DataType));

            switch (DataType)
            {
                case TTDataType.Type8:
                    IntegerValue = s.Serialize<byte>((byte)IntegerValue, name: nameof(IntegerValue));
                    break;

                case TTDataType.Type16:
                    IntegerValue = s.Serialize<ushort>((ushort)IntegerValue, name: nameof(IntegerValue));
                    break;

                case TTDataType.Type32:
                    IntegerValue = s.Serialize<uint>((uint)IntegerValue, name: nameof(IntegerValue));
                    break;

                case TTDataType.Type64:
                    IntegerValue = s.Serialize<ulong>(IntegerValue, name: nameof(IntegerValue));
                    break;

                case TTDataType.TypeXX:
                    ArrayValueLength = s.Serialize<uint>(ArrayValueLength, name: nameof(ArrayValueLength));
                    ArrayValue = s.SerializeArray<byte>(ArrayValue, ArrayValueLength, nameof(ArrayValue));
                    break;

                case TTDataType.TypePointer:
                    PointerValue = s.SerializeObject<TTPointerData>(PointerValue, name: nameof(PointerValue));
                    break;
            }
        }
    }
}