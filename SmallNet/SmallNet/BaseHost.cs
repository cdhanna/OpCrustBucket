using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using log4net;


namespace SmallNet
{
   
    class BaseHost : Host
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private TcpListener tcpListener;
        private string ipAddress;
        private Thread clientAcceptorThread;
        private bool clientAcceptorThreadRunner;
        
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
                log.Debug("client acceptor thread is starting...");
                try
                {
                    while (clientAcceptorThreadRunner)
                    {
                        //accept a new client
                        Socket socket = tcpListener.AcceptSocket();
                        //create clientProxy, which puts it into the model
                        BaseClientProxy client = new BaseClientProxy(socket, model);
                        client.Debug = Debug;
                        log.Debug("got a connection");
                    }
                }
                catch (Exception e)
                {
                    log.Debug("client acceptor thread has crashed...");
                }
            });
        }

        public void start()
        {
            this.clientAcceptorThreadRunner = true;
            this.tcpListener.Start();
            this.clientAcceptorThread.Start();
        }

        public void shutdown()
        {
            this.tcpListener.Stop();
            this.clientAcceptorThreadRunner = false;
            this.clientAcceptorThread.Abort();
            log.Debug("host shutdown");
        }

        public void sendMessageToAll(string msgType, params object[] parameters)
        {
            this.model.sendMessageToAll(msgType, parameters);
        }
    }
}
