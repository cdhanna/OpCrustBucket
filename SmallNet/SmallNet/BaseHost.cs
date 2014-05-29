using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
namespace SmallNet
{
   
    class BaseHost : Host
    {
        private TcpListener tcpListener;
        private string ipAddress;
        private Thread clientAcceptorThread;
        private NetModel model;
        public Boolean Debug { get; set; }
        public string IpAddress { get { return this.ipAddress; } }

        public NetModel Model { get { return this.model; } }

        public BaseHost()
        {
            this.model = new NetModel();
           
            this.ipAddress = SNetUtil.getLocalIp();
            this.tcpListener = new TcpListener(IPAddress.Parse(this.ipAddress), SNetProp.getPort());
            this.clientAcceptorThread = new Thread(() =>
            {
                log("client acceptor thread is starting...");
                while (true)
                {
                    //accept a new client
                    Socket socket = tcpListener.AcceptSocket();
                    //create clientProxy, which puts it into the model
                    BaseClientProxy client = new BaseClientProxy(socket, model);
                    client.Debug = Debug;
                    log("got a connection");
                }
            });
        }

        /// <summary>
        /// call this to log a message. It will only display if the Debug variable is true. It will be prefaced with "host: "
        /// </summary>
        /// <param name="str"></param>
        /// <param name="vars"></param>
        private void log(string str)
        {
            if (Debug)
            {
                Console.WriteLine("host: " + str);
            }
        }

        public void start()
        {
            this.tcpListener.Start();
            this.clientAcceptorThread.Start();
        }

        public void stop()
        {
            this.clientAcceptorThread.Abort();
            this.tcpListener.Stop();
        }

        public void sendMessageToAll(string msgType, params object[] parameters)
        {
            this.model.sendMessageToAll(msgType, parameters);
        }
    }
}
