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
    abstract class DefaultClientModel : ClientModel
    {
        protected static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);



        private string name;
        private System.Timers.Timer timer;
        private StreamWriter netWriter;

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

        public abstract void init();

        public abstract void update(Microsoft.Xna.Framework.GameTime time);

        public abstract void destroy();

        public abstract bool validateMessage(string msgType, params string[] parameters);
        public abstract void onMessage(string msgType, params string[] parameters);



        public virtual void sendMessage(string msgType, params object[] parameters)
        {
            //construct a message
            string msg = SNetUtil.encodeMessage(msgType, parameters);

            //send the message
            this.netWriter.WriteLine(msg, parameters);
            log.Debug("send msg- " + msg);
        }

        public void keepTime()
        {


        }
    }
}
