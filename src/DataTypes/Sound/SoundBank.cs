namespace BinarySerializer.OpenSpace
{
    // https://github.com/vgmstream/vgmstream/blob/master/src/meta/ubi_hx.c
    public class SoundBank : BinarySerializable
    {
        public Pointer IndexPointer { get; set; }

        public uint Version { get; set; }
        public uint IdObjInfosCount { get; set; }
        public IdObjInfo[] IdObjInfos { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            IndexPointer = s.SerializePointer(IndexPointer, anchor: Offset, name: nameof(IndexPointer));

            s.DoAt(IndexPointer, () =>
            {
                s.SerializeMagicString("INDX", 4);
                Version = s.Serialize<uint>(Version, name: nameof(Version));
                IdObjInfosCount = s.Serialize<uint>(IdObjInfosCount, name: nameof(IdObjInfosCount));
                IdObjInfos = s.SerializeObjectArray<IdObjInfo>(IdObjInfos, IdObjInfosCount, x =>
                {
                    x.Pre_Version = Version;
                    x.Pre_BaseOffset = Offset;
                }, name: nameof(IdObjInfos));
            });
        }
    }
}