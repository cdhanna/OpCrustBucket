using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Microsoft.Xna.Framework;

namespace SmallNet
{
    class TestClientModel : DefaultClientModel
    {


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
            //throw new NotImplementedException();
        }
    }
}
