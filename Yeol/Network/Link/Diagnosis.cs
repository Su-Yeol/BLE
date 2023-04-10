using System;
using System.ComponentModel;

namespace Yeol
{
    public class Diagnosis : PayloadBase
    {
        private const int Length = 1;
        private const int MaxUser = 3;
        private const int ExceptionBitMask = 0b00111100;
        private const int UserBitMask = 0b00000011;
        private const int PlatformBitOffset = 6;
        private const int ExceptionBitOffset = 2;

        public Diagnosis(PlatformType platformType, ExceptionType exceptionType, int user) : base(Length)
        {
            if (!Enum.IsDefined(typeof(PlatformType), platformType)) throw new InvalidEnumArgumentException(nameof(PlatformType));
            if (!Enum.IsDefined(typeof(ExceptionType), exceptionType)) throw new InvalidEnumArgumentException(nameof(ExceptionType));
            if (user > MaxUser) throw new ArgumentOutOfRangeException(nameof(User));

            PlatformType = platformType;
            ExceptionType = exceptionType;
            User = user;
            Stream[0] = (byte)(((int)PlatformType << PlatformBitOffset) | ((int)ExceptionType << ExceptionBitOffset) | User);
        }

        public Diagnosis(byte[] stream) : base(stream)
        {
            var value = stream[0];
            var platformType = (PlatformType)(value >> PlatformBitOffset);
            var exceptionType = (ExceptionType)((value & ExceptionBitMask) >> ExceptionBitOffset);
            var user = value & UserBitMask;
            if (!Enum.IsDefined(typeof(PlatformType), platformType)) throw new InvalidEnumArgumentException(nameof(PlatformType));
            if (!Enum.IsDefined(typeof(ExceptionType), exceptionType)) throw new InvalidEnumArgumentException(nameof(ExceptionType));
            if (user > MaxUser) throw new ArgumentOutOfRangeException(nameof(User));

            PlatformType = platformType;
            ExceptionType = exceptionType;
            User = user;
        }

        public PlatformType PlatformType { get; }
        public ExceptionType ExceptionType { get; }
        public int User { get; }
    }
}