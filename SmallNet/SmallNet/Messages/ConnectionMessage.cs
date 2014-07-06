using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.Messages
{
    class ConnectionMessage : SMessage
    {

        private String credentials;
        public ConnectionMessage(Id id, String credentials)
            : base(id)
        {
            this.credentials = credentials;
        }

        public ConnectionMessage() : base() { }


        public String Credentials { get { return this.credentials; } }

    }
}
