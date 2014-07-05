using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet
{
    public abstract class SMessage
    {
        private long time; 

        public SMessage()
        {
            time = SNetUtil.getCurrentTime();
        }

        public long TimeSent { get { return this.time; } }

    }

    

}
