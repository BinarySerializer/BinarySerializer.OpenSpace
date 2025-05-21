using System.Text;

namespace BinarySerializer.OpenSpace
{
    // Could serialize as a RIFF file, but it has some annoying differences with the size values, and custom chunks
    public class WaveFile : BinarySerializable
    {
        public uint ChunksDataSize { get; set; }
        public byte[] ChunksData { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            s.SerializeMagicString("RIFF", 4, Encoding.ASCII);
            ChunksDataSize = s.Serialize<uint>(ChunksDataSize, name: nameof(ChunksDataSize));
            ChunksData = s.SerializeArray<byte>(ChunksData, ChunksDataSize + 4, name: nameof(ChunksData));
        }
    }
}