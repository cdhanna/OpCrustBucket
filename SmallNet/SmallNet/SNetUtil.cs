﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
namespace SmallNet
{
    class SNetUtil
    {
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
        public static string encodeMessage(string msgType, params object[] parameters)
        {
            string msg = msgType + " ";
            for (int i = 0; i < parameters.Length; i++)
            {
                msg += "{" + i + "} ";
            }
            return msg;
        }

        /// <summary>
        /// Decode a message into a type and parameters
        /// you will get back a tuple, where the first item is the message type, and the second item is an array of 
        /// serialized paramters
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Tuple<string, string[]> decodeMessage(string message)
        {
            string receivedMsg = message;
            string[] msg = receivedMsg.Split(' ');
            string[] param = new string[msg.Length - 1];
            for (int i = 0; i < msg.Length - 1; i++)
            {
                param[i] = msg[i + 1];
            }
            
            Tuple<string, string[]> retValue = new Tuple<string, string[]>(msg[0], param);
            return retValue;
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

    }
}