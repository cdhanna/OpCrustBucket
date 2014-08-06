using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using log4net;
using Microsoft.Xna.Framework;

namespace SmallNet
{
    /// <summary>
    /// The BaseClientProxy runs on the host, as a way to simulate what is happening the client machine. 
    /// </summary>
    /// <typeparam name="T">What kind of clientmodel the network project is using</typeparam>
    public class BaseClientProxy<T> : Id where T: ClientModel
    {
        /// <summary>
        /// Log object
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <summary>
        /// The socket that is connected to the client
        /// </summary>
        private Socket socket;

        /// <summary>
        /// Data written to this stream came from the client
        /// </summary>
        private StreamReader netReader;

        /// <summary>
        /// Data written to this stream will go the client
        /// </summary>
        private StreamWriter netWriter;

        /// <summary>
        /// The host model
        /// </summary>
        private HostModel<T> model;

        /// <summary>
        /// Thread responsible for recieving messages off the netReader
        /// </summary>
        private Thread recieverThread;
        public Boolean Debug { get; set; }

        /// <summary>
        /// client model
        /// </summary>
        private T clientModel;

        /// <summary>
        /// unique id
        /// </summary>
        private int id;
        public int Id { get { return this.id; } }
       
        /// <summary>
        /// Creates a new client proxy.
        /// </summary>
        /// <param name="socket">The socket to use to create the net streams</param>
        /// <param name="model">the host model that this proxy is a part of</param>
        /// <param name="id">the unique id for the proxy.</param>
        public BaseClientProxy(Socket socket, HostModel<T> model, int id)
        {
            this.id = id;
            this.socket = socket;
            Stream socketStream = new NetworkStream(socket);
            
            this.netReader = new StreamReader(socketStream);
            this.netWriter = new StreamWriter(socketStream);

            SNetUtil.configureStreams(netReader, netWriter);

            this.model = model;
            
            this.clientModel = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
            this.clientModel.create(this.netWriter, NetworkSide.Host);
            clientModel.setId(id);
            this.startRecieverThread();
        }

        private void removeFromModel()
        {
            this.model.removeClient(this);
            log.Debug("client removed from net model");
        }

        public void kill()
        {
            Console.WriteLine("KILLING PROXY");
            loop = false;
        }

        public void playerJoined(Id id)
        {
         
            clientModel.playerJoined(id.Id);
            this.sendMessage(new Messages.PlayerJoined(id));
        }

        bool loop = true;
        private void startRecieverThread()
        {
            this.recieverThread = new Thread(() =>
            {
                try
                {
                   
                    while (loop) //listen forever
                    {
                        string netMessage = netReader.ReadLine();
                        log.Debug(this.clientModel.Owner + " recieved msg- " + netMessage);
                        SMessage smessage = SNetUtil.decodeMessage(netMessage);
                        if (smessage is Messages.DisconnectionMessage)
                        {
                            loop = false;
                            netReader.Close();
                            removeFromModel();
                            if (this.clientModel != null)
                            {
                                this.clientModel.destroy();
                            }
                        }
                        log.Debug(this.clientModel.Owner + " going to recieve method");
                        recieveMessage(smessage, netMessage);

                        Thread.Sleep(5);
                    }
                    log.Debug("client proxy reciever has stopped gracefully:: ");
                }
                catch (ThreadAbortException e)
                {
                    Console.WriteLine("client proxy reciever has kicked the bucket");
                }
                catch (Exception e)
                {
                    //throw e;
                    log.Debug("client proxy reciever has stopped:: " + e.Message);
                    //log.Debug("ERROR: " + e.);
                }
            });
            this.recieverThread.Name = "CLIENTPROXY:RECIEVER";
            this.recieverThread.Start();
        }

        protected void recieveMessage(SMessage message, String rawMessage)
        {

            if (this.clientModel.validateMessage(message))
            {
                this.clientModel.onMessage(message);
                this.model.sendMessageToAll(message, rawMessage);
            }
            
        }

        /// <summary>
        /// send a message to the client
        /// </summary>
        /// <param name="message"></param>
        public void sendMessage(SMessage message)
        {
            this.clientModel.sendMessage(message);
        }

        /// <summary>
        /// send a message to the client
        /// </summary>
        /// <param name="rawMessage">this string must be a well-formed SMessage serializable string</param>
        public void sendMessage(String rawMessage)
        {
            this.clientModel.sendMessage(rawMessage);
        }

        /// <summary>
        /// Updates the clientmodel in this proxy
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
