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
   
    public class BaseHost<T, H> : Id where T:ClientModel where H :HostModel<T>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        private TcpListener tcpListener;
        private string ipAddress;
        private Thread clientAcceptorThread;
        private bool clientAcceptorThreadRunner;
        
        //private NetModel<T> model;

        private H hostModel;

        public Boolean Debug { get; set; }
        public string IpAddress { get { return this.ipAddress; } }

        public H Model { get { return this.hostModel; } }
        private bool isRunning;
        public bool IsRunning { get { return this.isRunning; } }

        public event EventHandler Connected;

        public int Id { get { return SNetProp.HOST_ID; } }

        public BaseHost()
        {
            //this.model = new NetModel<T>();
            this.hostModel = (H)typeof(H).GetConstructor(new Type[] { }).Invoke(new object[] { });
            this.hostModel.init();
            //this.clientModel.create(this.netWriter, "host");



            this.tcpListener = new TcpListener(IPAddress.Parse("0.0.0.0"), SNetProp.getPort());
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
                        Console.WriteLine("waiting for connection form a client");

                        //accept a new client
                        Socket socket = tcpListener.AcceptSocket();
                        //create clientProxy, which puts it into the model

                        BaseClientProxy<T> client = this.hostModel.generateNewClient(socket);
                        this.hostModel.addClient(client);
                        client.sendMessage(new Messages.CreateNewModelMessage(this, client.Id));
                        client.Debug = Debug;
                        log.Debug("got a connection");

                    }
                }
                catch (ThreadAbortException e)
                {
                    log.Debug("Client Acceptor thread aborted");
                }
                catch (Exception e)
                {
                    log.Debug("client acceptor thread has stopped... " + e.Message);
                }
            });
            this.clientAcceptorThread.Start();
            this.clientAcceptorThreadRunner = true;
            this.tcpListener.Start();
            this.clientAcceptorThread.Name = "HOST:CLIENTACCEPTER";
            
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

                this.hostModel.onShutdown();
                this.tcpListener.Stop();
                this.clientAcceptorThreadRunner = false;
                this.clientAcceptorThread.Abort();
                
                log.Debug("host shutdown");

                this.isRunning = false;
                this.fireConnectedEvent();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message + Environment.NewLine + e.StackTrace);

            }
            
        }

        public void sendMessageToAll(SMessage message)
        {
            this.hostModel.sendMessageToAll(message);
        }

        public void update(GameTime time)
        {
            this.hostModel.update(time);
        }
    }
}
