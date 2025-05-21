namespace BinarySerializer.OpenSpace
{
    public class CUUID : BinarySerializable, ISerializerShortLog
    {
        public uint Uint_00 { get; set; }
        public uint Uint_04 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Uint_00 = s.Serialize<uint>(Uint_00, name: nameof(Uint_00));
            Uint_04 = s.Serialize<uint>(Uint_04, name: nameof(Uint_04));
        }

        public string ShortLog => ToString();
        public override string ToString() => $"( 0x{Uint_00:X8}, 0x{Uint_04:X8} )";
    }
}