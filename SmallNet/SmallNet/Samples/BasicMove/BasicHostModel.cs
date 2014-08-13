using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNet.Samples.BasicMove
{
    class BasicHostModel : DefaultHostModel<BasicClientModel>
    {


        public override void updateHost(Microsoft.Xna.Framework.GameTime time)
        {
            
        }

        public override void init()
        {
            Console.WriteLine("starting server");    
        }

        public override void destroy()
        {
            
        }

        public override bool validateMessage(SMessage message)
        {
            return true;
        }

        public override void onMessage(SMessage message)
        {
            
        }

        public override void playerJoined(int id)
        {
            
        }
    }
}
