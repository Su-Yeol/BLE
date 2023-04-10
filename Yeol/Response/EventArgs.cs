using System;

namespace UiEMG
{
    public class EventArgs<TValue> : EventArgs
    {
        public EventArgs(TValue value)
        {
            Value = value;
        }

        public TValue Value { get; }
    }
}
