using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.Samples
{
    class PaddleMoveMessage : SMessage
    {

        private int player;
        private float direction;

        public PaddleMoveMessage() : base() { }

        public PaddleMoveMessage(int player, float direction)
            : base()
        {
            this.player = player;
            this.direction = direction;
        }

        public int Player { get { return this.player; } }
        public float Direction { get { return this.direction; } }

        public void process(PongModel model)
        {
            if (Player == 1)
            {
                model.Player1Y += 1f * direction;
            }
            else
            {
                model.Player2Y += 1f * direction;
            }
        }

    }
}
