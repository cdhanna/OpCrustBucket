using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Microsoft.Xna.Framework;

namespace SmallNet
{
    public class TestClientModel : DefaultClientModel<DefaultPlayer>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        

        public override void init()
        {
           // throw new NotImplementedException();
        }

        public override void update(GameTime time)
        {
            //throw new NotImplementedException();
        }

        public override void destroy()
        {
            //throw new NotImplementedException();
        }

        public override bool validateMessage(SMessage message)
        {
           // throw new NotImplementedException();
            return true;
        }


        protected override void gotMessage(SMessage message)
        {
            //log.Debug(this.Owner + " : onMessage " + message);
        }

        public override void playerJoined(int id)
        {
            Console.WriteLine("PLAYER JOINED THE CLIENT MODEL "+ this.Id+ " : " + id);
        }
    }
}
