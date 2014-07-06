using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.Messages
{
    class StringMessage : SMessage
    {
        private String msg;
        public StringMessage(Id id, String msg)
            : base(id)
        {
            this.msg = msg;
        }
        public StringMessage() : base() { }

        public String Message { get { return this.msg; } }

        public override string ToString()
        {
            return this.msg;
        }

    }
}
