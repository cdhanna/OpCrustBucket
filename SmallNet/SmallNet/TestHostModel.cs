using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet
{
    class TestHostModel : DefaultHostModel<TestClientModel>
    {
        public override void updateHost(Microsoft.Xna.Framework.GameTime time)
        {
           // throw new NotImplementedException();
        }

        public override void init()
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

        public override void onMessage(SMessage message)
        {
            //throw new NotImplementedException();
        }
    }
}
