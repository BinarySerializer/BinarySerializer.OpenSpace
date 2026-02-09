namespace BinarySerializer.OpenSpace
{
    public class RAGameCubeSave : BinarySerializable
    {
        public byte[] ImageData { get; set; } // Comments, banner and icon
        public R3SaveFile SaveFile { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            // The data is split into multiple chunks for checksum calculation
            RAGameCubeMemoryCardChecksumProcessor[] checksumProcessors = new RAGameCubeMemoryCardChecksumProcessor[8];
            for (int i = 0; i < checksumProcessors.Length; i++)
            {
                checksumProcessors[i] = new RAGameCubeMemoryCardChecksumProcessor(0x7C10, 5, (uint)i);
                s.BeginProcessed(checksumProcessors[i]);
            }

            ImageData = s.SerializeArray<byte>(ImageData, 0x5840, name: nameof(ImageData));

            // Serialize the checksum. Make sure to disable the processors since these values should not be included in the calculations!
            foreach (RAGameCubeMemoryCardChecksumProcessor p in checksumProcessors)
            {
                p.IsActive = false;
                p.CurrentByteIndex += checksumProcessors.Length * 4;
            }
            foreach (RAGameCubeMemoryCardChecksumProcessor p in checksumProcessors)
            {
                p.Serialize<uint>(s, "Checksum");
            }
            foreach (RAGameCubeMemoryCardChecksumProcessor p in checksumProcessors)
            {
                p.IsActive = true;
            }

            SaveFile = s.SerializeObject<R3SaveFile>(SaveFile, name: nameof(SaveFile));

            long serializedLength = s.CurrentFileOffset - Offset.FileOffset;
            long remainingLength = 0x7C10 - serializedLength;
            if (remainingLength > 0)
                s.SerializePadding(remainingLength);

            foreach (RAGameCubeMemoryCardChecksumProcessor p in checksumProcessors)
                s.EndProcessed(p);
        }
    }
}