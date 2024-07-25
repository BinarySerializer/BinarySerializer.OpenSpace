using System;
using System.IO;

namespace BinarySerializer.OpenSpace
{
    public class GF_RLEEncoder : IStreamEncoder
    {
        public GF_RLEEncoder() { }

        public GF_RLEEncoder(byte rleCode, int bytesPerPixel, int imageSize)
        {
            RLECode = rleCode;
            BytesPerPixel = bytesPerPixel;
            ImageSize = imageSize;
        }

        public byte RLECode { get; }
        public int BytesPerPixel { get; }
        public int ImageSize { get; }

        public string Name => "GF_RLE";
        
        public void DecodeStream(Stream input, Stream output)
        {
            using var reader = new Reader(input, leaveOpen: true);

            // The encoded data is stored as an array of channels rather than pixels. Since it's simpler to work with the latter
            // we have the encoder also convert the data structure. Potentially this can be changed in the future if it causes issues.
            // If it were to be changed we would write directly to the output rather than using this buffer.
            byte[] pixelBuffer = new byte[BytesPerPixel * ImageSize];

            // Keep track of the current channel
            int channel = 0;

            // Enumerate each channel
            while (channel < BytesPerPixel)
            {
                int pixel = 0;

                // Enumerate through each pixel
                while (pixel < ImageSize)
                {
                    // Read the next byte
                    byte b = reader.ReadByte();

                    // Check if it's the repeat byte in which case we repeat a specific byte
                    if (b == RLECode)
                    {
                        // Get the value to repeat
                        byte value = reader.ReadByte();

                        // Get the number of times to repeat
                        byte count = reader.ReadByte();

                        // Repeat the value the specified number of times
                        for (int i = 0; i < count; ++i)
                        {
                            pixelBuffer[channel + pixel * BytesPerPixel] = value;
                            pixel++;
                        }
                    }
                    else
                    {
                        pixelBuffer[channel + pixel * BytesPerPixel] = b;
                        pixel++;
                    }
                }

                channel++;
            }

            output.Write(pixelBuffer, 0, pixelBuffer.Length);
        }

        public void EncodeStream(Stream input, Stream output)
        {
            // Read the bytes to be encoded
            byte[] buffer = new byte[input.Length];
            input.Read(buffer, 0, buffer.Length);

            // Keep track of the current channel
            int channel = 0;

            // Enumerate each channel
            while (channel < BytesPerPixel)
            {
                int pixelIndex = 0;
                byte getByte(int pixel) => buffer[pixel * BytesPerPixel + channel];

                // Enumerate through each pixel
                while (pixelIndex < ImageSize)
                {
                    // Get the byte
                    byte b = getByte(pixelIndex);

                    // Repeat if the byte is the repeat byte or else we can't write it normally
                    bool shouldRepeat = b == RLECode;

                    // Also repeat if the next 3 bytes matches this one
                    if (!shouldRepeat &&
                        pixelIndex + 3 < ImageSize &&
                        b == getByte(pixelIndex + 1) &&
                        b == getByte(pixelIndex + 2) &&
                        b == getByte(pixelIndex + 3))
                        shouldRepeat = true;

                    if (shouldRepeat)
                    {
                        // Get the value to repeat
                        byte repeatValue = b;

                        // Write the repeat byte
                        output.WriteByte(RLECode);

                        // Write the value to repeat
                        output.WriteByte(repeatValue);

                        // Keep track of how many times we repeat
                        int repeatCount = 0;

                        // Check each value until we break
                        while (pixelIndex < ImageSize)
                        {
                            // Get the byte
                            b = getByte(pixelIndex);

                            // Make sure it's still equal to the value and we haven't reached the maximum value
                            if (b != repeatValue || repeatCount >= Byte.MaxValue)
                                break;

                            // Increment the index and count
                            pixelIndex++;
                            repeatCount++;
                        }

                        // Write the repeat count
                        output.WriteByte((byte)repeatCount);
                    }
                    else
                    {
                        // Write the byte
                        output.WriteByte(b);

                        // Increment the index
                        pixelIndex++;
                    }
                }

                channel++;
            }
        }
    }
}