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
    class BaseClientProxy<T> where T: ClientModel 
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        private Socket socket;
        private StreamReader netReader;
        private StreamWriter netWriter;
        private NetModel<T> model;
        private Thread recieverThread;
        public Boolean Debug { get; set; }

        private T clientModel;

        public BaseClientProxy(Socket socket, NetModel<T> model)
        {
            this.socket = socket;
            Stream socketStream = new NetworkStream(socket);
            this.netReader = new StreamReader(socketStream);
            this.netWriter = new StreamWriter(socketStream);

            this.netWriter.AutoFlush = true;
            this.model = model;

            this.clientModel = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
            this.clientModel.init("host");

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
                        log.Debug("recieved msg- " + netMessage);
                        Tuple<string, string[]> data = SNetUtil.decodeMessage(netMessage);

                        if (data.Item1.Equals(SNetProp.DISCONNECT_NOTIFICATION))
                        {
                            loop = false;
                            removeFromModel();
                            if (this.clientModel != null)
                            {
                                this.clientModel.destroy();
                                
                            }
                        }

                        recieveMessage(data.Item1, data.Item2);
                    }
                }
                catch (Exception e)
                {
                    log.Debug("client proxy reciever has stopped");
                }
            });
            this.recieverThread.Start();
        }

        protected void recieveMessage(string msgType , params string[] parameters)
        {
            //TODO validate message
            //TODO apply message
            //TODO broadcast message

            //this.sendMessage(msgType, parameters);
        }

        public void sendMessage(string msgType, params object[] parameters)
        {
            string msg = SNetUtil.encodeMessage(msgType, parameters);
            this.netWriter.WriteLine(msg, parameters);
            log.Debug("send msg- " + msg);
            
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
