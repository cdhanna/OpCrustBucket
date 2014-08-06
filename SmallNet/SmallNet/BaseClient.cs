using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using log4net;
using Microsoft.Xna.Framework;


namespace SmallNet
{
    /// <summary>
    /// The base client of SmallNet.
    /// 
    /// </summary>
    /// <typeparam name="T"> The kind of client model that the base client will be using. </typeparam>
    public class BaseClient <T> : Id where T : ClientModel
    {

        /// <summary>
        /// log object. 
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// Stream to write data onto. Data written to this stream will travel to the host.
        /// </summary>
        private StreamWriter netWriter;
        
        /// <summary>
        /// Steam to read host data from. Data read from this stream came from the host.
        /// </summary>
        private StreamReader netReader;

        /// <summary>
        /// .NET core class to handle TCP connections
        /// </summary>
        private TcpClient tcp;
       
        /// <summary>
        /// true if the client is connected to the host
        /// </summary>
        private bool connected;

        /// <summary>
        /// Thread responsible for recieving messages from the netReader stream
        /// </summary>
        private Thread recieverThread;

        /// <summary>
        /// The clientmodel for the client.
        /// </summary>
        private T clientModel;

        /// <summary>
        /// DEPRECATED
        /// </summary>
        private System.Timers.Timer connectTimer;

        /// <summary>
        /// The unique ID for the client. 
        /// </summary>
        int id;

        public T ClientModel { get { return this.clientModel; } set { this.clientModel = value; } }
        public Boolean IsRunning { get { return this.connected; } }
        public Boolean Debug { get; set; }

        public event EventHandler Connected;
        public event EventHandler NewModel;

        
        public int Id { get { return this.id; } }

        /// <summary>
        /// Creates a new BaseClient.
        /// 
        /// </summary>
        public BaseClient()
        {
            // client initialization
            this.connected = false;
            this.connectTimer = new System.Timers.Timer(2000);
            this.connectTimer.Elapsed += new System.Timers.ElapsedEventHandler(ConnectTimerTimeOut);
        }



        private void ConnectTimerTimeOut(object source, System.Timers.ElapsedEventArgs e)
        {
            log.Debug(" connection timeout");
            this.connectTimer.Enabled = false;
            this.tcp.Close();
            //sock.Close();
        }

       

        /// <summary>
        /// attempt to connect a server at the ipaddress. 
        /// </summary>
        /// <param name="hostIpAddress">the ip address or host name of the host</param>
        /// <param name="credentials">the credential information to connect with</param>
        public void connectTo(String hostIpAddress, String credentials)
        {
            if (this.connected)
            {
                //Error. Cannot connect, because client already has a connection
                log.Debug("error! cannot connect because client already has a connection.");
                return;
            }

            this.connectTimer.Enabled = true;

            //initiate the connection
            try
            {
                //construct the .net tcp client
                this.tcp = new TcpClient();
               
                //connect the client
                tcp = new TcpClient(hostIpAddress, SNetProp.getPort());
                tcp.Client.NoDelay = true;

            }
            catch (SocketException e)
            {
                log.Debug("Client could not establish tcp :" + e.Message);
                return;
            }


            this.connectTimer.Enabled = false;


            //initiate the data-flow streams
            Stream netStream = this.tcp.GetStream();
            this.netWriter = new StreamWriter(netStream);
            this.netReader = new StreamReader(netStream);

            //configure the streams
            SNetUtil.configureStreams(netReader, netWriter);

            //send a message to the host with the credentials
            this.sendMessage(new Messages.ConnectionMessage(this, credentials));

            //start the reciever thread
            //if the reciever thread is already running, stop it.
            if (this.recieverThread != null)
            {
                this.recieverThread.Abort();
            }
            //and start a new one
            this.recieverThread = new Thread(() =>
                {
                    try
                    {
                        while (true)
                        {
                            //decode incoming messages, and pass them to the recievedMessage()
                            string receivedMsg = netReader.ReadLine();
                            log.Debug("recieved msg- " + receivedMsg);
                           
                            //pass the message along, and then sleep for a bit
                            SMessage message = SNetUtil.decodeMessage(receivedMsg);
                            receieveMessage(message);
                            Thread.Sleep(5);
                        }
                    }
                    catch (ThreadAbortException e)
                    {
                        log.Debug("client reciever has stopped");
                        this.tcp.Close();
                    }

                });
            this.recieverThread.Name = "BASECLIENT:RECIEVER";
            this.recieverThread.Start();
            

        }

        /// <summary>
        /// disconnect the client from a host. If the client is not connected, this will not do anything useful
        /// </summary>
        public void disconnect()
        {
            disconnect(true);
        }

        /// <summary>
        /// disconnect the client from a host. If the client is not connected, this will not do anything useful
        /// </summary>
        /// <param name="notify">if true, this will send a disconnection message to the host</param>
        public void disconnect(bool notify)
        {
            if (!this.connected)
            {
                log.Debug("has already disonnected");
                return;
            }
            try
            {
                this.clientModel.destroy();
                
                if (notify)
                {
                    this.sendMessage(new Messages.DisconnectionMessage(this));
                }
                this.recieverThread.Abort();
                this.netReader.Close();
                this.connected = false;
                
                log.Debug("disconnect");
            }
            catch (ThreadAbortException e)
            {
                log.Debug("disconnect did not finish");
            }
            
            this.fireConnected();
        }

        private void fireConnected()
        {
            var handler = Connected;
            if (handler != null)
            {
                handler(this, new EventArgs());
            }
        }
        private void fireMessageRecieved(String msg, String[] p)
        {
            //var handler = receieveMessage;
            //if (handler != null)
            {
               // receieveMessage(this, new MessageEventArgs(msg, p));
            }
        }

        /// <summary>
        /// called when a message is recieved. 
        /// </summary>
        /// <param name="message"></param>
        public void receieveMessage(SMessage message)
        {
            if (message is Messages.CreateNewModelMessage)
            {
                //create a new client model
                Messages.CreateNewModelMessage createMessage = (Messages.CreateNewModelMessage)message;
                this.clientModel = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
                this.clientModel.create(this.netWriter, NetworkSide.Client);
                this.clientModel.setId(createMessage.Id);
                this.id = createMessage.Id;
                if (NewModel != null)
                {
                    NewModel(this, new EventArgs());
                }
                this.connected = true;

                this.fireConnected();
            }
            else if (message is Messages.DisconnectionMessage)
            {
                this.disconnect(false);
            }
            else if (message is Messages.PlayerJoined)
            {
                this.ClientModel.playerJoined(message.SenderId);
            }
            else
            {

                this.clientModel.onMessage(message); // no need to validate it here, because it has already been validated server side.
            }
        }

        /// <summary>
        /// Send a message to the host
        /// </summary>
        /// <param name="message"></param>
        public void sendMessage(SMessage message)
        {
            //construct a message
            if (!this.connected) // this is only true on the first case
            {
                String msg = SNetUtil.encodeMessage(message);
                this.netWriter.WriteLine(msg);
                this.netWriter.Flush(); //remember to flush, sir.
                log.Debug("sent connection message");
            }
            else
            {
                this.clientModel.sendMessage(message);
                log.Debug("sent regular message");
            }
        }

        /// <summary>
        /// shutdown the client. Similar to disconnect(). Actually, exactly the same. Marked for deletion.
        /// </summary>
        public void shutdown()
        {
            this.disconnect();
            log.Debug("shutdown");
        }

        /// <summary>
        /// Update the client model
        /// </summary>
        /// <param name="time"></param>
        public void update(GameTime time)
        {
            if (this.clientModel != null)
            {
                this.clientModel.update(time);
            }
        }

    }
}
