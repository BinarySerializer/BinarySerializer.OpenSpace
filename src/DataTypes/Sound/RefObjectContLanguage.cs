namespace BinarySerializer.OpenSpace
{
    public class RefObjectContLanguage : BinarySerializable
    {
        public string LanguageCode { get; set; }
        public uint ReferencedIdsCount { get; set; }
        public CUUID[] ReferencedIds { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            LanguageCode = s.SerializeString(LanguageCode, length: 4, name: nameof(LanguageCode));
            ReferencedIdsCount = s.Serialize<uint>(ReferencedIdsCount, name: nameof(ReferencedIdsCount));
            ReferencedIds = s.SerializeObjectArray<CUUID>(ReferencedIds, ReferencedIdsCount, name: nameof(ReferencedIds));
        }
    }
}