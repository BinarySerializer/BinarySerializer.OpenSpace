namespace BinarySerializer.OpenSpace
{
    public class CNT : BinarySerializable
    {
        public bool IsXORUsed { get; set; }
        public bool IsChecksumUsed { get; set; }
        public byte StringsXORKey { get; set; }
        public string[] Directories { get; set; }
        public CNT_File[] Files { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // Serialize the directory and file array sizes
            Directories = s.SerializeArraySize<string, int>(Directories, name: nameof(Directories));
            Files = s.SerializeArraySize<CNT_File, int>(Files, name: nameof(Files));

            // Serialize header info
            IsXORUsed = s.Serialize<bool>(IsXORUsed, name: nameof(IsXORUsed));
            IsChecksumUsed = s.Serialize<bool>(IsChecksumUsed, name: nameof(IsChecksumUsed));
            StringsXORKey = s.Serialize<byte>(StringsXORKey, name: nameof(StringsXORKey));

            // Serialize the directory paths and the checksum afterwards
            s.DoChecksum<byte>(new Checksum8Calculator(), () =>
            {
                byte key = IsXORUsed ? StringsXORKey : (byte)0;

                for (int i = 0; i < Directories.Length; i++)
                    Directories[i] = s.SerializeObject<CNT_String>(Directories[i], x => x.Pre_XORKey = key, name: $"{nameof(Directories)}[{i}]");
            }, ChecksumPlacement.After, name: "DirectoriesChecksum");

            // Serialize the file info
            Files = s.SerializeObjectArray<CNT_File>(Files, Files.Length, x => x.Pre_FileNameXORKey = IsXORUsed ? StringsXORKey : (byte)0, name: nameof(Files));
        }
    }
}