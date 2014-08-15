using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;


namespace SmallNet.Messages
{
    /// <summary>
    /// simple message that notifies net machines of a new connection
    /// </summary>
    class DummyMessage : SMessage
    {
        private Vector2 myPos;
        private String credentials;
        public DummyMessage(Id id, String credentials, float a, float b)
            : base(id)
        {
            this.credentials = credentials;
            this.myPos = new Vector2(a, b);
        }

        public DummyMessage() : base() { }

        /// <summary>
        /// the credential string that the new client is trying to join with.
        /// </summary>
        public String Credentials { get { return this.credentials; } }

    }
}
