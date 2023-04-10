namespace Yeol
{
    public enum FrameType
    {
        Heartbeat = 0,
        ReadyToSend = 1,
        ClearToSend = 2,
        Acknowledgement = 3,
        Diagnosis = 4,
        Simulation = 5,
        Rx = 6, // 송신 - 0xC0(1100 0000)
        Tx = 7 // 수신 - 0xE0(1110 0000)
    }
}