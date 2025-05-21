namespace BinarySerializer.OpenSpace
{
    public abstract class IdObj : BinarySerializable
    {
        // The ID of this object
        public CUUID Id { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            Id = s.SerializeObject<CUUID>(Id, name: nameof(Id));
        }
    }
}