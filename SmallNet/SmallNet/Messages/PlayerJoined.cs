using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNet.Messages
{
    class PlayerJoined : SMessage
    {
        public PlayerJoined() { }
        public PlayerJoined(Id id)
            : base(id)
        {
        }
    }
}
