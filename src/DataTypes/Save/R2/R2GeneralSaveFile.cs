namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// Rayman 2 save data for general .sav files on PC
    /// </summary>
    public class R2GeneralSaveFile : BinarySerializable
    {
        // NOTE: All values are written in reverse order to the save file

        // Set from code in StdGame
        public byte HitPointsMaxMax { get; set; }
        public byte HitPointsMax { get; set; }
        public byte HitPoints { get; set; }

        // Remaining values are DsgVars from the Global perso
        public int LastLevel { get; set; } // Int_67
        public bool Boolean_50 { get; set; }
        public bool Boolean_49 { get; set; }
        public byte UByte_46 { get; set; }
        public byte UByte_45 { get; set; }

        /// <summary>
        /// The global bit array. This contains 1440 bit flags which determine things like which
        /// Lums/cages are collected, which cutscenes have been viewed etc. Some values are parsed
        /// as integers such as the record times in the walk of races.
        /// </summary>
        public int[] GlobalArray { get; set; } // IntegerArray_42

        public bool Boolean_20 { get; set; }
        public int Int_6 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            HitPointsMaxMax = s.Serialize<byte>(HitPointsMaxMax, name: nameof(HitPointsMaxMax));
            HitPointsMax = s.Serialize<byte>(HitPointsMax, name: nameof(HitPointsMax));
            HitPoints = s.Serialize<byte>(HitPoints, name: nameof(HitPoints));

            LastLevel = s.Serialize<int>(LastLevel, name: nameof(LastLevel));
            Boolean_50 = s.Serialize<bool>(Boolean_50, name: nameof(Boolean_50));
            Boolean_49 = s.Serialize<bool>(Boolean_49, name: nameof(Boolean_49));
            UByte_46 = s.Serialize<byte>(UByte_46, name: nameof(UByte_46));
            UByte_45 = s.Serialize<byte>(UByte_45, name: nameof(UByte_45));
            GlobalArray = s.SerializeArray<int>(GlobalArray, 45, name: nameof(GlobalArray));
            Boolean_20 = s.Serialize<bool>(Boolean_20, name: nameof(Boolean_20));
            Int_6 = s.Serialize<int>(Int_6, name: nameof(Int_6));
        }

        /// <summary>
        /// Gets the global array as bit flags
        /// </summary>
        /// <returns>The booleans, representing every bit flag</returns>
        public bool[] GlobalArrayAsBitFlags()
        {
            // Get the size of each value in bits
            const int valueSize = sizeof(int) * 8;

            // Create the array if it doesn't exist
            bool[] output = new bool[GlobalArray.Length * valueSize];

            int index = 0;

            // Get the bits from every value
            for (int i = output.Length / valueSize - 1; i >= 0; i--)
            {
                // Get the value
                var v = GlobalArray[index];

                for (int j = valueSize - 1; j >= 0; j--)
                    output[i * valueSize + j] = BitHelpers.ExtractBits(v, 1, j) == 1;

                index++;
            }

            // Return the value
            return output;
        }
    }
}