using System;
using System.ComponentModel;

namespace Yeol
{
    public class ClearToSend : PayloadBase
    {
        private const int Length = 1;
        private const int MaxUser = 63;
        private const int UserBitMask = 0b00111111;
        private const int PlatformBitOffset = 6;

        public ClearToSend(PlatformType platformType, int user) : base(Length)
        {
            if (!Enum.IsDefined(typeof(PlatformType), platformType)) throw new InvalidEnumArgumentException(nameof(PlatformType));
            if (user > MaxUser) throw new ArgumentOutOfRangeException(nameof(User));

            PlatformType = platformType;
            User = user;
            Stream[0] = (byte)(((int)PlatformType << PlatformBitOffset) | User);
        }

        public ClearToSend(byte[] stream) : base(stream)
        {
            var value = stream[0];
            var platformType = (PlatformType)(value >> PlatformBitOffset);
            var user = value & UserBitMask;
            if (!Enum.IsDefined(typeof(PlatformType), platformType)) throw new InvalidEnumArgumentException(nameof(PlatformType));
            if (user > MaxUser) throw new ArgumentOutOfRangeException(nameof(User));

            PlatformType = platformType;
            User = user;
        }

        public PlatformType PlatformType { get; }
        public int User { get; }
    }
}