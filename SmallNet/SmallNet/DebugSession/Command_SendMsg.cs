using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    class Command_SendMsg<T> : CommandOption<T> where T : ClientModel
    {
        public Command_SendMsg()
            : base("Send Message")
        {
        }


        public override string runCommand(DebugSession<T> debug, string[] paramString)
        {
            if (debug.Client == null)
            {
                return "Client does not exist";
            }
            else if (!debug.Client.IsRunning)
            {
                return "Client is not connected";
            }
            else
            {
                if (paramString.Length == 0){
                    return "Cannot send empty message";
                }
                string[] args = new string[paramString.Length-1];
                for (int i = 0 ; i < args.Length ; i ++){
                    args[i] = paramString[i+1];
                }
                debug.Client.sendMessage("abc", "1");
                return "Sent message";
            }
        }

        public override string getParamDescription()
        {
            return "give parameters in the following format. <msgType> <arg1> <arg2> <arg...> <argn>";
        }
    }
}
