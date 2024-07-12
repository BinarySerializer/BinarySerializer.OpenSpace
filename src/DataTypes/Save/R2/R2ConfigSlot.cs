namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// Data for a Rayman 2 config slot entry
    /// </summary>
    public class R2ConfigSlot : BinarySerializable
    {
        /// <summary>
        /// The display name for the slot, including the percentage
        /// </summary>
        public string SlotName { get; set; }

        /// <summary>
        /// The slot index
        /// </summary>
        public int SlotIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SlotName = s.SerializeString(SlotName, 11, name: nameof(SlotName));
            SlotIndex = s.Serialize<int>(SlotIndex, name: nameof(SlotIndex));
        }
    }
}