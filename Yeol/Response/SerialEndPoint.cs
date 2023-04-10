using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yeol;

namespace UiEMG.Response
{
    /// <summary>
    ///     SerialEndPoint
    ///         Payload를 읽는 역할
    /// </summary>
    public class SerialEndPoint : SerialEndPointBase
    {
        private readonly List<ReceiveData> _processValueList = new List<ReceiveData>();
        public SerialEndPoint(string name) : base(name)
        {
        }

        public event EventHandler<EventArgs<ReceiveData>> ProcessValueReceived;
        public event EventHandler<EventArgs<ArrayData>> ProcessValueArrayReceived;

        public void Send(IPayload payload)
        {
            if (!IsOpen) return;
            var frame = new SerialFrame(FrameType.Tx, 0, payload);
            base.Send(frame.Stream);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="payload"></param> ByteBuffer.Read(SerialFrame.HeaderSize, payloadLength, true);
        protected override void ProcessFrame(int flag, byte[] payload, int checksum)
        {
            var serialFrame = new SerialFrame(flag, payload, checksum); // Header + PayLoad + CheckSum
            //if (serialFrame.FrameType != FrameType.Rx) return;

            var delimiter = serialFrame.Stream.Take(SerialFrame.DelimiterSize).ToArray(); // Stream = Header + PayLoad + CheckSum, 0x8080
            var offset = BitConverter.ToUInt16(delimiter, 0); // 32896
            //var opCode = serialFrame.Payload.Stream[UserPayload.OpCodeOffset]; // opCode = Delimter

            switch (offset)
            {
                case ReceiveData.OffsetValue: // 정해놓은 Delimiter가 같은 경우
                    var value = new ReceiveData(serialFrame.Stream); // value = Header + Payload + Checksum 
                    ProcessValueReceived?.Invoke(this, new EventArgs<ReceiveData>(value));
                    _processValueList.Add(value);
                    if (_processValueList.Count >= ValueArraySize)
                    {
                        ProcessValueArrayReceived?.Invoke(this, new EventArgs<ArrayData>(new ArrayData(_processValueList)));
                        _processValueList.Clear();
                    }

                    break;
            }
        }


    }
}
