namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// The data for a Rayman 3 save file
    /// </summary>
    public class R3SaveFile : BinarySerializable
    {
        public R3SaveList SaveList { get; set; }
        public R3GameOptions GameOptions { get; set; }
        public R3SaveItem[] Elements { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            OpenSpaceSettings settings = s.GetRequiredSettings<OpenSpaceSettings>();

            CalculatedValueProcessor processor = settings.Platform == Platform.PlayStation2 
                ? new R3PS2MemoryCardChecksumProcessor() // TODO: Fix
                : null;
            s.DoProcessed(processor, p =>
            {
                SaveList = s.SerializeObject<R3SaveList>(SaveList, name: nameof(SaveList));
                GameOptions = s.SerializeObject<R3GameOptions>(GameOptions, name: nameof(GameOptions));
                Elements = s.SerializeObjectArrayUntil(Elements, x => x.KeyLength == 0, () => new R3SaveItem(), name: nameof(Elements));

                if (settings.Platform == Platform.PlayStation2)
                    p.Serialize<int>(s, name: "Checksum");
            });
        }
    }
}