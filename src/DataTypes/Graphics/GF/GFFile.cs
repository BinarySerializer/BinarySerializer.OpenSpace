namespace BinarySerializer.OpenSpace
{
    public class GFFile : BinarySerializable
    {
        public GFFileHeader Header { get; set; }
        public byte[] Palette { get; set; }
        public byte[] ImgData { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Header = s.SerializeObject<GFFileHeader>(Header, name: nameof(Header));

            if (Header.HasPalette)
                Palette = s.SerializeArray<byte>(Palette, Header.PaletteDataAlignment * Header.PaletteLength, name: nameof(Palette));

            s.DoEncoded(new GF_RLEEncoder(Header.RLECode, Header.BytesPerPixel, Header.ImageSize), () =>
                ImgData = s.SerializeArray<byte>(ImgData, Header.BytesPerPixel * Header.ImageSize, name: nameof(ImgData)));
        }
    }
}