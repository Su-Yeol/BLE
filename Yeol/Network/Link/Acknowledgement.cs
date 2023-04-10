using System;
using System.ComponentModel;

namespace Yeol
{
    public class Acknowledgement : PayloadBase
    {
        private const int Length = 1;
        private const int MaxId = 31;
        private const int IdBitMask = 0b00011111;
        private const int TypeBitOffset = 5;

        public Acknowledgement(FrameType frameType, int id) : base(Length)
        {
            if (!Enum.IsDefined(typeof(FrameType), frameType)) throw new InvalidEnumArgumentException(nameof(FrameType));
            if (id > MaxId) throw new ArgumentOutOfRangeException(nameof(Id));

            FrameType = frameType;
            Id = id;
            Stream[0] = (byte)(((int)FrameType << TypeBitOffset) | Id);
        }

        public Acknowledgement(byte[] stream) : base(stream)
        {
            var value = stream[0];
            var frameType = (FrameType)(value >> TypeBitOffset);
            var id = value & IdBitMask;
            if (!Enum.IsDefined(typeof(FrameType), frameType)) throw new InvalidEnumArgumentException(nameof(FrameType));
            if (id > MaxId) throw new ArgumentOutOfRangeException(nameof(Id));

            FrameType = frameType;
            Id = id;
        }

        public FrameType FrameType { get; }
        public int Id { get; }
    }
}