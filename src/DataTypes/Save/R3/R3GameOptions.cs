namespace BinarySerializer.OpenSpace
{
    // tdstGameOptions
    public class R3GameOptions : BinarySerializable
    {
        // PS2
        public string DefaultFileName { get; set; }
        public string CurrentFileName { get; set; }
        public R3SaveGameSlot[] SaveGameSlots { get; set; }
        public uint SlotsCount { get; set; }
        public uint CurrentSlot { get; set; }

        // Video options
        public uint Details { get; set; }
        public uint ScreenSize { get; set; }
        public uint Pal60 { get; set; }
        public uint ScreenSizeLoad { get; set; }

        // Sound options
        public int VolumeSound { get; set; }
        public int VolumeVoice { get; set; }
        public int VolumeMusic { get; set; }
        public int VolumeAmbiance { get; set; }
        public int VolumeMenu { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            OpenSpaceSettings settings = s.GetRequiredSettings<OpenSpaceSettings>();

            if (settings.Platform is Platform.PlayStation2 or Platform.NintendoGameCube)
            {
                int stringLength = settings.Platform == Platform.PlayStation2 ? 256 : 260;
                int slotsCount = settings.Platform == Platform.PlayStation2 ? 1 : 100;

                DefaultFileName = s.SerializeString(DefaultFileName, length: stringLength, name: nameof(DefaultFileName));
                CurrentFileName = s.SerializeString(CurrentFileName, length: stringLength, name: nameof(CurrentFileName));
                SaveGameSlots = s.SerializeObjectArray<R3SaveGameSlot>(SaveGameSlots, slotsCount, name: nameof(SaveGameSlots));
                SlotsCount = s.Serialize<uint>(SlotsCount, name: nameof(SlotsCount));
                CurrentSlot = s.Serialize<uint>(CurrentSlot, name: nameof(CurrentSlot));
            }

            Details = s.Serialize<uint>(Details, name: nameof(Details));
            ScreenSize = s.Serialize<uint>(ScreenSize, name: nameof(ScreenSize));
            Pal60 = s.Serialize<uint>(Pal60, name: nameof(Pal60));
            ScreenSizeLoad = s.Serialize<uint>(ScreenSizeLoad, name: nameof(ScreenSizeLoad));
            VolumeSound = s.Serialize<int>(VolumeSound, name: nameof(VolumeSound));
            VolumeVoice = s.Serialize<int>(VolumeVoice, name: nameof(VolumeVoice));
            VolumeMusic = s.Serialize<int>(VolumeMusic, name: nameof(VolumeMusic));
            VolumeAmbiance = s.Serialize<int>(VolumeAmbiance, name: nameof(VolumeAmbiance));
            VolumeMenu = s.Serialize<int>(VolumeMenu, name: nameof(VolumeMenu));
        }
    }
}