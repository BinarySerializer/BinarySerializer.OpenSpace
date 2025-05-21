﻿using System;

namespace BinarySerializer.OpenSpace
{
    public abstract class WavResData : ResData
    {
        public uint NameLength { get; set; }
        public string Name { get; set; }

        // Unknown values - volume?
        public float Float1 { get; set; }
        public float Float2 { get; set; }
        public float Float3 { get; set; }

        public WavResDataFlags Flags { get; set; }
        public CUUID FileId { get; set; } // Default language if localized, usually English
        public uint LocalizedFileIdsCount { get; set; }
        public LocalizedFileId[] LocalizedFileIds { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            base.SerializeImpl(s);

            if (Version >= 2)
            {
                NameLength = s.Serialize<uint>(NameLength, name: nameof(NameLength));
                Name = s.SerializeString(Name, length: NameLength, name: nameof(Name));
            }

            Float1 = s.Serialize<float>(Float1, name: nameof(Float1));
            Float2 = s.Serialize<float>(Float2, name: nameof(Float2));
            Float3 = s.Serialize<float>(Float3, name: nameof(Float3));

            if (Version >= 3)
            {
                Flags = s.Serialize<WavResDataFlags>(Flags, name: nameof(Flags));
            }
            else
            {
                throw new NotImplementedException();
            }

            FileId = s.SerializeObject<CUUID>(FileId, name: nameof(FileId));

            if (Version < 3)
            {
                throw new NotImplementedException();
            }

            if ((Flags & WavResDataFlags.Localized) != 0)
            {
                LocalizedFileIdsCount = s.Serialize<uint>(LocalizedFileIdsCount, name: nameof(LocalizedFileIdsCount));
                LocalizedFileIds = s.SerializeObjectArray<LocalizedFileId>(LocalizedFileIds, LocalizedFileIdsCount, name: nameof(LocalizedFileIds));
            }
        }

        [Flags]
        public enum WavResDataFlags : byte
        {
            None = 0,
            Flag0 = 1 << 0,
            Localized = 1 << 1,
            Flag2 = 1 << 2,
            Flag3 = 1 << 3,
            Flag4 = 1 << 4,
            Flag5 = 1 << 5,
            Flag6 = 1 << 6,
            Flag7 = 1 << 7,
        }

        public class LocalizedFileId : BinarySerializable
        {
            public string LanguageCode { get; set; }
            public CUUID FileId { get; set; }

            public override void SerializeImpl(SerializerObject s)
            {
                LanguageCode = s.SerializeString(LanguageCode, length: 4, name: nameof(LanguageCode));
                FileId = s.SerializeObject<CUUID>(FileId, name: nameof(FileId));
            }
        }
    }
}