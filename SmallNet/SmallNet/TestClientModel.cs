using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Microsoft.Xna.Framework;

namespace SmallNet
{
    public class TestClientModel : DefaultClientModel
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

        public override bool validateMessage(string msgType, params string[] parameters)
        {
           // throw new NotImplementedException();
            return true;
        }

        public override void onMessage(string msgType, params string[] parameters)
        {
            log.Debug(this.Owner + " : onMessage " + msgType);
        }
    }
}
