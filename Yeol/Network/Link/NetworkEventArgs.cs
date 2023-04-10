using System;

namespace Yeol
{
    public class NetworkEventArgs<TValue> : EventArgs
    {
        public NetworkEventArgs(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
    }
}
