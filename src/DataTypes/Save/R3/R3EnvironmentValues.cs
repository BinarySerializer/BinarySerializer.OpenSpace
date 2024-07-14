namespace BinarySerializer.OpenSpace
{
    // g_aEnvValue
    public class R3EnvironmentValues : BinarySerializable
    {
        public R3EnvironmentValue Total { get; set; }
        public R3EnvironmentValue[] Levels { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Total = s.SerializeObject<R3EnvironmentValue>(Total, name: nameof(Total));
            Levels = s.SerializeObjectArray<R3EnvironmentValue>(Levels, 9, name: nameof(Levels));
        }
    }
}