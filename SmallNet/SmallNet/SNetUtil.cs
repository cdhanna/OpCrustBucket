using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net;

using log4net;[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace SmallNet
{
    class SNetUtil
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Encode a message type with parameters.
        /// the final encored message will look like this
        /// MSGTYPE {ARG1} {ARG2} {ARGn}
        /// The {ARG}s will be toString'd
        /// If you put this on a stream, remember to write the variables to it.
        /// netWriter.WriteLine(msg, parameters);
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string encodeMessage(SMessage smessage)
        {
            //string msg = msgType + " ";
            //for (int i = 0; i < parameters.Length; i++)
            //{
            //    msg += "{" + i + "} ";
            //}
            log.Debug("ENCODING MESSAGE : " + smessage.ToString());
            string msg = Serializer.serialize(smessage);
            return msg;
        }

        /// <summary>
        /// Decode a message into a type and parameters
        /// you will get back a tuple, where the first item is the message type, and the second item is an array of 
        /// serialized paramters
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static SMessage decodeMessage(string message)
        {

            log.Debug("DECODING MESSAGE : " + message);
            SMessage smess = (SMessage)Serializer.deserialize(message);
            //string receivedMsg = message;
            //string[] msg = receivedMsg.Split(' ');
            //string[] param = new string[msg.Length - 1];
            //for (int i = 0; i < msg.Length - 1; i++)
            //{
            //    param[i] = msg[i + 1];
            //}
            
            //Tuple<string, string[]> retValue = new Tuple<string, string[]>(msg[0], param);
            return smess;
        }

        /// <summary>
        /// return the localIp address of the computer.
        /// taken from http://stackoverflow.com/questions/1069103/how-to-get-my-own-ip-address-in-c
        /// </summary>
        /// <returns></returns>
        public static string getLocalIp()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily== System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;

        }

        /// <summary>
        /// discover a list of all other IP addresses on the LAN
        /// </summary>
        public static List<string> discoverIps()
        {
            List<string> networkIPs = new List<string>();

            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "arp";
            p.StartInfo.Arguments = "-a";
            p.Start();

            string line = null;
            while (!p.StandardOutput.EndOfStream){
                line = p.StandardOutput.ReadLine();
                if (line.Contains("dynamic"))
                {
                    line = line.Substring(0, "123.123.123.123  ".Length);
                    line = line.Replace(" ", "");
                    networkIPs.Add(line);
                  //  Console.WriteLine(line);
                }
            }
            p.WaitForExit();
            return networkIPs;
        }

        public T createClientModel<T>(StreamWriter netWriter, string owner) where T : ClientModel 
        {
            //create a new client model
            T clientModel = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
            clientModel.create(netWriter, owner);
            return clientModel;
        }
    }
}
