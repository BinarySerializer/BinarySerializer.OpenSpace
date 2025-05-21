namespace BinarySerializer.OpenSpace
{
    public abstract class ResData : IdObj
    {
        public uint Version { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            base.SerializeImpl(s);

            Version = s.Serialize<uint>(Version, name: nameof(Version));
        }
    }
}