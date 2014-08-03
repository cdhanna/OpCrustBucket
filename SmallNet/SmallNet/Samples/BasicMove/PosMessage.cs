using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmallNet;
using Microsoft.Xna.Framework;
namespace SmallNet.Samples.BasicMove
{
    class PosMessage : SMessage
    {

        private int x, y;

        public PosMessage() { }

        public PosMessage(Id id, int x, int y) : base(id)
        {
            
            this.x = x;
            this.y = y;
        }

        public int getX()
        {
            return x;
        }
        public int getY()
        {
            return y;
        }
    }
}
