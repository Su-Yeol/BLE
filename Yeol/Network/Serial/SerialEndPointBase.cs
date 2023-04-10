using System;

namespace Yeol

{
    /// <summary>
    ///     SerialEndPintBase
    ///         SeraiFram(Delimeter + Flag(0) + Length + Payload + Checksum) 각각을 읽고 해당 변수에 넣기
    /// </summary>
    public abstract class SerialEndPointBase : SerialPortBase
    {
        protected SerialEndPointBase(string name) : base(name)
        {
        }

        /// <summary>
        ///     SerialFrame = Header(Delimiter(2) + Flag(1) + Payloadlength(1)) + PayLoad( n : 0~127) + CheckSum(2)
        /// </summary>
        protected override void ProcessBuffer()
        {
            var buffer = ByteBuffer.Read(SerialFrame.DelimiterSize, true); // delimiter 데이터 읽기 
            int delimiter = BitConverter.ToUInt16(buffer, 0); // Delimiter: 0x8080 2byte이기 때문에 변환이 필요 

            if (delimiter != SerialFrame.Delimiter) // 데이터 Frame이 다른 경우: 올바른 데이터X
            {
                ByteBuffer.MoveNext(MoveNextLength); // 한 칸씩 움직이면서 올바른 delimiter 탐색
                IgnoredByteCount += MoveNextLength; // 무시된 데이터의 카운팅 숫자(길이)
            }
            else // 데이터 Frame이 같은 경우: 올바른 데이터 O
            {
                var bufferLength = ByteBuffer.Length;
                buffer = ByteBuffer.Read(SerialFrame.HeaderSize, true); // Data Frame에서 Header 부분 먼저 읽기
                var flag = buffer[SerialFrame.FlagOffset]; // flag 위치 = buffer index 2
                var payloadLength = buffer[SerialFrame.PayloadLengthOffset]; // payloadlength 위치 = buffer index 3
                var frameSize = SerialFrame.MinSize + payloadLength; // buffer 전체 사이즈: 최소 사이즈(delimiter, flag, length, chk) + payload
                if (bufferLength < frameSize) return; // buffer 사이즈보다 크면 return

                var payload = ByteBuffer.Read(SerialFrame.HeaderSize, payloadLength, true); // PayLoad 데이터만 읽기
                var checksum = BitConverter.ToUInt16(ByteBuffer.Read(frameSize - SerialFrame.ChecksumSize, SerialFrame.ChecksumSize, true), 0);
                ByteBuffer.MoveNext(frameSize); // 다음 데이터를 읽기 위해서 shift
                if (IgnoredByteCount != 0) OnByteIgnored(); // 올바르지 않은 데이터가 들어오면 error event 발생
                ProcessFrame(flag, payload, checksum); 
            }
        }

        protected abstract void ProcessFrame(int flag, byte[] payload, int checksum);
    }
}