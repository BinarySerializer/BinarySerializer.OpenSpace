using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// The encoder for Rayman 3 save data files
    /// </summary>
    public class R3SaveEncoder : IStreamEncoder
    {
        public string Name => "R3SaveEncoding";

        public void DecodeStream(Stream input, Stream output)
        {
            using var reader = new Reader(input, leaveOpen: true);

            // Read the initial key
            uint XORKey = reader.ReadUInt32() ^ 0xA55AA55A;

            // Helper method for reading the next byte
            byte ReadByte()
            {
                // Update the key
                XORKey = (XORKey >> 3) | (XORKey << 29);
                
                // Read the next byte
                byte b = reader.ReadByte();
                
                // XOR the byte
                b ^= (byte)XORKey;

                // Return the byte
                return b;
            }

            // Read the file data
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                // Read the next byte
                byte lastByte = ReadByte();

                // The last bit is a flag
                if ((lastByte & 0x80) == 0)
                {
                    // Write 0 the specified number of times. We don't need to remove the flag here since it's already 0.
                    for (int i = 0; i < lastByte; i++)
                        output.WriteByte(0);
                }
                else
                {
                    // Get the size to write by removing the first bit, which is used as a flag
                    int size = lastByte & 0x7F;

                    // Create the byte array
                    var byteArray = new byte[size];

                    // Read the specified number of bytes in reverse
                    for (int i = size; i > 0; --i)
                        byteArray[i - 1] = ReadByte();

                    // Write the bytes to the stream
                    output.Write(byteArray, 0, byteArray.Length);
                }
            }
        }

        public void EncodeStream(Stream input, Stream output)
        {
            using var writer = new Writer(output, leaveOpen: true);

            // Write the initial XOR key to be the same as the hard-coded key to make it 0, thus removing the encryption
            writer.Write(0xA55AA55A);

            // NOTE: We're not using the compression system here, TODO: Add a bool to the class to have the encoding to compress the data

            // Write in chunks of 127 (max size)
            while (input.Position < input.Length)
            {
                // Create a buffer for the chunk of data
                var buffer = new List<byte>();

                for (int i = 0; i < 127; i++)
                {
                    int value = input.ReadByte();

                    if (value == -1)
                        break;

                    buffer.Add((byte)value);
                }

                // Write the size and set the flag
                writer.Write((byte)((byte)buffer.Count | 0x80));

                // Write the buffer, but reversed
                writer.Write(((IEnumerable<byte>)buffer).Reverse().ToArray());
            }
        }
    }
}