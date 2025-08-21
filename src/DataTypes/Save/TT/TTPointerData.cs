namespace BinarySerializer.OpenSpace
{
    public class TTPointerData : BinarySerializable
    {
        public byte BlockId { get; set; }
        public byte ModuleId { get; set; }
        public uint Pointer { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            BlockId = s.Serialize<byte>(BlockId, name: nameof(BlockId));
            ModuleId = s.Serialize<byte>(ModuleId, name: nameof(ModuleId));
            Pointer = s.Serialize<uint>(Pointer, name: nameof(Pointer));
        }
    }
}