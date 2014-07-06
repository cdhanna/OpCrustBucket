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

        public MoveMessage(Id id, int x, int y)
            : base(id)
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
