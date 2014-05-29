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

        public string IpAddress { get { return this.ipAddress; } }

        public BaseHost()
        {
            this.model = new NetModel();

            this.ipAddress = SNetUtil.getLocalIp();
            this.tcpListener = new TcpListener(IPAddress.Parse(this.ipAddress), SNetProp.getPort());
            this.clientAcceptorThread = new Thread(() =>
            {
                Console.WriteLine("server waiting");
                while (true)
                {
                    //accept a new client
                    Socket socket = tcpListener.AcceptSocket();
                    Console.WriteLine("got a connection");
                    //create clientProxy, which puts it into the model
                    BaseClientProxy client = new BaseClientProxy(socket, model);
                }
                Console.WriteLine("server done waiting");
            });
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
        

    }
}
