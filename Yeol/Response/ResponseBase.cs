using Yeol;

namespace UiEMG
{
    public abstract class ResponseBase : UserPayload
    {
        protected ResponseBase(byte[] stream) : base(stream)
        {
        }
    }

}
