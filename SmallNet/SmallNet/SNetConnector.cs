using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;


namespace SmallNet
{
    class SNetConnector
    {

        public class MSGTYPE
        {
            private MSGTYPE() { }
            public const string NOFITY = "N";
            public const string CLOSE = "C";
        }


        // The port number for the remote device.
        private const int port = 5006;

        // The server address and ip
        private const string serverName = "node05.greggernaut.com";

        private IPHostEntry ipHostInfo;
        private IPAddress ipHost;
        private IPEndPoint remoteEP;
        
        
        private TcpClient client;
        private bool isConnected;

        private StreamReader netReader;
        private Thread readingThread;

        private StreamWriter netWriter;

        public event EventHandler<SNetConnectorMessageEventArgs> MessageRecieved;

        public SNetConnector()
        {



            ipHostInfo = Dns.Resolve(serverName);
            ipHost = ipHostInfo.AddressList[0];
            remoteEP = new IPEndPoint(ipHost, port);


            openConnection();
            MessageRecieved += (sender, args) =>
            {
                Console.WriteLine(args.msg);
            };


            sendNotificationMessage();
            sendNotificationMessage();



            Thread.Sleep(1500);

            closeConnection();
        }

        protected virtual void OnMessageRecieved(SNetConnectorMessageEventArgs e)
        {
            EventHandler<SNetConnectorMessageEventArgs> handler = MessageRecieved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void openConnection()
        {
            //connect
            client = new TcpClient();
            client.Connect(remoteEP);
            isConnected = true;

            NetworkStream stream = client.GetStream();
            netReader = new StreamReader(stream);
            netWriter = new StreamWriter(stream);

            readingThread = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        String msg = netReader.ReadLine();
                        SNetConnectorMessageEventArgs args = new SNetConnectorMessageEventArgs();
                        {
                            args.msg = msg;
                        }
                        OnMessageRecieved(args);
                    }
                }
                catch (ThreadAbortException abort)
                {
                    netReader.Close();
                    stream.Close();
                    Console.WriteLine("SNetConnector Reader Thread is Aborting");
                }
                catch (IOException io)
                {
                    netReader.Close();
                    stream.Close();
                    Console.WriteLine("SNetConnector Encounted an IO Exception");
                    closeConnection();
                }
            });
            readingThread.Start();

        }

        public void closeConnection()
        {
            //disconnect
            sendMessage(buildMessage(MSGTYPE.CLOSE));
            readingThread.Abort();
            netWriter.Close();
            client.Close();
            isConnected = false;
        }

        //send test message
        public void sendMessage(String msg)
        {
            if (isConnected)
            {
                //send message
                netWriter.WriteLine(msg);
                netWriter.Flush();
            }
        }


        public void sendNotificationMessage()
        {
            String msg = this.buildMessage(MSGTYPE.NOFITY, "chris");
            sendMessage(msg);
        }

        private string buildMessage(string msgType, params string[] info)
        {
            String msg = msgType;
            foreach (string i in info)
            {
                msg += " " + i;
            }
            return msg;
        }


        public class SNetConnectorMessageEventArgs : EventArgs
        {
            public String msg { get; set; }
        }


    }



}
