using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Microsoft.Xna.Framework;

namespace SmallNet
{
    class TestClientModel : ClientModel
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private int ticks;
        private string owner;

        public void init(string owner)
        {
            this.owner = owner;
            this.ticks = 0;
        }

        public void update(GameTime time)
        {
            ticks++;
            //log.Debug(owner + ": " + ticks);
        }

        public void destroy()
        {
            
        }


        public string getOwner()
        {
            return this.owner;
        }
    }
}
