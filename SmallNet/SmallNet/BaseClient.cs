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
    class BaseClient <T> : Client where T : ClientModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private StreamWriter netWriter;
        private StreamReader netReader;
        private TcpClient tcp;
        private bool connected;

        private Thread recieverThread;

        private T clientModel;
        private System.Timers.Timer connectTimer;


        public Boolean Debug { get; set; }

        public BaseClient()
        {
            // client initialization
            this.tcp = new TcpClient();
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
                this.tcp.Connect(hostIpAddress, SNetProp.getPort());
            }
            catch (SocketException e)
            {
                return;
            }


            this.connectTimer.Enabled = false;
            //initiate the data-flow streams
            Stream netStream = this.tcp.GetStream();
            this.netWriter = new StreamWriter(netStream);
            this.netReader = new StreamReader(netStream);

            this.netWriter.AutoFlush = true;

            this.sendMessage(SNetProp.CREDENTIALS, credentials);

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
                            Tuple<string, string[]> data = SNetUtil.decodeMessage(receivedMsg);
                            receieveMessage(data.Item1, data.Item2);
                        }
                    }
                    catch (Exception e)
                    {
                        log.Debug("client reciever has stopped");
                    }

                });
            this.recieverThread.Start();
            this.connected = true;

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
                
                this.sendMessage(SNetProp.DISCONNECT_NOTIFICATION);
                this.recieverThread.Abort();
                this.tcp.Close();
                this.connected = false;
                log.Debug("disconnect");
            }
            catch (Exception e)
            {
                log.Debug("disconnect did not finish");
            }
        }

        public void receieveMessage(string msgType, params string[] paramterStrings)
        {
            if (msgType.Equals(SNetProp.CREATE_NEW_CLIENT_MODEL))
            {
                //create a new client model
                this.clientModel = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
                this.clientModel.create(this.netWriter, "client");
            }
            else
            {
                this.clientModel.onMessage(msgType, paramterStrings); // no need to validate it here, because it has already been validated server side.
            }
        }

        public void sendMessage(string msgType, params object[] parameters)
        {
            //construct a message
            if (this.clientModel == null) // this is only true on the first case
            {
                String msg = SNetUtil.encodeMessage(msgType, parameters);
                this.netWriter.WriteLine(msg, parameters);
            }
            else
            {
                //log.Debug("send msg- " + msg);
                this.clientModel.sendMessage(msgType, parameters);

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
