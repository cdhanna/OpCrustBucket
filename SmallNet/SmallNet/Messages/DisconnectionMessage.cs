using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.Messages
{
    /// <summary>
    /// simple message that notifies net machines when a disconnection is happening
    /// </summary>
    class DisconnectionMessage : SMessage
    {
        public DisconnectionMessage() : base() { }
        public DisconnectionMessage(Id id) : base (id) { }
    }
}
