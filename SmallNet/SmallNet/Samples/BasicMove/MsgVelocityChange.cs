using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNet.Samples.BasicMove
{
    class MsgVelocityChange : SMessage
    {
        int x, y;

        public MsgVelocityChange(Id id, int x, int y)
            : base(id)
        {
            this.x = x;
            this.y = y;
        }
        public MsgVelocityChange() { }

        public int X { get { return x; } }
        public int Y { get { return y; } }

    }
}
