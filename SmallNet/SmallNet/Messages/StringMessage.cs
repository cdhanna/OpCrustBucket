using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.Messages
{
    /// <summary>
    /// Simple message that sends a string over the network
    /// </summary>
    public class StringMessage : SMessage
    {
        private String msg;
        public StringMessage(Id id, String msg)
            : base(id)
        {
            this.msg = msg;
        }
        public StringMessage() : base() { }

        /// <summary>
        /// the string that was sent over the network
        /// </summary>
        public String Message { get { return this.msg; } }

        public override string ToString()
        {
            return this.msg;
        }

    }
}
