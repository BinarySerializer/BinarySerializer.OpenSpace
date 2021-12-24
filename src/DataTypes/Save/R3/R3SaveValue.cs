using System;

namespace BinarySerializer.OpenSpace
{
    public class R3SaveValue : BinarySerializable
    {
        public R3SaveValueType Pre_Type { get; set; }

        public int IntValue { get; set; }

        public byte StringValueLength { get; set; }
        public string StringValue { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            switch (Pre_Type)
            {
                case R3SaveValueType.Byte:
                    IntValue = s.Serialize<byte>((byte)IntValue, name: nameof(IntValue));
                    break;

                case R3SaveValueType.Short:
                    IntValue = s.Serialize<short>((short)IntValue, name: nameof(IntValue));
                    break;

                case R3SaveValueType.Int:
                    IntValue = s.Serialize<int>(IntValue, name: nameof(IntValue));
                    break;

                case R3SaveValueType.String:
                    StringValueLength = s.Serialize<byte>(StringValueLength, name: nameof(StringValueLength));
                    StringValue = s.SerializeString(StringValue, StringValueLength, name: nameof(StringValue));
                    break;

                // These are never used in save files, so ignore for now
                default:
                case R3SaveValueType.Float:
                case R3SaveValueType.Vector:
                    // An array of bytes of size 0x0C
                    throw new NotImplementedException($"The specified save value type {Pre_Type} is currently not supported");
            }
        }
    }
}