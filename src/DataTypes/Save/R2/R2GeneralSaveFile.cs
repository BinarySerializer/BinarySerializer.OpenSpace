namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// Rayman 2 save data for general .sav files on PC
    /// </summary>
    public class R2GeneralSaveFile : BinarySerializable
    {
        public byte[] Bytes_00 { get; set; }
        
        // Level to spawn on in the Hall of Doors
        public byte LastLevel { get; set; }
        
        public byte[] Bytes_04 { get; set; }

        // TODO: Parse as bool properties for each flag?
        /// <summary>
        /// The global bit array. This contains 1440 bit flags which determine things like which Lums/cages are collected, which cutscenes have been viewed etc. Some values are parsed as integers such as the record times in the walk of races.
        /// </summary>
        public int[] GlobalArray { get; set; }

        public byte[] Bytes_BF { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Bytes_00 = s.SerializeArray<byte>(Bytes_00, 3, name: nameof(Bytes_00));
            LastLevel = s.Serialize<byte>(LastLevel, name: nameof(LastLevel));
            Bytes_04 = s.SerializeArray<byte>(Bytes_04, 7, name: nameof(Bytes_04));
            GlobalArray = s.SerializeArray<int>(GlobalArray, 45, name: nameof(GlobalArray));
            Bytes_BF = s.SerializeArray<byte>(Bytes_BF, 5, name: nameof(Bytes_BF));
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