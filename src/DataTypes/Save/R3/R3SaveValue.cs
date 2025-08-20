using System;

namespace BinarySerializer.OpenSpace
{
    public class R3SaveValue : BinarySerializable
    {
        public R3SaveDataType Pre_Type { get; set; }

        public uint IntegerValue { get; set; }

        public byte StringValueLength { get; set; }
        public string StringValue { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            switch (Pre_Type)
            {
                case R3SaveDataType.Byte:
                    IntegerValue = s.Serialize<byte>((byte)IntegerValue, name: nameof(IntegerValue));
                    break;

                case R3SaveDataType.Ushort:
                    IntegerValue = s.Serialize<ushort>((ushort)IntegerValue, name: nameof(IntegerValue));
                    break;

                case R3SaveDataType.Uint:
                    IntegerValue = s.Serialize<uint>(IntegerValue, name: nameof(IntegerValue));
                    break;

                case R3SaveDataType.String:
                    StringValueLength = s.Serialize<byte>(StringValueLength, name: nameof(StringValueLength));
                    StringValue = s.SerializeString(StringValue, StringValueLength, name: nameof(StringValue));
                    break;

                // These are never used in save files, so ignore for now
                default:
                case R3SaveDataType.Float:
                case R3SaveDataType.Vector:
                    // An array of bytes of size 0x0C
                    throw new NotImplementedException($"The specified save value type {Pre_Type} is currently not supported");
            }
        }
    }
}