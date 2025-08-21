namespace BinarySerializer.OpenSpace
{
    public enum TTDataType : byte
    {
        Type8 = 1,
        Type16 = 2,
        Type32 = 4,
        Type64 = 8,
        TypeXX = 16,
        TypePointer = 32,
    }
}