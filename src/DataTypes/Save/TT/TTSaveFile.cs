namespace BinarySerializer.OpenSpace
{
    public class TTSaveFile : BinarySerializable
    {
        public TTGameTableValue[] GameTable { get; set; }

        public override void SerializeImpl(SerializerObject s)
        {
            GameTable = s.SerializeObjectArrayUntil<TTGameTableValue>(GameTable, _ => s.CurrentFileOffset >= s.CurrentLength, name: nameof(GameTable));
        }
    }
}