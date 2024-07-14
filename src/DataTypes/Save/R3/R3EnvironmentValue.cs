namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// The data for a Rayman 3 save file level
    /// </summary>
    public class R3EnvironmentValue : BinarySerializable
    {
        public int Cages { get; set; }
        public int Score { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Cages = s.Serialize<int>(Cages, name: nameof(Cages));
            Score = s.Serialize<int>(Score, name: nameof(Score));
        }
    }
}