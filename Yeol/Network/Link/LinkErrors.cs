using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yeol
{
    [Flags]
    public enum LinkErrors
    {
        /// <summary>
        ///     Indicates no error.
        /// </summary>
        NoError = 0,

        /// <summary>
        ///     Driver buffer has reached 80% full.
        /// </summary>
        RxOver = 1,

        /// <summary>
        ///     Driver has detected an overflow.
        /// </summary>
        Overrun = 2,

        /// <summary>
        ///     Parity error detected.
        /// </summary>
        RxParity = 4,

        /// <summary>
        ///     Frame error detected.
        /// </summary>
        Frame = 8,

        #region User

        /// <summary>
        ///     Rx Thread is faulted.
        /// </summary>
        RxThreadFaulted = 128,

        #endregion

        /// <summary>
        ///     Transmit buffer is full.
        /// </summary>
        TxFull = 256
    }
}
