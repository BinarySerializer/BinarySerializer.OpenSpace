using System;

namespace BinarySerializer.OpenSpace
{
    public class RefObjectCont : BinarySerializable
    {
        public uint Pre_Version { get; set; }

        public uint ReferencedIdsCount { get; set; }
        public CUUID[] ReferencedIds { get; set; }
        public uint LanguagesCount { get; set; }
        public RefObjectContLanguage[] Languages { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            ReferencedIdsCount = s.Serialize<uint>(ReferencedIdsCount, name: nameof(ReferencedIdsCount));
            ReferencedIds = s.SerializeObjectArray<CUUID>(ReferencedIds, ReferencedIdsCount, name: nameof(ReferencedIds));

            if (Pre_Version >= 3)
            {
                throw new NotImplementedException();
            }
            else
            {
                LanguagesCount = s.Serialize<uint>(LanguagesCount, name: nameof(LanguagesCount));
                Languages = s.SerializeObjectArray<RefObjectContLanguage>(Languages, LanguagesCount, name: nameof(Languages));
            }
        }
    }
}