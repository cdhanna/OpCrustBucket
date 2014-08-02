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
    public abstract class DefaultClientModel : ClientModel
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        private string name;
        private System.Timers.Timer timer;
        private StreamWriter netWriter;
        private int id;

        private EventHandler<MessageEventArgs> messageRecieved;
        public EventHandler<MessageEventArgs> MessageRecieved { get { return this.messageRecieved; } set { this.messageRecieved = value; } }

        public DefaultClientModel()
        {
            this.timer = new System.Timers.Timer();
        }

        public double TargetTime
        {
            get;
            set;
        }
        public string Owner { get{ return this.name;} }
        public int Id { get { return this.id; } }
        public void create(StreamWriter netWriter, string owner)
        {
            this.TargetTime = 30;
            this.name = owner;
            this.netWriter = netWriter;
            this.timer.AutoReset = true;
            this.timer.Interval = this.TargetTime;
            this.timer.Enabled = true;



            this.init();
        }

        public void setId(int id)
        {
            this.id = id;
        }

        public abstract void init();

        public abstract void update(Microsoft.Xna.Framework.GameTime time);

        public abstract void destroy();

        public abstract bool validateMessage(SMessage message);


        public void onMessage(SMessage message)
        {

            log.Debug("MESSAGE GOT: " + (SNetUtil.getCurrentTime() - message.TimeSent));


            if (messageRecieved != null)
            {
                MessageRecieved(this, new MessageEventArgs(message));
            }
            this.gotMessage(message);
        }
        protected abstract void gotMessage(SMessage message);


        public virtual void sendMessage(SMessage message)
        {
            //construct a message
            String msg = SNetUtil.encodeMessage(message);

            this.sendMessage(msg);
        }

        public virtual void sendMessage(String rawMessage)
        {
            //send the message
            this.netWriter.WriteLine(rawMessage);
            this.netWriter.Flush();
            log.Debug(this.Owner + " send msg- " + rawMessage);
        }

        public void keepTime()
        {


        }
    }
}
