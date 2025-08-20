using System;

namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// The data for a Rayman M/Arena save file
    /// </summary>
    public class RMSaveFile : BinarySerializable
    {
        public string Header { get; set; }

        public int Date { get; set; }
        public int Time { get; set; }

        public int Int_14 { get; set; }
        public int Int_18 { get; set; }
        public int Int_1C { get; set; }
        public int Int_20 { get; set; }
        public int Int_24 { get; set; }
        public int Int_28 { get; set; }
        public int Int_2C { get; set; }
        public int Int_30 { get; set; }
        public int Int_34 { get; set; }

        public byte[] PS2_Bytes_00 { get; set; }

        public R3SaveElement[] Elements { get; set; }

        /// <summary>
        /// The time the save was last modified
        /// </summary>
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

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public override void SerializeImpl(SerializerObject s)
        {
            OpenSpaceSettings settings = s.GetRequiredSettings<OpenSpaceSettings>();

            if (settings.Platform == Platform.PlayStation2)
            {
                PS2_Bytes_00 = s.SerializeArray<byte>(PS2_Bytes_00, 2976, name: nameof(PS2_Bytes_00));
            }
            else
            {
                // Serialize header
                Header = s.SerializeString(Header, 12, name: nameof(Header));
                Date = s.Serialize<int>(Date, name: nameof(Date));
                Time = s.Serialize<int>(Time, name: nameof(Time));

                // Serialize unknown data
                Int_14 = s.Serialize<int>(Int_14, name: nameof(Int_14));
                Int_18 = s.Serialize<int>(Int_18, name: nameof(Int_18));
                Int_1C = s.Serialize<int>(Int_1C, name: nameof(Int_1C));
                Int_20 = s.Serialize<int>(Int_20, name: nameof(Int_20));
                Int_24 = s.Serialize<int>(Int_24, name: nameof(Int_24));
                Int_28 = s.Serialize<int>(Int_28, name: nameof(Int_28));
                Int_2C = s.Serialize<int>(Int_2C, name: nameof(Int_2C));
                Int_30 = s.Serialize<int>(Int_30, name: nameof(Int_30));
                Int_34 = s.Serialize<int>(Int_34, name: nameof(Int_34));
            }

            // Serialize data entries
            Elements = s.SerializeObjectArrayUntil(Elements, x => x.ElementNameLength == 0, () => new R3SaveElement(), name: nameof(Elements));
        }
    }
}