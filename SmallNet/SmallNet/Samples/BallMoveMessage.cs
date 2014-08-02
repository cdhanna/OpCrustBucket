using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.Samples
{
    class BallMoveMessage : SMessage
    {
        public BallMoveMessage()
            : base()
        {
        }

        private float x, y;


        public BallMoveMessage(float x, float y)
            : base()
        {
            this.x = x;
            this.y = y;
        }

        public void process(PongModel model)
        {
            model.BallX = this.x;
            model.BallY = this.y;
        }

    }
}
