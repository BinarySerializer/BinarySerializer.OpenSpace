using System;

namespace BinarySerializer.OpenSpace
{
    public class CNT : BinarySerializable
    {
        public bool IsXORUsed { get; set; }
        public bool IsChecksumUsed { get; set; }
        public byte StringsXORKey { get; set; }
        public string[] Directories { get; set; }
        public CNT_File[] Files { get; set; }

        public void ReadFile(CNT_File file, Action<BinaryDeserializer> readAction, bool logIfNotFullyRead = true)
        {
            BinaryDeserializer s = Context.Deserializer;

            // Get the file start offset
            Pointer startOffset = Offset + file.FileOffset;

            // Go to the file offset
            s.Goto(startOffset);

            // Read using the xor key
            int maxXORLength = (int)(file.FileSize - file.FileSize % file.FileXORKey.Length);
            s.DoXOR(new XORArrayCalculator(file.FileXORKey, maxLength: maxXORLength), () => readAction(s));

            if (logIfNotFullyRead && s.CurrentPointer != startOffset + file.FileSize)
                s.SystemLog?.LogWarning("File {0} was not fully read", file.FileName);
        }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize the directory and file array sizes
            Directories = s.SerializeArraySize<string, int>(Directories, name: nameof(Directories));
            Files = s.SerializeArraySize<CNT_File, int>(Files, name: nameof(Files));

            // Serialize header info
            IsXORUsed = s.Serialize<bool>(IsXORUsed, name: nameof(IsXORUsed));
            IsChecksumUsed = s.Serialize<bool>(IsChecksumUsed, name: nameof(IsChecksumUsed));
            StringsXORKey = s.Serialize<byte>(StringsXORKey, name: nameof(StringsXORKey));

            // Serialize the directories
            s.DoChecksum<byte>(
                c: IsChecksumUsed ? new Checksum8Calculator() : null,
                value: default,
                placement: ChecksumPlacement.After,
                name: "DirectoriesChecksum",
                action: () =>
                {
                    byte key = IsXORUsed ? StringsXORKey : (byte)0;

                    for (int i = 0; i < Directories.Length; i++)
                        Directories[i] = s.SerializeObject<CNT_String>(Directories[i], x => x.Pre_XORKey = key, name: $"{nameof(Directories)}[{i}]");
                });

            if (!IsChecksumUsed)
                // If the checksum flag is false the checksum value has to be 0 or else the game will see it as
                // being incorrect. This is because turning off the checksum doesn't turn off the verification itself,
                // but instead turns off the calculation, thus leaving the calculated value at 0.
                s.Serialize<byte>(0, "DirectoriesChecksum");

            // Serialize the file info
            Files = s.SerializeObjectArray<CNT_File>(Files, Files.Length, x => x.Pre_FileNameXORKey = IsXORUsed ? StringsXORKey : (byte)0, name: nameof(Files));
        }
    }
}