using System;

namespace BinarySerializer.OpenSpace
{
    public abstract class WaveFileIdObj : IdObj
    {
        public uint Version { get; set; }
        public float Float_08 { get; set; } // Unknown - can't be greater than 0
        public WaveFileFlags FileFlags { get; set; }

        public uint ExternalFileNameLength { get; set; }
        public string ExternalFileName { get; set; }

        public WaveFile WaveFile { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            base.SerializeImpl(s);

            Version = s.Serialize<uint>(Version, name: nameof(Version));
            Float_08 = s.Serialize<float>(Float_08, name: nameof(Float_08));

            if (Version >= 3)
            {
                FileFlags = s.Serialize<WaveFileFlags>(FileFlags, name: nameof(FileFlags));
            }
            else
            {
                throw new NotImplementedException();
            }

            if ((FileFlags & WaveFileFlags.External) != 0)
            {
                ExternalFileNameLength = s.Serialize<uint>(ExternalFileNameLength, name: nameof(ExternalFileNameLength));
                ExternalFileName = s.SerializeString(ExternalFileName, length: ExternalFileNameLength, name: nameof(ExternalFileName));

                if (Version < 2)
                {
                    throw new NotImplementedException();
                }

                if (ExternalFileName.EndsWith("HXB", StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new NotImplementedException();
                }
                else
                {
                    WaveFile = s.SerializeObject<WaveFile>(WaveFile, name: nameof(WaveFile));
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        [Flags]
        public enum WaveFileFlags : byte
        {
            None = 0,
            External = 1 << 0,
            Flag1 = 1 << 1,
            Flag2 = 1 << 2,
            Flag3 = 1 << 3,
            Flag4 = 1 << 4,
            Flag5 = 1 << 5,
            Flag6 = 1 << 6,
            Flag7 = 1 << 7,
        }
    }
}