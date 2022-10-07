using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// The data used for a .gf graphics file
    /// </summary>
    public class GF : BinarySerializable
    {
        #region Public Properties

        public uint Format { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte Channels { get; set; }
        public byte MipmapsCount { get; set; } // Includes the primary texture if not 0
        public byte RepeatByte { get; set; }
        public int PixelCount { get; set; }
        public byte[] PixelData { get; set; }

        // Montreal
        public byte Montreal_Version { get; set; }
        public byte Montreal_PaletteBytesPerColor { get; set; }
        public byte[] Montreal_Palette { get; set; }
        public ushort Montreal_PaletteLength { get; set; }
        public byte Montreal_Byte_0F { get; set; }
        public byte Montreal_Byte_10 { get; set; }
        public byte Montreal_Byte_11 { get; set; }
        public uint Montreal_Uint_12 { get; set; }

        // Helpers

        /// <summary>
        /// The amount of additional mipmaps, not counting the primary texture
        /// </summary>
        public int ExclusiveMipmapCount => MipmapsCount == 0 ? 0 : MipmapsCount - 1;

        public GF_Format PixelFormat
        {
            get
            {
                if (Channels == 4)
                {
                    return GF_Format.Format_32bpp_BGRA_8888;
                }
                else if (Channels >= 3)
                {
                    return GF_Format.Format_24bpp_BGR_888;
                }
                else if (Channels == 2)
                {
                    return Format switch
                    {
                        88 => GF_Format.Format_16bpp_GrayAlpha_88,
                        4444 => GF_Format.Format_16bpp_BGRA_4444,
                        1555 => GF_Format.Format_16bpp_BGRA_1555,
                        565 => GF_Format.Format_16bpp_BGR_565,
                        _ => GF_Format.Format_16bpp_BGR_565
                    };
                }
                else if (Channels == 1)
                {
                    if (Montreal_PaletteLength != 0 && Montreal_PaletteBytesPerColor != 0)
                    {
                        return Montreal_PaletteBytesPerColor switch
                        {
                            3 => GF_Format.Format_8bpp_BGR_Indexed,
                            4 => GF_Format.Format_8bpp_BGRA_Indexed,
                            _ => throw new Exception("The number of palette bytes per color is not valid")
                        };
                    }
                    else
                    {
                        return GF_Format.Format_8bpp_Gray;
                    }
                }
                else
                {
                    throw new Exception("The number of channels is not valid");
                }
            }
            set
            {
                switch (value)
                {
                    case GF_Format.Format_32bpp_BGRA_8888:
                        Format = 8888;
                        Channels = 4;
                        break;

                    case GF_Format.Format_24bpp_BGR_888:
                        Format = 888;
                        Channels = 3;
                        break;

                    case GF_Format.Format_16bpp_GrayAlpha_88:
                        Format = 88;
                        Channels = 2;
                        break;

                    case GF_Format.Format_16bpp_BGRA_4444:
                        Format = 4444;
                        Channels = 2;
                        break;

                    case GF_Format.Format_16bpp_BGRA_1555:
                        Format = 1555;
                        Channels = 2;
                        break;

                    case GF_Format.Format_16bpp_BGR_565:
                        Format = 565;
                        Channels = 2;
                        break;

                    case GF_Format.Format_8bpp_BGRA_Indexed:
                        Format = 0;
                        Channels = 1;
                        Montreal_PaletteBytesPerColor = 4;
                        break;

                    case GF_Format.Format_8bpp_BGR_Indexed:
                        Format = 0;
                        Channels = 1;
                        Montreal_PaletteBytesPerColor = 3;
                        break;

                    case GF_Format.Format_8bpp_Gray:
                        Format = 8;
                        Channels = 1;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the sizes for all mipmap images, starting from the largest and not including the primary texture
        /// </summary>
        /// <returns>The sizes of all images</returns>
        public IEnumerable<(int Width, int Height)> GetExclusiveMipmapSizes()
        {
            // Get the largest size
            int width = Width;
            int height = Height;

            // Enumerate each mipmap
            for (int i = 0; i < ExclusiveMipmapCount; i++)
            {
                // Get the next mipmap size
                if (width != 1)
                    width >>= 1;

                if (height != 1)
                    height >>= 1;

                // Return the current size
                yield return (width, height);
            }
        }

        /// <summary>
        /// Determines the preferred mipmaps count based on the current size
        /// </summary>
        /// <returns>The mipmaps count</returns>
        public byte DeterminePreferredMipmapsCount()
        {
            // Get the largest size
            int width = Width;
            int height = Height;

            // Keep track of the count
            byte count = 1;

            // Enumerate each mipmap
            while (!(width == 1 && height == 1))
            {
                // Get the next mipmap size
                if (width != 1)
                    width >>= 1;

                if (height != 1)
                    height >>= 1;

                count++;
            }

            return count;
        }

        /// <summary>
        /// Gets the pixel color from the pixel data in the BGR(A) format
        /// </summary>
        /// <param name="format">The GF pixel format</param>
        /// <param name="gfPixelData">The pixel data to get the color from</param>
        /// <param name="offset">The offset for the specific pixel in the data array</param>
        /// <returns>The color for the pixel in the BGR(A) format</returns>
        public IEnumerable<byte> GetBGRAPixel(GF_Format format, byte[] gfPixelData, long offset)
        {
            switch (format)
            {
                case GF_Format.Format_32bpp_BGRA_8888:
                    // Get the BGR color values
                    yield return gfPixelData[offset + 0];
                    yield return gfPixelData[offset + 1];
                    yield return gfPixelData[offset + 2];
                    yield return gfPixelData[offset + 3];

                    break;

                case GF_Format.Format_24bpp_BGR_888:
                    // Get the BGRA color values
                    yield return gfPixelData[offset + 0];
                    yield return gfPixelData[offset + 1];
                    yield return gfPixelData[offset + 2];

                    break;

                case GF_Format.Format_16bpp_GrayAlpha_88:
                case GF_Format.Format_16bpp_BGRA_4444:
                case GF_Format.Format_16bpp_BGRA_1555:
                case GF_Format.Format_16bpp_BGR_565:

                    ushort pixel = BitConverter.ToUInt16(new byte[]
                    {
                        gfPixelData[offset],
                        gfPixelData[offset + 1]
                    }, 0); // RRRRR, GGGGGG, BBBBB (565)

                    switch (format)
                    {
                        case GF_Format.Format_16bpp_GrayAlpha_88:
                            yield return gfPixelData[offset];
                            yield return gfPixelData[offset];
                            yield return gfPixelData[offset];
                            yield return gfPixelData[offset + 1];

                            break;

                        case GF_Format.Format_16bpp_BGRA_4444:

                            yield return (byte)(BitHelpers.ExtractBits(pixel, 4, 0) * 17);
                            yield return (byte)(BitHelpers.ExtractBits(pixel, 4, 4) * 17);
                            yield return (byte)(BitHelpers.ExtractBits(pixel, 4, 8) * 17);
                            yield return (byte)(BitHelpers.ExtractBits(pixel, 4, 12) * 17);

                            break;

                        case GF_Format.Format_16bpp_BGRA_1555:
                            const float multiple = (255 / 31f);

                            yield return (byte)(BitHelpers.ExtractBits(pixel, 5, 0) * multiple);
                            yield return (byte)(BitHelpers.ExtractBits(pixel, 5, 5) * multiple);
                            yield return (byte)(BitHelpers.ExtractBits(pixel, 5, 10) * multiple);
                            yield return (byte)(BitHelpers.ExtractBits(pixel, 1, 15) * 255);

                            break;

                        case GF_Format.Format_16bpp_BGR_565:
                            yield return (byte)(BitHelpers.ExtractBits(pixel, 5, 0) * (255 / 31f));
                            yield return (byte)(BitHelpers.ExtractBits(pixel, 6, 5) * (255 / 63f));
                            yield return (byte)(BitHelpers.ExtractBits(pixel, 5, 11) * (255 / 31f));

                            break;
                    }

                    break;

                case GF_Format.Format_8bpp_BGRA_Indexed:
                case GF_Format.Format_8bpp_BGR_Indexed:
                    for (int i = 0; i < Montreal_PaletteBytesPerColor; i++)
                        yield return Montreal_Palette[gfPixelData[offset] * Montreal_PaletteBytesPerColor + i];

                    break;

                case GF_Format.Format_8bpp_Gray:
                    yield return gfPixelData[offset];
                    yield return gfPixelData[offset];
                    yield return gfPixelData[offset];

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        /// <summary>
        /// Gets the pixel bytes from the pixel colors in the GF format
        /// </summary>
        /// <param name="format">The .gf pixel format</param>
        /// <param name="bgraPixelData">The bitmap pixel data to get the color from, always 4 bytes</param>
        /// <returns>The color for the pixel in the .gf format</returns>
        public IEnumerable<byte> GetGfPixelBytes(GF_Format format, byte[] bgraPixelData)
        {
            switch (format)
            {
                case GF_Format.Format_32bpp_BGRA_8888:
                    // Get the BGRA color values
                    yield return bgraPixelData[0];
                    yield return bgraPixelData[1];
                    yield return bgraPixelData[2];
                    yield return bgraPixelData[3];

                    break;

                case GF_Format.Format_24bpp_BGR_888:
                    // Get the BGR color values
                    yield return bgraPixelData[0];
                    yield return bgraPixelData[1];
                    yield return bgraPixelData[2];

                    break;

                case GF_Format.Format_16bpp_GrayAlpha_88:
                case GF_Format.Format_16bpp_BGRA_4444:
                case GF_Format.Format_16bpp_BGRA_1555:
                case GF_Format.Format_16bpp_BGR_565:
                    throw new NotImplementedException("Importing to files with 2 channels is currently not supported");

                case GF_Format.Format_8bpp_BGRA_Indexed:
                case GF_Format.Format_8bpp_BGR_Indexed:
                    throw new NotImplementedException("Importing to files with a palette is currently not supported");

                case GF_Format.Format_8bpp_Gray:
                    yield return bgraPixelData[0];

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        /// <summary>
        /// Indicates if mipmaps are supported. NOTE: This currently does not count the mipmapping used in Hype.
        /// </summary>
        public bool SupportsMipmaps(OpenSpaceSettings settings) => settings.MajorEngineVersion == MajorEngineVersion.Rayman3 &&
                                                                   settings.EngineVersion != EngineVersion.Dinosaur && 
                                                                   settings.EngineVersion != EngineVersion.LargoWinch;

        /// <summary>
        /// Updates the repeat byte to the most appropriate value
        /// </summary>
        public void UpdateRepeatByte()
        {
            // Keep track of the occurrence for each value
            int[] byteCounts = new int[Byte.MaxValue + 1];

            // Enumerate each byte
            foreach (byte b in PixelData)
                byteCounts[b]++;

            // Get the min value
            int min = byteCounts.Min();

            // Set the repeat byte to the index with the minimum value
            RepeatByte = (byte)Array.IndexOf(byteCounts, min);
        }

        public override void SerializeImpl(SerializerObject s)
        {
            // Get the settings
            OpenSpaceSettings settings = s.GetRequiredSettings<OpenSpaceSettings>();

            if (settings.MajorEngineVersion == MajorEngineVersion.Montreal)
            {
                // Serialize the version
                Montreal_Version = s.Serialize<byte>(Montreal_Version, name: nameof(Montreal_Version));
            }
            else if (settings.Platform != Platform.iOS && settings.EngineVersion != EngineVersion.TonicTroubleSpecialEdition)
            {
                // Serialize the format
                Format = s.Serialize<uint>(Format, name: nameof(Format));
            }

            // Serialize the dimensions and channels count
            Width = s.Serialize<int>(Width, name: nameof(Width));
            Height = s.Serialize<int>(Height, name: nameof(Height));
            Channels = s.Serialize<byte>(Channels, name: nameof(Channels));

            if (settings.Platform == Platform.iOS || settings.EngineVersion == EngineVersion.TonicTroubleSpecialEdition)
                Format = Channels == 4 ? 8888u : 888u;

            // Check if mipmaps are used
            if (SupportsMipmaps(settings))
                MipmapsCount = s.Serialize<byte>(MipmapsCount, name: nameof(MipmapsCount));
            else
                MipmapsCount = 0;

            // Set the pixel count
            PixelCount = Width * Height;

            // Enumerate each mipmap size
            foreach (var (width, height) in GetExclusiveMipmapSizes())
            {
                // Get the mipmap pixel count
                int count = width * height;

                // Add to the total pixel count
                PixelCount += count;
            }

            // Serialize the repeat byte
            RepeatByte = s.Serialize<byte>(RepeatByte, name: nameof(RepeatByte));

            // Serialize Montreal specific values
            if (settings.MajorEngineVersion == MajorEngineVersion.Montreal)
            {
                Montreal_PaletteLength = s.Serialize<ushort>(Montreal_PaletteLength, name: nameof(Montreal_PaletteLength));
                Montreal_PaletteBytesPerColor = s.Serialize<byte>(Montreal_PaletteBytesPerColor, name: nameof(Montreal_PaletteBytesPerColor));

                Montreal_Byte_0F = s.Serialize<byte>(Montreal_Byte_0F, name: nameof(Montreal_Byte_0F));
                Montreal_Byte_10 = s.Serialize<byte>(Montreal_Byte_10, name: nameof(Montreal_Byte_10));
                Montreal_Byte_11 = s.Serialize<byte>(Montreal_Byte_11, name: nameof(Montreal_Byte_11));
                Montreal_Uint_12 = s.Serialize<uint>(Montreal_Uint_12, name: nameof(Montreal_Uint_12));

                PixelCount = s.Serialize<int>(PixelCount, name: nameof(PixelCount)); // Hype has mipmaps

                // Get the current Montreal type based on the format
                byte montrealType = Format switch
                {
                    0 => 5,
                    565 => 10,
                    1555 => 11,
                    4444 => 12,
                    _ => throw new BinarySerializableException(this, $"Unknown Montreal GF format {Format}")
                };

                // Serialize the Montreal type
                montrealType = s.Serialize<byte>(montrealType, name: nameof(montrealType));

                // Set the format based on the Montreal type
                Format = montrealType switch
                {
                    5 => 0u,
                    10 => 565u,
                    11 => 1555u,
                    12 => 4444u,
                    _ => throw new BinarySerializableException(this, $"Unknown Montreal GF format {montrealType}")
                };

                Montreal_Palette = s.SerializeArray<byte>(Montreal_Palette, Montreal_PaletteBytesPerColor * Montreal_PaletteLength, name: nameof(Montreal_Palette));
            }

            // Serialize the pixel data
            s.DoEncoded(new GF_Encoder(RepeatByte, Channels, PixelCount), () =>
                PixelData = s.SerializeArray<byte>(PixelData, Channels * PixelCount, name: nameof(PixelData)));
        }

        #endregion
    }
}