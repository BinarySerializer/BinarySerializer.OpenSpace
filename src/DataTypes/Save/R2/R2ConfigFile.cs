namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// Config file data for Rayman 2 .cfg files on PC
    /// </summary>
    public class R2ConfigFile : BinarySerializable
    {
        public int VersionLength { get; set; }
        public string Version { get; set; }

        /// <summary>
        /// The amount of save slots
        /// </summary>
        public int SlotCount { get; set; }

        /// <summary>
        /// The available save slots
        /// </summary>
        public R2ConfigSlot[] Slots { get; set; }

        // Always 0?
        public int UnknownInt1 { get; set; }
        public int UnknownInt2 { get; set; }

        // Value between 0-100
        public int Lumonosity { get; set; }

        // 127 is max
        public int SoundEffectVolume { get; set; }
        public int MusicVolume { get; set; }

        // Also some volume?
        public int UnknownInt3 { get; set; }

        // Joystick related?
        public byte[] UnknownBytes { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            VersionLength = s.Serialize<int>(VersionLength, name: nameof(VersionLength));
            Version = s.SerializeString(Version, VersionLength + 1, name: nameof(Version));
            SlotCount = s.Serialize<int>(SlotCount, name: nameof(SlotCount));
            Slots = s.SerializeObjectArray<R2ConfigSlot>(Slots, SlotCount, name: nameof(Slots));
            UnknownInt1 = s.Serialize<int>(UnknownInt1, name: nameof(UnknownInt1));
            UnknownInt2 = s.Serialize<int>(UnknownInt2, name: nameof(UnknownInt2));
            Lumonosity = s.Serialize<int>(Lumonosity, name: nameof(Lumonosity));
            SoundEffectVolume = s.Serialize<int>(SoundEffectVolume, name: nameof(SoundEffectVolume));
            MusicVolume = s.Serialize<int>(MusicVolume, name: nameof(MusicVolume));
            UnknownInt3 = s.Serialize<int>(UnknownInt3, name: nameof(UnknownInt3));
            UnknownBytes = s.SerializeArray<byte>(UnknownBytes, 7, name: nameof(UnknownBytes));
        }
    }
}