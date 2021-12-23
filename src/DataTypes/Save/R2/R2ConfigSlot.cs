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
        public string SlotDisplayName { get; set; }

        /// <summary>
        /// The slot index
        /// </summary>
        public int SlotIndex { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            SlotDisplayName = s.SerializeString(SlotDisplayName, 11, name: nameof(SlotDisplayName));
            SlotIndex = s.Serialize<int>(SlotIndex, name: nameof(SlotIndex));
        }
    }
}