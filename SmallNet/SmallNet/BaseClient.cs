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
    public class BaseClient <T> : Id where T : ClientModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private StreamWriter netWriter;
        private StreamReader netReader;
        private TcpClient tcp;
        private bool connected;

        private Thread recieverThread;

        private T clientModel;
        private System.Timers.Timer connectTimer;

        public T ClientModel { get { return this.clientModel; } set { this.clientModel = value; } }
        public Boolean IsRunning { get { return this.connected; } }
        public Boolean Debug { get; set; }

        public event EventHandler Connected;
        public event EventHandler NewModel;

        int id;
        public int Id { get { return this.id; } }

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
        }

       

        /// <summary>
        /// attempt to connect a server at the ipaddress
        /// </summary>
        /// <param name="hostIpAddress"></param>
        /// <param name="credentials"></param>
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
                this.tcp = new TcpClient();
                this.tcp.Connect(hostIpAddress, SNetProp.getPort());
            }
            catch (SocketException e)
            {
                log.Debug("Client could not establish tcp");
                return;
            }


            this.connectTimer.Enabled = false;
            //initiate the data-flow streams
            Stream netStream = this.tcp.GetStream();
            this.netWriter = new StreamWriter(netStream);
            this.netReader = new StreamReader(netStream);

            this.netWriter.AutoFlush = true;

            this.sendMessage(new Messages.ConnectionMessage(this, credentials));

            if (this.recieverThread != null)
            {
                this.recieverThread.Abort();
            }
            this.recieverThread = new Thread(() =>
                {
                    try
                    {
                        while (true)
                        {
                            //decode incoming messages, and pass them to the recievedMessage()
                            string receivedMsg = netReader.ReadLine();
                            log.Debug("recieved msg- " + receivedMsg);
                           // Tuple<string, string[]> data = SNetUtil.decodeMessage(receivedMsg);
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


        public void disconnect()
        {
            if (!this.connected)
            {
                log.Debug("has already disonnected");
                return;
            }
            try
            {
                this.clientModel.destroy();
                
                this.sendMessage(new Messages.DisconnectionMessage(this));
                this.recieverThread.Abort();
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

        public void receieveMessage(SMessage message)
        {
            if (message is Messages.CreateNewModelMessage)
            {
                //create a new client model
                Messages.CreateNewModelMessage createMessage = (Messages.CreateNewModelMessage)message;
                this.clientModel = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
                this.clientModel.create(this.netWriter, "client");
                this.clientModel.setId(createMessage.Id);
                this.id = createMessage.Id;
                if (NewModel != null)
                {
                    NewModel(this, new EventArgs());
                }
                this.connected = true;

                this.fireConnected();
            }
            else
            {
                
                this.clientModel.onMessage(message); // no need to validate it here, because it has already been validated server side.
            }
        }

        public void sendMessage(SMessage message)
        {
            //construct a message
            if (!this.connected) // this is only true on the first case
            {
                String msg = SNetUtil.encodeMessage(message);
                this.netWriter.WriteLine(msg);
                log.Debug("sent connection message");
            }
            else
            {
                //log.Debug("send msg- " + msg);
                this.clientModel.sendMessage(message);
                log.Debug("sent regular message");
            }
        }
        public void shutdown()
        {
            this.disconnect();
            this.recieverThread.Abort();
            log.Debug("shutdown");
        }

        public void update(GameTime time)
        {
            
            if (this.clientModel != null)
            {
                this.clientModel.update(time);
            }
        }

    }
}
