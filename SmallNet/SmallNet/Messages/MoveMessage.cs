using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.Messages
{
    class MoveMessage : SMessage
    {
        public int x, y;

        public MoveMessage()
            : base()
        {
        }

        public MoveMessage(int x, int y)
            : base()
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return "X:" + x + " Y: " + y;
        }
    }
}
