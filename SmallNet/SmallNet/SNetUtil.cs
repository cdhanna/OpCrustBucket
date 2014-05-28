using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace SmallNet
{
    class SNetUtil
    {
        public static string encodeMessage(string msgType, params object[] parameters)
        {
            string msg = msgType + " ";
            for (int i = 0; i < parameters.Length; i++)
            {
                msg += "{" + i + "} ";
            }
            return msg;
        }

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

    }
}
