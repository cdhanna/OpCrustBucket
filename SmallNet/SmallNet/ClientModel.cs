using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SmallNet
{
    interface ClientModel
    {
        void init(string owner);
        void update(GameTime time);
        void destroy();

        string getOwner();

    }
}
