using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet
{
    public abstract class SMessage
    {
        private long time;
        private int senderId;


        public SMessage(Id id)
        {
            time = SNetUtil.getCurrentTime();
            this.senderId = id.Id;
        }
        public SMessage() {
           // time = SNetUtil.getCurrentTime();
            this.senderId = -1;
        }

        public long TimeSent { get { return this.time; } }
        public int SenderId { get { return this.senderId; } }
    }

    

}
