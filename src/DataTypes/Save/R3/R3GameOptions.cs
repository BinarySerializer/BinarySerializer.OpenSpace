namespace BinarySerializer.OpenSpace
{
    // g_stGameOptions
    public class R3GameOptions : BinarySerializable
    {
        // PS2
        public byte[] PS2_Bytes_00 { get; set; }

        // Video options (unused)
        public int Details { get; set; }
        public int Int_04 { get; set; } // Same as Rayman 2? ScreenSize and then Brightness?
        public int Int_08 { get; set; }
        public int Int_0C { get; set; }

        // Sound options
        public int SoundVolume { get; set; }
        public int VoiceVolume { get; set; }
        public int MusicVolume { get; set; }
        public int AmbianceVolume { get; set; }
        public int MenuVolume { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            OpenSpaceSettings settings = s.GetRequiredSettings<OpenSpaceSettings>();

            if (settings.Platform == Platform.PC)
            {
                Details = s.Serialize<int>(Details, name: nameof(Details));
                Int_04 = s.Serialize<int>(Int_04, name: nameof(Int_04));
                Int_08 = s.Serialize<int>(Int_08, name: nameof(Int_08));
                Int_0C = s.Serialize<int>(Int_0C, name: nameof(Int_0C));
                SoundVolume = s.Serialize<int>(SoundVolume, name: nameof(SoundVolume));
                VoiceVolume = s.Serialize<int>(VoiceVolume, name: nameof(VoiceVolume));
                MusicVolume = s.Serialize<int>(MusicVolume, name: nameof(MusicVolume));
                AmbianceVolume = s.Serialize<int>(AmbianceVolume, name: nameof(AmbianceVolume));
                MenuVolume = s.Serialize<int>(MenuVolume, name: nameof(MenuVolume));
            }
            else if (settings.Platform == Platform.PlayStation2)
            {
                PS2_Bytes_00 = s.SerializeArray<byte>(PS2_Bytes_00, 564, name: nameof(PS2_Bytes_00));
            }
        }
    }
}