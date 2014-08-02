using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.Samples
{
    class PongHostModel : DefaultHostModel<PongModel>
    {

        public float BallX { get; set; }
        public float BallY { get; set; }

        public float BallVelX { get; set; }
        public float BallVelY { get; set; }

        public override void updateHost(Microsoft.Xna.Framework.GameTime time)
        {
            this.BallX += this.BallVelX;
            this.BallY += this.BallVelY;

            this.sendMessageToAll(new BallMoveMessage(this.BallX, this.BallY));

        }

        public override void init()
        {
            this.BallX = 400;
            this.BallY = 200;

            this.BallVelX = -1;
            this.BallVelY = 0;
        }

        public override void destroy()
        {
            
        }

        public override bool validateMessage(SMessage message)
        {
            return true;
        }

        public override void onMessage(SMessage message)
        {
            if (message is ChangeBallVelMessage)
            {
                ((ChangeBallVelMessage)message).process(this);
            }
            
        }
    }
}
