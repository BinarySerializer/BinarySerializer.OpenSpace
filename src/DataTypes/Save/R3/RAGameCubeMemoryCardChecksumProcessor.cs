namespace BinarySerializer.OpenSpace
{
    public class RAGameCubeMemoryCardChecksumProcessor : ChecksumProcessor
    {
        public RAGameCubeMemoryCardChecksumProcessor(uint totalSize, uint chunksCount, uint chunkIndex)
        {
            TotalSize = totalSize;
            ChunksCount = chunksCount;
            ChunkIndex = chunkIndex;

            ChunkStartByteIndex = (totalSize * chunkIndex) / chunksCount;
            ChunkEndByteIndex = (totalSize * (chunkIndex + 1)) / chunksCount;
            ChunkSize = ChunkEndByteIndex - ChunkStartByteIndex;
        }

        private uint _checksumValue;

        public uint TotalSize { get; }
        public uint ChunksCount { get; }
        public uint ChunkIndex { get; }
        public uint ChunkStartByteIndex { get; }
        public uint ChunkEndByteIndex { get; }
        public uint ChunkSize { get; }

        public int CurrentByteIndex { get; set; }

        public override long CalculatedValue
        {
            get => _checksumValue;
            set => _checksumValue = (uint)value;
        }

        // Re-implemented checksum function from decompiled code:
        //public void CalculateChecksum(byte[] buffer, uint totalSize, uint chunksCount, uint[] chunkChecksums)
        //{
        //    for (uint chunkIndex = 0; chunkIndex < chunksCount; chunkIndex++)
        //    {
        //        chunkChecksums[chunkIndex] = 0;

        //        uint chunkOffset = (totalSize * chunkIndex) / chunksCount;
        //        uint nextChunkOffset = (totalSize * (chunkIndex + 1)) / chunksCount;
        //        uint chunkSize = nextChunkOffset - chunkOffset;

        //        for (int i = 0; i < chunkSize; i++)
        //        {
        //            uint add = (uint)(buffer[chunkOffset + i] << ((4 - (i % 4) - 1) * 8));
        //            chunkChecksums[chunkIndex] += add;
        //        }
        //    }
        //}

        public override void ProcessBytes(byte[] buffer, int offset, int count)
        {
            if (CurrentByteIndex >= ChunkEndByteIndex || ChunkIndex >= ChunksCount)
                return;

            int end = offset + count;
            for (int i = offset; i < end; i++)
            {
                if (CurrentByteIndex >= ChunkStartByteIndex)
                {
                    uint add = (uint)(buffer[i] << ((4 - (CurrentByteIndex % 4) - 1) * 8));
                    _checksumValue += add;
                }
                CurrentByteIndex++;

                if (CurrentByteIndex >= ChunkEndByteIndex)
                    return;
            }
        }
    }
}