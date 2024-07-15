using System;

namespace BinarySerializer.OpenSpace
{
    // C_PS2MC_Manager::CalculateCheckSum
    public class R3PS2MemoryCardChecksumProcessor : ChecksumProcessor
    {
        private int _checksumValue;
        private int _valueIndex;

        private int ReverseBytes(int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public override long CalculatedValue
        {
            get => ReverseBytes(_checksumValue);
            set => _checksumValue = ReverseBytes((int)value);
        }

        public override void ProcessBytes(byte[] buffer, int offset, int count)
        {
            int end = offset + count;
            for (int i = offset; i < end; i++)
            {
                int v0 = _valueIndex & 1;
                _valueIndex++;
                int v1 = buffer[i] + v0;
                _checksumValue = _checksumValue * 3 + (v0 + 1) * v1;
            }
        }
    }
}