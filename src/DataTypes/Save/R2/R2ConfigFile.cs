namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// Config file data for Rayman 2 .cfg files on PC
    /// </summary>
    public class R2ConfigFile : BinarySerializable
    {
        // Version
        public int VersionLength { get; set; }
        public string Version { get; set; }

        // Save slots
        public int SlotsCount { get; set; }
        public R2ConfigSlot[] Slots { get; set; }

        // Video options
        public int Details { get; set; }
        public int ScreenSize { get; set; }
        public int Brightness { get; set; } // Lumonosity 0-100

        // Sound options
        public int FxVolume { get; set; } // 0-127
        public int MusicVolume { get; set; } // 0-127
        public int VoiceVolume { get; set; } // 0-127

        // Joystick calibration
        public sbyte JoystickXmin { get; set; }
        public sbyte JoystickXmax { get; set; }
        public sbyte JoystickYmin { get; set; }
        public sbyte JoystickYmax { get; set; }
        public sbyte JoystickXcenter { get; set; }
        public sbyte JoystickYcenter { get; set; }

        // TODO: Would be nice to parse this, but it requires also parsing the fix.sna file
        //       since the values depend on if they're for keyboard or joypad. However, this
        //       is unused in Rayman 2 on PC, so the data is empty (0xFF).
        // Input options
        public byte[] InputOptions { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            VersionLength = s.Serialize<int>(VersionLength, name: nameof(VersionLength));
            Version = s.SerializeString(Version, VersionLength + 1, name: nameof(Version));

            SlotsCount = s.Serialize<int>(SlotsCount, name: nameof(SlotsCount));
            Slots = s.SerializeObjectArray<R2ConfigSlot>(Slots, SlotsCount, name: nameof(Slots));

            Details = s.Serialize<int>(Details, name: nameof(Details));
            ScreenSize = s.Serialize<int>(ScreenSize, name: nameof(ScreenSize));
            Brightness = s.Serialize<int>(Brightness, name: nameof(Brightness));

            FxVolume = s.Serialize<int>(FxVolume, name: nameof(FxVolume));
            MusicVolume = s.Serialize<int>(MusicVolume, name: nameof(MusicVolume));
            VoiceVolume = s.Serialize<int>(VoiceVolume, name: nameof(VoiceVolume));

            JoystickXmin = s.Serialize<sbyte>(JoystickXmin, name: nameof(JoystickXmin));
            JoystickXmax = s.Serialize<sbyte>(JoystickXmax, name: nameof(JoystickXmax));
            JoystickYmin = s.Serialize<sbyte>(JoystickYmin, name: nameof(JoystickYmin));
            JoystickYmax = s.Serialize<sbyte>(JoystickYmax, name: nameof(JoystickYmax));
            JoystickXcenter = s.Serialize<sbyte>(JoystickXcenter, name: nameof(JoystickXcenter));
            JoystickYcenter = s.Serialize<sbyte>(JoystickYcenter, name: nameof(JoystickYcenter));

            InputOptions = s.SerializeArray<byte>(InputOptions, 1, name: nameof(InputOptions));
        }
    }
}