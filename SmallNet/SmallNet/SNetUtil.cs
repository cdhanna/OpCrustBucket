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

    /// <summary>
    /// A collection of SmallNet utility functions
    /// </summary>
    class SNetUtil
    {
        /// <summary>
        /// log object
        /// </summary>
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// configure streams to best performance. 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="writer"></param>
        public static void configureStreams(StreamReader reader, StreamWriter writer)
        {
           // writer.AutoFlush = true;
            //do something with streams? maybe
        }

        /// <summary>
        /// Encode a message type with parameters.
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string encodeMessage(SMessage smessage)
        {
            log.Debug("ENCODING MESSAGE : " + smessage.ToString());
            string msg = Serializer.serialize(smessage);
            return msg;
        }

        /// <summary>
        /// Decode a message into a type and parameters
        /// serialized paramters
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static SMessage decodeMessage(string message)
        {

            log.Debug("DECODING MESSAGE : " + message);
            SMessage smess = (SMessage)Serializer.deserialize(message);
           
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

        /// <summary>
        /// Get the current system time since the epoch. 
        /// NOTE: this time will not be consistent accross machines.
        /// </summary>
        /// <returns></returns>
        public static long getCurrentTime()
        {
           return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

    }
}
