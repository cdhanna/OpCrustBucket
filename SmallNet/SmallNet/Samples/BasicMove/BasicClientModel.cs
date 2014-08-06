using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmallNet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SmallNet.Samples.BasicMove
{
    class BasicClientModel : DefaultClientModel, Id
    {

        private Dictionary<int, BasicPlayer> playerMap;
        private List<int> playerIds;

        private BasicPlayer me;

        private KeyboardHelper keyboard;

        public override void init()
        {
            Console.WriteLine("Client starting");
           
            playerMap = new Dictionary<int, BasicPlayer>();
            playerIds = new List<int>();

           

            keyboard = new KeyboardHelper();
        }

        public void addPlayer(int id, BasicPlayer player)
        {
            playerIds.Add(id);
            playerMap[id] = player;
        }

        private void runInput()
        {
            if (this.Owner == NetworkSide.Client)
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
            }
        }

        public override void update(Microsoft.Xna.Framework.GameTime time)
        {

            runInput();
            this.keyboard.Update();


            doForAllPlayers((plr) => { plr.update(); });


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
                    //Console.WriteLine(SNetUtil.getCurrentTime() + " : " + msg.SenderId + " : " + this.playerMap.ContainsKey(msg.SenderId));
                    playerMap[msg.SenderId].Velocity = new Vector2(msg.X, msg.Y);
                }
            }
            
        }

        public override void playerJoined(int id)
        {
            Console.WriteLine(Owner + " : " + Id + " PLAYER JOIN: " + id);

            if (id == Id)
            {
                //I joined!!!
                me = new BasicPlayer(id, new Vector2(200, 200));
                this.addPlayer(id, me);
            }
            else
            {
                //some one else joined!
                BasicPlayer other = new BasicPlayer(id, new Vector2(200, 200));
                this.addPlayer(id, other);
            }
        }

        public void draw(PrimitiveBatch prim, SpriteBatch spr, SpriteFont font)
        {
            doForAllPlayers((plr) => { plr.draw(prim, spr, font); });
           // netman.draw(prim);
        }

        delegate void PlayerFunc(BasicPlayer plr);
        private void doForAllPlayers(PlayerFunc d)
        {
            foreach (int player in this.playerIds)
            {
                d.Invoke(this.playerMap[player]);
            }
        }

        int Id.Id
        {
            get { return Id; }
        }
    }
}
