using System;
using System.ComponentModel;

namespace Yeol
{
    public class ReadyToSend : PayloadBase
    {
        private const int Length = 1;
        private const int MaxUser = 15;
        private const int HeartbeatBitMask = 0b00100000;
        private const int AckBitMask = 0b00001000;
        private const int UserBitMask = 0b00001111;
        private const int PlatformBitOffset = 6;
        private const int HeartbeatBitOffset = 3;
        private const int AckBitOffset = 2;

        public ReadyToSend(PlatformType platformType, bool canHeartbeat, bool canAck, int user) : base(Length)
        {
            if (!Enum.IsDefined(typeof(PlatformType), platformType)) throw new InvalidEnumArgumentException(nameof(PlatformType));
            if (user > MaxUser) throw new ArgumentOutOfRangeException(nameof(User));

            PlatformType = platformType;
            CanHeartbeat = canHeartbeat;
            CanAck = canAck;
            User = user;
            Stream[0] = (byte)(((int)PlatformType << PlatformBitOffset) | ((CanHeartbeat ? 1 : 0) << HeartbeatBitOffset) | ((CanAck ? 1 : 0) << AckBitOffset) | User);
        }

        public ReadyToSend(byte[] stream) : base(stream)
        {
            var value = stream[0];
            var platformType = (PlatformType)(value >> PlatformBitOffset);
            var user = value & UserBitMask;
            if (!Enum.IsDefined(typeof(PlatformType), platformType)) throw new InvalidEnumArgumentException(nameof(PlatformType));
            if (user > MaxUser) throw new ArgumentOutOfRangeException(nameof(User));

            PlatformType = platformType;
            CanHeartbeat = (value & HeartbeatBitMask) == HeartbeatBitMask;
            CanAck = (value & AckBitMask) == AckBitMask;
            User = user;
        }

        public PlatformType PlatformType { get; }
        public bool CanHeartbeat { get; }
        public bool CanAck { get; }
        public int User { get; }
    }
}