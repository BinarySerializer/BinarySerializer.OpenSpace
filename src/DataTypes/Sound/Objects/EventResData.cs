namespace BinarySerializer.OpenSpace
{
    public class EventResData : ResData
    {
        public const string ClassName = "CEventResData";

        // Name of the event
        public uint NameLength { get; set; }
        public string Name { get; set; }

        // Type and the resource to control
        public EventType Type { get; set; }
        public CUUID ResDataId { get; set; }

        // Unknown parameters. Probably things like fading?
        public float Float1 { get; set; }
        public float Float2 { get; set; }
        public float Float3 { get; set; }
        public float Float4 { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            base.SerializeImpl(s);

            if (Version >= 2)
            {
                NameLength = s.Serialize<uint>(NameLength, name: nameof(NameLength));
                Name = s.SerializeString(Name, length: NameLength, name: nameof(Name));
            }

            Type = s.Serialize<EventType>(Type, name: nameof(Type));
            ResDataId = s.SerializeObject<CUUID>(ResDataId, name: nameof(ResDataId));
            Float1 = s.Serialize<float>(Float1, name: nameof(Float1));
            Float2 = s.Serialize<float>(Float2, name: nameof(Float2));
            Float3 = s.Serialize<float>(Float3, name: nameof(Float3));
            Float4 = s.Serialize<float>(Float4, name: nameof(Float4));
        }

        public enum EventType : uint
        {
            Play = 0,
            Stop = 1,
            // TODO: Define the remaining types
        }
    }
}