using System;
using System.ComponentModel;
using System.Linq;

namespace Yeol
{
    /// <summary>
    ///     UART Data Frame 처리 class
    /// </summary>
    public class SerialFrame
    {
        // 최상위 비트마스크와 최하위 비트 옵셋은 생략가능
        //private const int TypeBitMask = 0b11100000;
        private const int IdBitMask = 0b00011111;

        private const int TypeBitOffset = 5;
        //private const int IdBitOffset = 0;

        /// <summary>
        ///     Maximum of Checksum is 128bytes * 0xFF = 32,640(0x7F80).
        /// </summary>
        private const int MaxChecksum = 0x7F80;

        /// <summary>
        ///     Maximum of five bits is 31(0x1F).
        /// </summary>
        private const int MaxId = 0b00011111;

        /// <summary>
        ///     Delimiter: 0x8080 - 고유값
        /// </summary>
        public const int Delimiter = 0x8080;

        /// <summary>
        ///     Delimiter Size 2
        /// </summary>
        public const int DelimiterSize = 2;

        /// <summary>
        ///     Delimiter(2) + Flag(1) + PayloadLength(1) + [0 bytes: Payload(n)] + Checksum(2)
        ///     HeaderSize + ChecksumSize
        /// </summary>
        public const int MinSize = 6;

        /// <summary>
        ///     Delimiter(2) + Flag(1) + PayloadLength(1)
        /// </summary>
        public const int HeaderSize = 4;

        /// <summary>
        ///     Delimiter(2) + Flag(1) + PayloadLength(1)
        /// </summary>
        public const int FlagOffset = 2;

        /// <summary>
        ///     Delimiter(2) + Flag(1) + PayloadLength(1)
        /// </summary>
        public const int PayloadLengthOffset = 3;

        /// <summary>
        ///     Checksum(2): Sum of 128bytes 0xFF
        /// </summary>
        public const int ChecksumSize = 2;

        public int Flag { get; }
        public FrameType FrameType { get; }
        public int Id { get; }
        public IPayload Payload { get; }
        public int Checksum { get; }
        public byte[] Stream { get; }

        public SerialFrame(FrameType frameType, int id, IPayload payload) // buffer
        {
            if (!Enum.IsDefined(typeof(FrameType), frameType)) throw new InvalidEnumArgumentException(nameof(FrameType));
            if (id > MaxId) throw new ArgumentOutOfRangeException(nameof(Id));
            Payload = payload ?? throw new ArgumentNullException(nameof(payload));
            if (Payload.Stream.Length > PayloadBase.MaxLength) throw new ArgumentOutOfRangeException(nameof(Payload));
            var flag = ((int)frameType << TypeBitOffset) | id;
            var checksum = CalculateChecksum(flag, Payload);
            if (checksum > MaxChecksum) throw new ArgumentOutOfRangeException(nameof(Checksum));

            Flag = flag;
            FrameType = frameType;
            Id = id;
            Checksum = checksum;
            Stream = CreateStream();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param> ByteBuffer.Read(SerialFrame.HeaderSize, payloadLength, true);
        public SerialFrame(int flag, byte[] payload, int checksum)
        {
            // framType, id 단순 확인용 - 굳이 사용안해도 된다
            var frameType = (FrameType)(flag >> TypeBitOffset);
            var id = flag & IdBitMask;
            if (!Enum.IsDefined(typeof(FrameType), frameType)) throw new InvalidEnumArgumentException(nameof(FrameType));
            if (id > MaxId) throw new ArgumentOutOfRangeException(nameof(Id));

            Payload = CreatePayload(frameType, payload); // Payload.Stream = payload 데이터만 존재
            // ToDo 체크섬 확인하기
            //if (checksum != CalculateChecksum(flag, Payload) || checksum > MaxChecksum) throw new ArgumentOutOfRangeException(nameof(Checksum));
            Flag = flag;
            FrameType = frameType;
            Id = id;
            Checksum = checksum;
            Stream = CreateStream(); // SerailFram.Stream = Header + Payload + CheckSum
        }

        public SerialFrame(byte[] frame)
        {
            Stream = frame ?? throw new ArgumentNullException(nameof(frame));
            var flag = Stream[FlagOffset];
            var frameType = (FrameType)(Flag >> TypeBitOffset);
            var id = (byte)(Flag & IdBitMask);
            var payloadLength = Stream[PayloadLengthOffset];
            if (!Enum.IsDefined(typeof(FrameType), frameType)) throw new InvalidEnumArgumentException(nameof(FrameType));
            if (id > MaxId) throw new ArgumentOutOfRangeException(nameof(Id));
            if (Stream.Length != MinSize + payloadLength) throw new ArgumentOutOfRangeException(nameof(Stream));
            var payload = new byte[payloadLength];
            Buffer.BlockCopy(Stream, HeaderSize, payload, 0, payloadLength);
            Payload = CreatePayload(frameType, payload);
            var checksum = BitConverter.ToUInt16(frame, frame.Length - ChecksumSize);
            if (checksum != CalculateChecksum(flag, Payload) || checksum > MaxChecksum) throw new ArgumentOutOfRangeException(nameof(Checksum));

            Flag = flag;
            FrameType = frameType;
            Id = id;
            Checksum = checksum;
        }

        /// <summary>
        ///     PayLoad 데이터에 Header, CheckSum 붙이기
        /// </summary>
        /// <returns></returns> UART Data Frame 형태로 buffer return
        private byte[] CreateStream()
        {
            var buffer = new byte[MinSize + Payload.Stream.Length]; // buffer 크기 = SerialFrame 최소 사이즈 + 데이터 전체 사이즈
            // delimiter +
            Buffer.BlockCopy(BitConverter.GetBytes(Delimiter), 0, buffer, 0, DelimiterSize);
            // delimiter + flag + 
            buffer[FlagOffset] = (byte)Flag;
            // delimiter + flag + payloadlength + 
            buffer[PayloadLengthOffset] = (byte)Payload.Stream.Length;
            // delimiter + flag + payloadlength + payload 
            Buffer.BlockCopy(Payload.Stream, 0, buffer, HeaderSize, Payload.Stream.Length); 
            // delimiter + flag + payloadlength + payload + checksum
            Buffer.BlockCopy(BitConverter.GetBytes((short)Checksum), 0, buffer, buffer.Length - ChecksumSize, ChecksumSize);
            return buffer;
        }

        /// <summary>
        ///     수신받은 데이터를 frameType에 맞게 저장
        /// </summary>
        /// <param name="frameType"></param> FrameType = 0~7
        /// <param name="payload"></param> 읽어드린 유효 데이터
        /// <returns></returns>
        private static IPayload CreatePayload(FrameType frameType, byte[] payload)
        {
            switch (frameType)
            {
                case FrameType.Heartbeat:
                    return new Heartbeat(payload);
                case FrameType.ReadyToSend:
                    return new ReadyToSend(payload);
                case FrameType.ClearToSend:
                    return new ClearToSend(payload);
                case FrameType.Acknowledgement:
                    return new Acknowledgement(payload);
                case FrameType.Diagnosis:
                    return new Diagnosis(payload);
                case FrameType.Simulation:
                    return new Simulation(payload);
                case FrameType.Rx:
                case FrameType.Tx:
                    return new UserPayload(payload);
                default:
                    throw new InvalidEnumArgumentException(nameof(frameType));
            }
        }

        private static int CalculateChecksum(int flag, IPayload payload)
        {
            var checksum = flag + payload.Stream.Length + payload.Stream.Sum(x => x);
            return checksum;
        }
    }
}