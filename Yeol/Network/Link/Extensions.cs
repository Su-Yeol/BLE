using RJCP.IO.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yeol
{
    public static class Extensions
    {
        // 남이 짜놓은 코드에 LinkError를 추가하고 싶을때 
        public static LinkErrors ToLinkErrors(this SerialError value)
        {
            return (LinkErrors)(byte)value;
        }
    }
}
