using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet
{
    public class MessageEventArgs : EventArgs
    {
        private SMessage message;
        public MessageEventArgs(SMessage message)
            : base()
        {
            this.message = message;
        }
        public SMessage getMessage()
        {
            return this.message;
        }
    }
}
