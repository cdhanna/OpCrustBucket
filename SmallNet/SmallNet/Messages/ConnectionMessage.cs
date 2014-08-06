using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.Messages
{
    /// <summary>
    /// simple message that notifies net machines of a new connection
    /// </summary>
    class ConnectionMessage : SMessage
    {

        private String credentials;
        public ConnectionMessage(Id id, String credentials)
            : base(id)
        {
            this.credentials = credentials;
        }

        public ConnectionMessage() : base() { }

        /// <summary>
        /// the credential string that the new client is trying to join with.
        /// </summary>
        public String Credentials { get { return this.credentials; } }

    }
}
