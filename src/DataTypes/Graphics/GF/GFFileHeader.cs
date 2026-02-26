using System;
using System.Collections.Generic;
using System.Linq;

namespace BinarySerializer.OpenSpace
{
    // TODO: Add support for the mipmaps used in Montreal games such as Hype
    public class GFFileHeader : BinarySerializable
    {
        #region Public Properties

        public byte Version { get; set; } // Always 1?
        public uint Format { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public int ImageSize { get; set; }
        public byte BytesPerPixel { get; set; } // NumberOfBits
        public byte MipmapLevels { get; set; } // NumberOfLod

        public byte RLECode { get; set; } // Byte to use to indicate repeating bytes

        public byte PaletteDataAlignment { get; set; }
        public byte PaletteBytesPerColor { get; set; }
        public ushort PaletteLength { get; set; }
        public bool HasPalette => PaletteDataAlignment != 0 && PaletteBytesPerColor != 0 && PaletteLength != 0;

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
        public int ChromakeyIndex { get; set; } // -1 if no palette

        // Helpers

        /// <summary>
        /// The amount of additional mipmaps, not counting the primary texture
        /// </summary>
        public int ExclusiveMipmapsCount => MipmapLevels == 0 ? 0 : MipmapLevels - 1;

        public GF_Format PixelFormat
        {
            get
            {
                if (BytesPerPixel == 4)
                {
                    return GF_Format.BGRA_8888;
                }
                else if (BytesPerPixel >= 3)
                {
                    return GF_Format.BGR_888;
                }
                else if (BytesPerPixel == 2)
                {
                    return Format switch
                    {
                        88 => GF_Format.GrayscaleAlpha_88,
                        4444 => GF_Format.BGRA_4444,
                        1555 => GF_Format.BGRA_1555,
                        565 => GF_Format.BGR_565,
                        _ => GF_Format.BGR_565
                    };
                }
                else if (BytesPerPixel == 1)
                {
                    if (HasPalette)
                    {
                        return PaletteBytesPerColor switch
                        {
                            3 => GF_Format.BGR_Indexed,
                            4 => GF_Format.BGRA_Indexed,
                            _ => throw new Exception("The number of palette bytes per color is not valid")
                        };
                    }
                    else
                    {
                        return GF_Format.Grayscale;
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
                    case GF_Format.BGRA_8888:
                        Format = 8888;
                        BytesPerPixel = 4;
                        break;

                    case GF_Format.BGR_888:
                        Format = 888;
                        BytesPerPixel = 3;
                        break;

                    case GF_Format.GrayscaleAlpha_88:
                        Format = 88;
                        BytesPerPixel = 2;
                        break;

                    case GF_Format.BGRA_4444:
                        Format = 4444;
                        BytesPerPixel = 2;
                        break;

                    case GF_Format.BGRA_1555:
                        Format = 1555;
                        BytesPerPixel = 2;
                        break;

                    case GF_Format.BGR_565:
                        Format = 565;
                        BytesPerPixel = 2;
                        break;

                    case GF_Format.BGRA_Indexed:
                        Format = 0;
                        BytesPerPixel = 1;
                        PaletteBytesPerColor = 4;
                        break;

                    case GF_Format.BGR_Indexed:
                        Format = 0;
                        BytesPerPixel = 1;
                        PaletteBytesPerColor = 3;
                        break;

                    case GF_Format.Grayscale:
                        Format = 8;
                        BytesPerPixel = 1;
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
            for (int i = 0; i < ExclusiveMipmapsCount; i++)
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
        /// Indicates if mipmaps are supported. NOTE: This currently does not count the mipmapping used in Hype.
        /// </summary>
        public bool SupportsMipmaps(OpenSpaceSettings settings) => settings.MajorEngineVersion == MajorEngineVersion.Rayman3 &&
                                                                   settings.EngineVersion != EngineVersion.Dinosaur &&
                                                                   settings.EngineVersion != EngineVersion.LargoWinch;

        public void RecalculateMipmapLevels()
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

            MipmapLevels = count;
        }

        public void RecalculateImageSize()
        {
            // Set the pixels count
            ImageSize = Width * Height;

            // Enumerate each mipmap size
            foreach (var (width, height) in GetExclusiveMipmapSizes())
            {
                // Get the mipmap pixel count
                int count = width * height;

                // Add to the total pixel count
                ImageSize += count;
            }
        }

        public void RecalculateRLECode(byte[] imgData)
        {
            // Keep track of the occurrence for each value
            int[] byteCounts = new int[Byte.MaxValue + 1];

            // Enumerate each byte
            foreach (byte b in imgData)
                byteCounts[b]++;

            // Get the min value
            int min = byteCounts.Min();

            // Set the repeat byte to the index with the minimum value
            RLECode = (byte)Array.IndexOf(byteCounts, min);
        }

        public override void SerializeImpl(SerializerObject s)
        {
            // Get the settings
            OpenSpaceSettings settings = s.GetRequiredSettings<OpenSpaceSettings>();

            if (settings.MajorEngineVersion == MajorEngineVersion.Montreal)
            {
                // Serialize the version
                Version = s.Serialize<byte>(Version, name: nameof(Version));
            }
            else if (settings.Platform != Platform.iOS && settings.EngineVersion != EngineVersion.TonicTroubleSpecialEdition)
            {
                // Serialize the format
                Format = s.Serialize<uint>(Format, name: nameof(Format));
            }

            // Serialize the dimensions and channels count
            Width = s.Serialize<int>(Width, name: nameof(Width));
            Height = s.Serialize<int>(Height, name: nameof(Height));
            BytesPerPixel = s.Serialize<byte>(BytesPerPixel, name: nameof(BytesPerPixel));

            if (settings.Platform == Platform.iOS || 
                settings.EngineVersion == EngineVersion.TonicTroubleSpecialEdition)
            {
                if (BytesPerPixel == 4)
                {
                    Format = 8888u;
                }
                else if (BytesPerPixel == 3)
                {
                    Format = 888u;
                }
                else if (BytesPerPixel == 1)
                {
                    Format = 0;
                    PaletteDataAlignment = 4;
                    PaletteBytesPerColor = 3;
                    PaletteLength = 256;
                }
                else
                {
                    // Default to 888 for now
                    Format = 888u;
                }
            }

            // Serialize mipmaps
            if (SupportsMipmaps(settings))
                MipmapLevels = s.Serialize<byte>(MipmapLevels, name: nameof(MipmapLevels));
            else
                MipmapLevels = 0;

            // Serialize the RLE code
            RLECode = s.Serialize<byte>(RLECode, name: nameof(RLECode));

            // Serialize Montreal specific values
            if (settings.MajorEngineVersion == MajorEngineVersion.Montreal)
            {
                PaletteLength = s.Serialize<ushort>(PaletteLength, name: nameof(PaletteLength));
                PaletteBytesPerColor = s.Serialize<byte>(PaletteBytesPerColor, name: nameof(PaletteBytesPerColor));
                PaletteDataAlignment = PaletteBytesPerColor;

                Red = s.Serialize<byte>(Red, name: nameof(Red));
                Green = s.Serialize<byte>(Green, name: nameof(Green));
                Blue = s.Serialize<byte>(Blue, name: nameof(Blue));
                ChromakeyIndex = s.Serialize<int>(ChromakeyIndex, name: nameof(ChromakeyIndex));

                ImageSize = s.Serialize<int>(ImageSize, name: nameof(ImageSize)); // Hype has mipmaps

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
            }
            else
            {
                RecalculateImageSize();
            }
        }

        #endregion
    }
}