using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.Messages
{
    class DisconnectionMessage : SMessage
    {
        public DisconnectionMessage() : base() { }
        public DisconnectionMessage(Id id) : base (id) { }
    }
}
