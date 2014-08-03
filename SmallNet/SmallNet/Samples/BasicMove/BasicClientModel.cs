using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmallNet;
using Microsoft.Xna.Framework;

namespace SmallNet.Samples.BasicMove
{
    class BasicClientModel : DefaultClientModel, Id
    {

        private BasicPlayer me, netman;

        private KeyboardHelper keyboard;

        public override void init()
        {
            Console.WriteLine("Client starting");
            me = new BasicPlayer(new Vector2(200, 200));
            netman = new BasicPlayer(new Vector2(200, 200));
            keyboard = new KeyboardHelper();
        }

        public override void update(Microsoft.Xna.Framework.GameTime time)
        {
            if (keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                this.sendMessage(new PosMessage(this, 1 + (int)me.Position.X, (int)me.Position.Y));
            }
            if (keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                this.sendMessage(new PosMessage(this, -1 + (int)me.Position.X, (int)me.Position.Y));
            }
            if (keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                this.sendMessage(new PosMessage(this, (int)me.Position.X, -1 + (int)me.Position.Y));
            }
            if (keyboard.KeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                this.sendMessage(new PosMessage(this, (int)me.Position.X, 1 + (int)me.Position.Y));
            }

            this.keyboard.Update();
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
            if (Id != 0)
            {
                if (message is PosMessage)
                {
                    PosMessage msg = (PosMessage)message;

                    if (msg.SenderId == Id)
                    {
                        me.Position = new Vector2(msg.getX(), msg.getY());
                    }
                    else
                    {
                        netman.Position = new Vector2(msg.getX(), msg.getY());
                    }
                }
            }
            
        }



        public void draw(PrimitiveBatch prim)
        {
            me.draw(prim);
            netman.draw(prim);
        }



        int Id.Id
        {
            get { return Id; }
        }
    }
}
