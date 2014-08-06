using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace SmallNet
{
    /// <summary>
    /// The default client model that will be run on SmallNet projects. It is recommended that everytime a new SmallNet project is started, the ClientModel uses this class as a base.
    /// WARNING: ALL implementations must have a no-arg constructor. That is what will be used to construct the client model
    /// </summary>
    public abstract class DefaultClientModel : ClientModel
    {

        /// <summary>
        /// log object
        /// </summary>
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// where the client model is running, either on the client, or on the host
        /// </summary>
        private NetworkSide name;


        /// <summary>
        /// the stream to write data to the host
        /// </summary>
        private StreamWriter netWriter;

        /// <summary>
        /// a unique ID that matches the id of the model's corresponding client|clientproxy
        /// </summary>
        private int id;

        private EventHandler<MessageEventArgs> messageRecieved;
        public EventHandler<MessageEventArgs> MessageRecieved { get { return this.messageRecieved; } set { this.messageRecieved = value; } }

        /// <summary>
        /// Creates a new client model
        /// </summary>
        public DefaultClientModel()
        {
            
        }

        /// <summary>
        /// DEPRECATED
        /// </summary>
        public double TargetTime
        {
            get;
            set;
        }

        public NetworkSide Owner { get{ return this.name;} }
        public int Id { get { return this.id; } }


        /// <summary>
        /// called from the client|proxy, right after the constructor is called.
        /// </summary>
        /// <param name="netWriter"></param>
        /// <param name="owner"></param>
        public void create(StreamWriter netWriter, NetworkSide owner)
        {
            this.TargetTime = 30;
            this.name = owner;
            this.netWriter = netWriter;



            //call init, which new implementations must define
            this.init();
        }

        /// <summary>
        /// set the id of the model. Dangerous. 
        /// </summary>
        /// <param name="id"></param>
        public void setId(int id)
        {
            this.id = id;
        }

        /// <summary>
        /// new implementation must define this. It will be called when the model is first created.
        /// Owner will be set
        /// Netwriter will be set
        /// 
        /// </summary>
        public abstract void init();

        /// <summary>
        /// new implementation must define this. It will be called to update the model state
        /// </summary>
        /// <param name="time"></param>
        public abstract void update(Microsoft.Xna.Framework.GameTime time);

        /// <summary>
        /// called when the client must disconnect
        /// </summary>
        public abstract void destroy();

        /// <summary>
        /// new implementation must define this. It will be called when a message needs to be validated.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>true if the message should be allowed to propegate through the network</returns>
        public abstract bool validateMessage(SMessage message);

        /// <summary>
        /// new implementation must define this. It will be called whenever a new player joins the host.
        /// </summary>
        /// <param name="id"></param>
        public abstract void playerJoined(int id);

        /// <summary>
        /// Called when a message is recieved.
        /// </summary>
        /// <param name="message"></param>
        public void onMessage(SMessage message)
        {

            log.Debug("MESSAGE GOT: " + (SNetUtil.getCurrentTime() - message.TimeSent));
            if (messageRecieved != null)
            {
                MessageRecieved(this, new MessageEventArgs(message));
            }
            this.gotMessage(message);
        }

        /// <summary>
        /// new implementation must define this. It will be called whenever a message is recieved
        /// </summary>
        /// <param name="message"></param>
        protected abstract void gotMessage(SMessage message);

        /// <summary>
        /// sends a message to the host
        /// </summary>
        /// <param name="message"></param>
        public virtual void sendMessage(SMessage message)
        {
            //construct a message
            String msg = SNetUtil.encodeMessage(message);

            this.sendMessage(msg);
        }

        /// <summary>
        /// sends a message to the host
        /// </summary>
        /// <param name="rawMessage">must be a well-formed Smessage serialable string</param>
        public virtual void sendMessage(String rawMessage)
        {
            //send the message
            this.netWriter.WriteLine(rawMessage);
            this.netWriter.Flush();
            log.Debug(this.Owner + " send msg- " + rawMessage);
        }

    }
}
