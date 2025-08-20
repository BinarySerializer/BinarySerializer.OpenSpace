namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// The data for a Rayman 3 save file level
    /// </summary>
    public class R3EnvironmentValue : BinarySerializable
    {
        public uint Cages { get; set; }
        public uint Score { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Cages = s.Serialize<uint>(Cages, name: nameof(Cages));
            Score = s.Serialize<uint>(Score, name: nameof(Score));
        }
    }
}