namespace BinarySerializer.OpenSpace
{
    public class R3GameCubeSave : BinarySerializable
    {
        public int MaxSaveFiles = 10;

        public byte[] ImageData { get; set; } // Comments, banner and icon
        public byte ExistingSaveFilesCount { get; set; }
        public bool[] ExistingSaveFiles { get; set; }
        public string[] SaveFileNames { get; set; }
        public short[] SaveFileOffsets { get; set; }

        public R3SaveFile[] SaveFiles { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ImageData = s.SerializeArray<byte>(ImageData, 0x5040, name: nameof(ImageData));
            ExistingSaveFilesCount = s.Serialize<byte>(ExistingSaveFilesCount, name: nameof(ExistingSaveFilesCount));
            ExistingSaveFiles = s.SerializeArray<bool>(ExistingSaveFiles, MaxSaveFiles, name: nameof(ExistingSaveFiles));
            SaveFileNames = s.SerializeStringArray(SaveFileNames, MaxSaveFiles, length: 7, name: nameof(SaveFileNames));
            s.SerializePadding(1);
            SaveFileOffsets = s.SerializeArray<short>(SaveFileOffsets, MaxSaveFiles, name: nameof(SaveFileOffsets));

            SaveFiles = s.InitializeArray(SaveFiles, MaxSaveFiles);
            s.DoArray(SaveFiles, (obj, i, name) =>
            {
                if (ExistingSaveFiles[i])
                    return s.DoAt(Offset + 0x5040 + SaveFileOffsets[i], () => s.SerializeObject<R3SaveFile>(obj, name: name));
                else
                    return null;
            }, name: nameof(SaveFiles));
        }
    }
}