using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SmallNet;

namespace SmallNet.Samples.BasicMove
{
    class BasicClientModel : DefaultClientModel
    {
        public override void init()
        {
            Console.WriteLine("Client starting");
        }

        public override void update(Microsoft.Xna.Framework.GameTime time)
        {
           
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
            Console.WriteLine(this.Id + " MESSAGE!!!" + message);
            
        }
    }
}
