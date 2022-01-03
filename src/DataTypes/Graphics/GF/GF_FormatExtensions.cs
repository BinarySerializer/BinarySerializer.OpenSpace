using System;

namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// Extension methods for <see cref="GF_Format"/>
    /// </summary>
    public static class GF_FormatExtensions
    {
        /// <summary>
        /// Indicates if the image format supports transparency (the alpha channel)
        /// </summary>
        /// <returns>True if transparency is supported, otherwise false</returns>
        public static bool SupportsTransparency(this GF_Format format)
        {
            return format switch
            {
                GF_Format.Format_32bpp_BGRA_8888 => true,
                GF_Format.Format_24bpp_BGR_888 => false,
                GF_Format.Format_16bpp_GrayAlpha_88 => true,
                GF_Format.Format_16bpp_BGRA_4444 => true,
                GF_Format.Format_16bpp_BGRA_1555 => true,
                GF_Format.Format_16bpp_BGR_565 => false,
                GF_Format.Format_8bpp_BGRA_Indexed => true,
                GF_Format.Format_8bpp_BGR_Indexed => false,
                GF_Format.Format_8bpp_Gray => false,
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
            };
        }
    }
}