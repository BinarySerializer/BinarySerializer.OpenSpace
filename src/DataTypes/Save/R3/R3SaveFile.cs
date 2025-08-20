namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// The data for a Rayman 3 save file
    /// </summary>
    public class R3SaveFile : BinarySerializable
    {
        public R3SaveHeader SaveHeader { get; set; }
        public R3GameOptions GameOptions { get; set; }
        public R3SaveElement[] Elements { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            OpenSpaceSettings settings = s.GetRequiredSettings<OpenSpaceSettings>();

            CalculatedValueProcessor processor = settings.Platform == Platform.PlayStation2 
                ? new R3PS2MemoryCardChecksumProcessor() // TODO: Fix
                : null;
            s.DoProcessed(processor, p =>
            {
                SaveHeader = s.SerializeObject<R3SaveHeader>(SaveHeader, name: nameof(SaveHeader));
                GameOptions = s.SerializeObject<R3GameOptions>(GameOptions, name: nameof(GameOptions));
                Elements = s.SerializeObjectArrayUntil(Elements, x => x.ElementNameLength == 0, () => new R3SaveElement(), name: nameof(Elements));

                if (settings.Platform == Platform.PlayStation2)
                    p.Serialize<int>(s, name: "Checksum");
            });
        }
    }
}