using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace SmallNet
{
    class BaseClient : Client
    {
        private StreamWriter netWriter;
        private StreamReader netReader;
        private TcpClient tcp;
        private bool connected;

        private Thread recieverThread;

        public BaseClient()
        {
            // client initialization
            this.tcp = new TcpClient();
            this.connected = false;
        }

        
        public void connectTo(String hostIpAddress, String credentials)
        {
            if (this.connected)
            {
                //Error. Cannot connect, because client already has a connection
                return;
            }

            //initiate the connection
            this.tcp.Connect(hostIpAddress, SNetProp.getPort());
            //initiate the data-flow streams
            Stream netStream = this.tcp.GetStream();
            this.netWriter = new StreamWriter(netStream);
            this.netReader = new StreamReader(netStream);

            this.netWriter.AutoFlush = true;

            this.sendMessage(SNetProp.CLIENT_CREDENTIALS, credentials);

            if (this.recieverThread != null)
            {
                this.recieverThread.Abort();
            }
            this.recieverThread = new Thread(() =>
                {
                    //decode incoming messages, and pass them to the recievedMessage()
                    string receivedMsg = netReader.ReadLine();

                    Tuple<string, string[]> data = SNetUtil.decodeMessage(receivedMsg);
                    receieveMessage(data.Item1, data.Item2);
                });
            this.recieverThread.Start();
            this.connected = true;

        }


        public void disconnect()
        {
            this.sendMessage(SNetProp.CLIENT_DISCONNECT_NOTIFICATION);
            this.tcp.Close();
            this.connected = false;
        }

        public void receieveMessage(string msgType, params string[] paramterStrings)
        {
            //do something with messages from server
            throw new NotImplementedException();
        }

        public void sendMessage(string msgType, params object[] parameters)
        {
            //construct a message
            string msg = SNetUtil.encodeMessage(msgType, parameters);

            //send the message
            this.netWriter.WriteLine(msg, parameters);

        }

        public void shutdown()
        {
            this.disconnect();
            this.recieverThread.Abort();
        }



    }
}
