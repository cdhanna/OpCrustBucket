using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet
{

    /// <summary>
    /// Core class for message passing between host and client. 
    /// </summary>
    public abstract class SMessage
    {

        /// <summary>
        /// the time that the message was created
        /// </summary>
        private long time;

        /// <summary>
        /// the id of the net-machine that this message originated on
        /// </summary>
        private int senderId;

        /// <summary>
        /// create a new SMessage.
        /// </summary>
        /// <param name="id"></param>
        public SMessage(Id id)
        {
            time = SNetUtil.getCurrentTime();
            this.senderId = id.Id;
        }

        /// <summary>
        /// DO NOT USE THIS CONSTRUCTOR. IT IS FOR SERIALIZATION ONLY.
        /// </summary>
        public SMessage() {
            this.senderId = -1;
        }

        /// <summary>
        /// The time the message was created on its original machine
        /// </summary>
        public long TimeSent { get { return this.time; } }

        /// <summary>
        /// the id of the net-machine that this message came from
        /// </summary>
        public int SenderId { get { return this.senderId; } }
    }

    

}
