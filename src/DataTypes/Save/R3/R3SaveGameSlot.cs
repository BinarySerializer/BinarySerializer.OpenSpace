namespace BinarySerializer.OpenSpace
{
    // tdstSaveGameSlot
    public class R3SaveGameSlot : BinarySerializable
    {
        public uint Date { get; set; }
        public string NewSlotName { get; set; }
        public string OldSlotName { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Date = s.Serialize<uint>(Date, name: nameof(Date));
            NewSlotName = s.SerializeString(NewSlotName, length: 2, name: nameof(NewSlotName));
            OldSlotName = s.SerializeString(OldSlotName, length: 2, name: nameof(OldSlotName));
        }
    }
}