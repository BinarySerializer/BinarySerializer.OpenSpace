using System;
using System.IO;

namespace BinarySerializer.OpenSpace
{
    /// <summary>
    /// A dummy stream without an actual data source which keeps track of the position for written bytes
    /// </summary>
    internal class DummyWriteStream : Stream
    {
        private long _length;
        private long _position;

        public override bool CanRead => false;
        public override bool CanSeek => true;
        public override bool CanWrite => true;
        public override long Length => _length;

        public override long Position
        {
            get => _position;
            set
            {
                _position = value;

                if (_position > Length)
                    _length = _position;
            }
        }

        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count) => throw new InvalidOperationException("Reading is not supported");

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;

                case SeekOrigin.Current:
                    Position += offset;
                    break;
                
                case SeekOrigin.End:
                    Position = Length + offset;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(origin), origin, null);
            }

            return Position;
        }

        public override void SetLength(long value) => _length = value;

        public override void Write(byte[] buffer, int offset, int count) => Position += count;
    }
}