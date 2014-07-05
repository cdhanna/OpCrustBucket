using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet
{
    public class MessageEventArgs : EventArgs
    {
        private String msgType;
        private String[] parameters;
        public MessageEventArgs(String msgType, String[] parameters)
            : base()
        {
            this.msgType = msgType;
            this.parameters = parameters;
        }
        public String getMsgType()
        {
            return this.msgType;
        }
        public String[] getParameters()
        {
            return this.parameters;
        }
    }
}
