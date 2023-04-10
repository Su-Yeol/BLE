namespace Yeol
{
    public enum ExceptionType
    {
        Success = 0,
        PlatformNotSupported = 1,
        FunctionNotSupported = 2,
        HeartbeatNotSupported = 3,
        AckNotSupported = 4,
        UserNotSupported = 5,
        HeartbeatNotFound = 6,
        AckNotFound = 7,
        NotConnected = 8,
        InvalidChecksum = 9,
        InvalidArgument = 10,
        ConnectionRefused = 11,
        FrameRefused = 12,
        RxFull = 13,
        TxFull = 14,
        Fault = 15
    }
}