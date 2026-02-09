using System;
using System.Runtime.Serialization;

namespace BinarySerializer.OpenSpace
{
    // tdstGameOptions
    public class R3GameOptions : BinarySerializable
    {
        // PC
        public string Header { get; set; }
        public int Date { get; set; }
        public int Time { get; set; }

        [IgnoreDataMember]
        public DateTime SaveDateTime
        {
            get
            {
                int day = Date % 31;

                if (day == 0)
                    day = 31;

                int calc1_1 = (Date - day) / 31;
                int month = calc1_1 % 12;

                if (month == 0)
                    month = 12;

                int year = (calc1_1 - month) / 12;

                int milliSeconds = Time % 1000;
                int calc2_1 = (Time - milliSeconds) / 1000;
                int seconds = calc2_1 % 60;
                int calc2_2 = (calc2_1 - seconds) / 60;
                int minute = calc2_2 % 60;
                int hour = (calc2_2 - minute) / 60;

                return new DateTime(year, month, day, hour, minute, seconds, milliSeconds);
            }
            set
            {
                Date = value.Day + (31 * (value.Month + (12 * value.Year)));
                Time = value.Millisecond + (1000 * (value.Second + (60 * (value.Minute + (60 * value.Hour)))));
            }
        }

        // PS2/GameCube
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

            if (settings.EngineVersion is EngineVersion.RaymanM or EngineVersion.RaymanArena &&
                settings.Platform == Platform.PC)
            {
                Header = s.SerializeString(Header, 12, name: nameof(Header));
                Date = s.Serialize<int>(Date, name: nameof(Date));
                Time = s.Serialize<int>(Time, name: nameof(Time));
            }
            else if (settings.Platform is Platform.PlayStation2 or Platform.NintendoGameCube)
            {
                int stringLength = settings.Platform == Platform.PlayStation2 ? 256 : 260;
                int slotsCount = settings.EngineVersion == EngineVersion.Rayman3 && settings.Platform == Platform.PlayStation2 ? 1 : 100;

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