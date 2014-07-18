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
    public class BaseClientProxy<T> : Id where T: ClientModel
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private Socket socket;
        private StreamReader netReader;
        private StreamWriter netWriter;
        private HostModel<T> model;
        private Thread recieverThread;
        public Boolean Debug { get; set; }

        private T clientModel;
        private int id;
        public int Id { get { return this.id; } }

        public BaseClientProxy(Socket socket, HostModel<T> model, int id)
        {
            this.id = id;
            this.socket = socket;
            Stream socketStream = new NetworkStream(socket);
            
            this.netReader = new StreamReader(socketStream);
            this.netWriter = new StreamWriter(socketStream);

            this.netWriter.AutoFlush = true;
            this.model = model;

            this.clientModel = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
            this.clientModel.create(this.netWriter, "host");

            this.startRecieverThread();
        }

        private void removeFromModel()
        {
            this.model.removeClient(this);
            log.Debug("client removed from net model");
        }
        private void startRecieverThread()
        {
            this.recieverThread = new Thread(() =>
            {
                try
                {
                    bool loop = true;
                    while (loop) //listen forever
                    {
                        string netMessage = netReader.ReadLine();
                        log.Debug(this.clientModel.Owner + " recieved msg- " + netMessage);
                        //Tuple<string, string[]> data = SNetUtil.decodeMessage(netMessage);
                        SMessage smessage = SNetUtil.decodeMessage(netMessage);
                        //if (data.Item1.Equals(SNetProp.DISCONNECT_NOTIFICATION))
                        if (smessage is Messages.DisconnectionMessage)
                        {
                            loop = false;
                            removeFromModel();
                            if (this.clientModel != null)
                            {
                                this.clientModel.destroy();
                                
                            }
                        }
                        log.Debug(this.clientModel.Owner + " going to recieve method");
                        recieveMessage(smessage);

                        Thread.Sleep(5);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                    //log.Debug("client proxy reciever has stopped:: " + e.StackTrace);
                    //log.Debug("ERROR: " + e.);
                }
            });
            this.recieverThread.Name = "CLIENTPROXY:RECIEVER";
            this.recieverThread.Start();
        }

        protected void recieveMessage(SMessage message)
        {

            if (this.clientModel.validateMessage(message))
            {
                this.clientModel.onMessage(message);
                this.model.sendMessageToAll(message);
            }
            

        }

        public void sendMessage(SMessage message)
        {
            //string msg = SNetUtil.encodeMessage(msgType, parameters);
            //this.netWriter.WriteLine(msg, parameters);
            //log.Debug("send msg- " + msg);
            this.clientModel.sendMessage(message);
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
