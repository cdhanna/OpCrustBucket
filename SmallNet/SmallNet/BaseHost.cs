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
   
    /// <summary>
    /// The base host of the SmallNet project
    /// </summary>
    /// <typeparam name="T">The type of client model that will be used in the SmallNet project</typeparam>
    /// <typeparam name="H">The type of host model that will be used in the SmallNet project</typeparam>
    public class BaseHost<T, H> : Id where T:ClientModel where H :HostModel<T>
    {

        /// <summary>
        /// log object
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// .NET class that handles incoming tcp connections
        /// </summary>
        private TcpListener tcpListener;

        /// <summary>
        /// the ipaddress that the host is listening on
        /// </summary>
        private string ipAddress;

        /// <summary>
        /// thread responsible for accepting new clients
        /// </summary>
        private Thread clientAcceptorThread;

        /// <summary>
        /// true if the clientacceptor thread should be running
        /// </summary>
        private bool clientAcceptorThreadRunner;
        
        /// <summary>
        /// the host model
        /// </summary>
        private H hostModel;

        public Boolean Debug { get; set; }
        public string IpAddress { get { return this.ipAddress; } }

        public H Model { get { return this.hostModel; } }
        private bool isRunning;
        public bool IsRunning { get { return this.isRunning; } }

        public event EventHandler Connected;

        /// <summary>
        /// unique id. The host is always set to SNetProp.HOST_ID
        /// </summary>
        public int Id { get { return SNetProp.HOST_ID; } }

        /// <summary>
        /// Create a new host
        /// </summary>
        public BaseHost()
        {

            //create a new host model
            this.hostModel = (H)typeof(H).GetConstructor(new Type[] { }).Invoke(new object[] { });
            this.hostModel.init();

            //listen on all interfaces with 0.0.0.0
            this.tcpListener = new TcpListener(IPAddress.Parse("0.0.0.0"), SNetProp.getPort());
        }

        /// <summary>
        /// Start the host listening at the ip address.
        /// </summary>
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
                        
                        socket.NoDelay = true;
                        

                        //create clientProxy, which puts it into the model
                        BaseClientProxy<T> client = this.hostModel.generateNewClient(socket);
                        client.sendMessage(new Messages.CreateNewModelMessage(this, client.Id));
                        this.hostModel.addClient(client);
                        

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

        /// <summary>
        /// Shutdown the host. Clients will no longer be accepted. All clients will be disconnected from the client, and may experience turmoil and extreme unhappyness.
        /// </summary>
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

        /// <summary>
        /// Update the hostmodel
        /// </summary>
        /// <param name="time"></param>
        public void update(GameTime time)
        {
            this.hostModel.update(time);
        }
    }
}
