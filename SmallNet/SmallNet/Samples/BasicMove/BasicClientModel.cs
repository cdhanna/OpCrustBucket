﻿using System;
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

        private Dictionary<int, BasicPlayer> playerMap;
        private List<int> playerIds;

        private BasicPlayer me;

        private KeyboardHelper keyboard;

        public override void init()
        {
            Console.WriteLine("Client starting");
            me = new BasicPlayer(new Vector2(200, 200));

            playerMap = new Dictionary<int, BasicPlayer>();
            playerIds = new List<int>();

           

            keyboard = new KeyboardHelper();
        }

        public void addPlayer(int id, BasicPlayer player)
        {
            playerIds.Add(id);
            playerMap[id] = player;
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
            if (id == Id)
            {
                //I joined!!!
                me = new BasicPlayer(new Vector2(200, 200));
                this.addPlayer(id, me);
            }
            else
            {
                //some one else joined!
                BasicPlayer other = new BasicPlayer(new Vector2(200, 200));
                this.addPlayer(id, other);
            }
        }

        public void draw(PrimitiveBatch prim)
        {
            doForAllPlayers((plr) => { plr.draw(prim); });
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