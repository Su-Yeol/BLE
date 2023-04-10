using System;
using System.Linq;
using Yeol;

namespace UiEMG
{
    /// <summary>
    ///     BLE 통신을 통해 전달받은 데이터 분리
    ///     ResponseBse: UserPayload의 상속 class
    ///     UserPayload: 수신받은 payload 데이터를 저장
    /// </summary>
    public class ReceiveData : ResponseBase
    {
        //public const byte OpCode = 0x8080;
        public const int OffsetValue = SerialFrame.Delimiter;
        public const int Size = 8; // delimiter(2) + flag(1) + payloadlength(1) + payload(emg1, emg2 - 2) + chk(2)

        public int EMG1 { get; }
        public int EMG2 { get; }

        public ReceiveData(byte[] stream) : base(stream)
        {
            // Stream: 일련의 데이터가 직렬로 이동하는 상태를 가리킨다. 프로그램이 주변 장치와 통신하는 수단.
            var delimiter = stream.Take(SerialFrame.DelimiterSize).ToArray(); // 0x8080
            var offset = BitConverter.ToUInt16(delimiter, 0); // 32896

            ///<summary>
            ///     Header error, Data Size error
            ///</summary>
            if (offset != OffsetValue) throw new ArgumentException(nameof(OffsetValue));
            if (stream.Length != Size) throw new ArgumentException(nameof(stream));

            EMG1 = stream[SerialFrame.HeaderSize];
            EMG2 = stream[SerialFrame.HeaderSize+1];
            //PWM = BitConverter.ToInt32(stream, 3) / 100.0; // 1byte 이상일 때만 사용
        }

    }
}
