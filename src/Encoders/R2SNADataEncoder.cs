using System.IO;

namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// The encoder for Rayman 2 .sna data files
    /// </summary>
    public class R2SNADataEncoder : IStreamEncoder
    {
        public string Name => "R2SNAEncoding";

        public void DecodeStream(Stream input, Stream output)
        {
            // Get the initial key
            uint currentMask = 0x6AB5CC79;

            // Return the initial magic key
            output.WriteByte(0x79);
            output.WriteByte(0xCC);
            output.WriteByte(0xB5);
            output.WriteByte(0x6A);

            // Get the length
            long length = input.Length - input.Position;

            // Set the position to skip the first 4 bytes
            input.Position += 4;

            // Enumerate every byte
            for (long i = 4; i < length; i++)
            {
                // Read the byte
                byte b = (byte)input.ReadByte();

                // Decode the byte
                b ^= (byte)((currentMask >> 8) & 0xFF);

                // Write the byte
                output.WriteByte(b);

                // Update the magic key
                currentMask = 16807 * (currentMask ^ 0x75BD924) - 0x7FFFFFFF * ((currentMask ^ 0x75BD924) / 0x1F31D);

                // Use this instead for the iOS version
                //currentMask = (uint)(16807 * ((currentMask ^ 0x75BD924u) % 0x1F31D) - 2836 * ((currentMask ^ 0x75BD924u) / 0x1F31D));
            }
        }

        public void EncodeStream(Stream input, Stream output)
        {
            // Same as decoding
            DecodeStream(input, output);
        }
    }
}