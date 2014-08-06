using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallNet.Messages
{
    /// <summary>
    /// simple message that notifies machines a player has joined. The ID is set to the id of the player that just joined.
    /// </summary>
    class PlayerJoined : SMessage
    {
        public PlayerJoined() { }
        public PlayerJoined(Id id)
            : base(id)
        {
        }
    }
}
