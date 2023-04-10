namespace Yeol
{
    /// <summary>
    ///     TX 수신받은 payload 데이터 저장
    /// </summary>
    public class UserPayload : PayloadBase, IUserPayload
    {
        public const int OpCodeSize = 1;
        public const int OpCodeOffset = 0;

        public UserPayload(int size) : base(size)
        {
        }

        public UserPayload(byte[] stream) : base(stream)
        {
        }
    }
}