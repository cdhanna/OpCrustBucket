using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.Samples
{
    class ChangeBallVelMessage : SMessage
    {

        private float velX, velY;

        public ChangeBallVelMessage() : base() { }

        public ChangeBallVelMessage(float velX, float velY) : base(){
            this.velX = velX;
            this.velY = velY;
        }

        public void process(PongHostModel host)
        {
            host.BallVelX = velX;
            host.BallVelY = velY;
        }

    }
}
