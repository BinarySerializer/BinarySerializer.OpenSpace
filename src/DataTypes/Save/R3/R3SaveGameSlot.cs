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
            OpenSpaceSettings settings = s.GetRequiredSettings<OpenSpaceSettings>();
            
            Date = s.Serialize<uint>(Date, name: nameof(Date));
            
            int stringLength = settings.EngineVersion == EngineVersion.Rayman3 && settings.Platform == Platform.PlayStation2 ? 2 : 10;

            NewSlotName = s.SerializeString(NewSlotName, length: stringLength, name: nameof(NewSlotName));
            OldSlotName = s.SerializeString(OldSlotName, length: stringLength, name: nameof(OldSlotName));
        }
    }
}