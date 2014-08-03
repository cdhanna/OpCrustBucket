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
            if (keyboard.NewKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            {
                this.sendMessage(new MsgVelocityChange(this, 1, 0));
            }
            if (keyboard.NewKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            {
                this.sendMessage(new MsgVelocityChange(this, -1, 0));
            }
            if (keyboard.NewKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
            {
                this.sendMessage(new MsgVelocityChange(this, 0, -1));
            }
            if (keyboard.NewKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            {
                this.sendMessage(new MsgVelocityChange(this, 0, 1));
            }


            if (keyboard.NewKeyUp(Microsoft.Xna.Framework.Input.Keys.D))
            {
                this.sendMessage(new MsgVelocityChange(this, 0, 0));
            }
            if (keyboard.NewKeyUp(Microsoft.Xna.Framework.Input.Keys.A))
            {
                this.sendMessage(new MsgVelocityChange(this, 0, 0));
            }
            if (keyboard.NewKeyUp(Microsoft.Xna.Framework.Input.Keys.W))
            {
                this.sendMessage(new MsgVelocityChange(this, 0, 0));
            }
            if (keyboard.NewKeyUp(Microsoft.Xna.Framework.Input.Keys.S))
            {
                this.sendMessage(new MsgVelocityChange(this, 0, 0));
            }

            this.keyboard.Update();


            me.update();
            netman.update();
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
            if (message.SenderId != 0)
            {
                if (message is MsgVelocityChange)
                {
                    MsgVelocityChange msg = (MsgVelocityChange)message;

                    if (msg.SenderId == Id)
                    {
                        me.Velocity = new Vector2(msg.X, msg.Y);
                    }
                    else
                    {
                       // Console.WriteLine(msg.SenderId);
                        netman.Velocity = new Vector2(msg.X, msg.Y);
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
