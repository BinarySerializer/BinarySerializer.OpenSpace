namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// The data for a Rayman M/Arena/3 save file
    /// </summary>
    public class R3SaveFile : BinarySerializable
    {
        public R3SaveHeader SaveHeader { get; set; }
        public R3GameOptions GameOptions { get; set; }
        public R3SaveElement[] Elements { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            OpenSpaceSettings settings = s.GetRequiredSettings<OpenSpaceSettings>();

            CalculatedValueProcessor checksumProcessor = null;
            if (settings.EngineVersion == EngineVersion.Rayman3 && 
                settings.Platform == Platform.PlayStation2)
                checksumProcessor = new R3PS2MemoryCardChecksumProcessor();
            else if (settings.EngineVersion == EngineVersion.Rayman3 && 
                     settings.Platform == Platform.NintendoGameCube)
                checksumProcessor = new Checksum32Processor();

            s.DoProcessed(checksumProcessor, p =>
            {
                SaveHeader = s.SerializeObject<R3SaveHeader>(SaveHeader, name: nameof(SaveHeader));
                GameOptions = s.SerializeObject<R3GameOptions>(GameOptions, name: nameof(GameOptions));
                Elements = s.SerializeObjectArrayUntil(Elements, x => x.ElementNameLength == 0, () => new R3SaveElement(), name: nameof(Elements));

                if (settings.EngineVersion == EngineVersion.Rayman3 && 
                    settings.Platform == Platform.PlayStation2)
                {
                    p.Serialize<int>(s, name: "Checksum");
                }
                else if (settings.EngineVersion == EngineVersion.Rayman3 &&
                         settings.Platform == Platform.NintendoGameCube)
                {
                    s.Goto(Offset + 0x1000 - 4);
                    p.Serialize<int>(s, name: "Checksum");
                }
            });
        }
    }
}