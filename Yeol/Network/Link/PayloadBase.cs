using System;

namespace Yeol
{
    public abstract class PayloadBase : IPayload
    {
        public const int MinLength = 1;
        public const int MaxLength = 127;

        protected PayloadBase(byte[] stream)
        {
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            if (Stream.Length > MaxLength || Stream.Length < MinLength) throw new ArgumentOutOfRangeException(nameof(Stream));
        }

        protected PayloadBase(int payloadLength) : this(new byte[payloadLength])
        {
        }

        public byte[] Stream { get; }
    }
}