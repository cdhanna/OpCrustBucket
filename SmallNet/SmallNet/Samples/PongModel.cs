using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SmallNet.Samples
{
    class PongModel : DefaultClientModel
    {
        
        public float Player1Y { get; set; }
        public float Player2Y { get; set; }
        public float PlayerHeight { get; set; }
        public float PlayerWidth { get; set; }
        public float BallX { get; set; }
        public float BallY { get; set; }
        public const float BallSpeed = 1.0f;

        private KeyboardHelper keyBoard;

        public override void init()
        {
            this.Player1Y = 200f ;
            this.Player2Y = 200f ;
            this.BallX = 400f;
            this.BallY = 200f;
            this.PlayerHeight = 50f;
            this.PlayerWidth = 20f;

            this.keyBoard = new KeyboardHelper();
        }

        public override void update(Microsoft.Xna.Framework.GameTime time)
        {
            
            if (this.keyBoard.KeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                //this.Player1Y -= .01f;
                this.sendMessage(new PaddleMoveMessage(1, -1));
            }
            if (this.keyBoard.KeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                //this.Player1Y += .01f;
                this.sendMessage(new PaddleMoveMessage(1, 1));
            }

            if (this.BallX < 50)// && this.BallY > this.Player1Y && this.BallY + 20 < this.Player1Y + this.PlayerHeight)
            {
                this.sendMessage(new ChangeBallVelMessage(1, 0));
            }


            this.keyBoard.Update();
        }

        public override void destroy()
        {
            
        }

        public override bool validateMessage(SMessage message)
        {
            return true;
        }

        protected override void gotMessage(SMessage message)
        {
            if (message is PaddleMoveMessage)
            {
                ((PaddleMoveMessage)message).process(this);
            }
            else if (message is BallMoveMessage)
            {
                ((BallMoveMessage)message).process(this);
            }
        }

        public void draw(Vector2 screenSize, PrimitiveBatch pb)
        {
            
            //draw player1
            pb.Begin(Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleStrip);
            this.drawBox(pb,Color.Red, 50f, Player1Y - PlayerHeight/2, PlayerWidth, PlayerHeight);
            pb.End();

            ////draw player2
            pb.Begin(Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleStrip);
            this.drawBox(pb, Color.Blue, 750, Player2Y - PlayerHeight/2, PlayerWidth, PlayerHeight);
            pb.End();

            ////draw ball
            pb.Begin(Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleStrip);
            this.drawBox(pb, Color.Black, BallX, BallY, 20, 20);
            pb.End();
        }

        private void drawBox(PrimitiveBatch pb,Color c, float x, float y, float width, float height)
        {
            pb.AddVertex(new Vector2(x, y), c);
            pb.AddVertex(new Vector2(x + width, y),c);
            pb.AddVertex(new Vector2(x + width, y + height),c);
            pb.AddVertex(new Vector2(x, y + height),c);
        }

    }
}
