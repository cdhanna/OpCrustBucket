using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using log4net;
using Microsoft.Xna.Framework;

namespace SmallNet
{
   
    public class BaseHost<T> where T:ClientModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private TcpListener tcpListener;
        private string ipAddress;
        private Thread clientAcceptorThread;
        private bool clientAcceptorThreadRunner;
        
        private NetModel<T> model;
        public Boolean Debug { get; set; }
        public string IpAddress { get { return this.ipAddress; } }

        public NetModel<T> Model { get { return this.model; } }
        private bool isRunning;
        public bool IsRunning { get { return this.isRunning; } }

        public event EventHandler Connected;

        public BaseHost()
        {
            this.model = new NetModel<T>();
           
            this.ipAddress = SNetUtil.getLocalIp();
            this.tcpListener = new TcpListener(IPAddress.Parse(this.ipAddress), SNetProp.getPort());

        }

        public void start()
        {

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
                        BaseClientProxy<T> client = new BaseClientProxy<T>(socket, model);
                        client.sendMessage(new Messages.CreateNewModelMessage());
                        client.Debug = Debug;
                        log.Debug("got a connection");
                        this.model.addClient(client);
                    }
                }
                catch (Exception e)
                {
                    log.Debug("client acceptor thread has stopped... " + e.Message);
                }
            });

            this.clientAcceptorThreadRunner = true;
            this.tcpListener.Start();
            this.clientAcceptorThread.Name = "HOST:CLIENTACCEPTER";
            this.clientAcceptorThread.Start();
            isRunning = true;
            this.fireConnectedEvent();

        }

        private void fireConnectedEvent(){
            var handler = Connected;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }

        public void shutdown()
        {
            try
            {
                this.tcpListener.Stop();
                this.clientAcceptorThreadRunner = false;
                this.clientAcceptorThread.Abort();

                log.Debug("host shutdown");

                this.isRunning = false;
                this.fireConnectedEvent();
            }
            catch (Exception e)
            {
            }
            
        }

        public void sendMessageToAll(SMessage message)
        {
            this.model.sendMessageToAll(message);
        }

        public void update(GameTime time)
        {
            this.model.update(time);
        }
    }
}
