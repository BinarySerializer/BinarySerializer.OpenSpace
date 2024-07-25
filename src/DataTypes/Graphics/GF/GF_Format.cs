namespace BinarySerializer.OpenSpace
{
    public enum GF_Format
    {
        /// <summary>
        /// 32 bpp (8888), BGRA
        /// </summary>
        BGRA_8888,

        /// <summary>
        /// 24 bpp (888), BGR
        /// </summary>
        BGR_888,

        /// <summary>
        /// 16 bpp (88), grayscale with alpha channel
        /// </summary>
        GrayscaleAlpha_88,

        /// <summary>
        /// 16 bpp (4444), BGRA
        /// </summary>
        BGRA_4444,

        /// <summary>
        /// 16 bpp (1555), BGRA
        /// </summary>
        BGRA_1555,

        /// <summary>
        /// 16 bpp (565), BGR
        /// </summary>
        BGR_565,

        /// <summary>
        /// 8 bpp, BGRA
        /// </summary>
        BGRA_Indexed,

        /// <summary>
        /// 8 bpp, BGR
        /// </summary>
        BGR_Indexed,

        /// <summary>
        /// 8 bpp, grayscale
        /// </summary>
        Grayscale,
    }
}