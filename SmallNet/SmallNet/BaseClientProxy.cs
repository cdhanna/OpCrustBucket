using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace SmallNet
{
    class BaseClientProxy 
    {
        private Socket socket;
        private StreamReader netReader;
        private StreamWriter netWriter;
        private NetModel model;
        private Thread recieverThread;
        public Boolean Debug { get; set; }

        public BaseClientProxy(Socket socket, NetModel model)
        {
            this.socket = socket;
            Stream socketStream = new NetworkStream(socket);
            this.netReader = new StreamReader(socketStream);
            this.netWriter = new StreamWriter(socketStream);

            this.netWriter.AutoFlush = true;
            this.model = model;

            this.startRecieverThread();
        }

        /// <summary>
        /// call this to log a message. It will only display if the Debug variable is true. It will be prefaced with "host-client-proxy: "
        /// </summary>
        /// <param name="str"></param>
        /// <param name="vars"></param>
        private void log(string str)
        {
            if (Debug)
            {
                Console.WriteLine("host-client-proxy: " + str);
            }
        }

        private void startRecieverThread()
        {
            this.recieverThread = new Thread(() =>
            {
                while (true) //listen forever
                {
                    string netMessage = netReader.ReadLine();
                    log("recieved msg- " + netMessage);
                    Tuple<string, string[]> data = SNetUtil.decodeMessage(netMessage);
                    recieveMessage(data.Item1, data.Item2);
                }
            });
            this.recieverThread.Start();
        }

        protected void recieveMessage(string msgType , params string[] parameters)
        {
            //TODO validate message
            //TODO apply message
            //TODO broadcast message

            this.sendMessage(msgType, parameters);
        }

        public void sendMessage(string msgType, params object[] parameters)
        {
            string msg = SNetUtil.encodeMessage(msgType, parameters);
            this.netWriter.WriteLine(msg, parameters);
            log("send msg- " + msg);
        }
    }
}
