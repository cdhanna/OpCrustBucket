using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    class Command_SendMsg<T, H> : CommandOption<T, H> where T : ClientModel where H:HostModel<T>
    {
        public Command_SendMsg()
            : base("Send Message")
        {
        }


        public override string runCommand(DebugSession<T, H> debug, string[] paramString)
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
                string msg = "";
                foreach (string m in paramString)
                    msg += m + " ";
                debug.Client.sendMessage(new Messages.StringMessage(debug.Client, msg));
                return "Sent message";
            }
        }

        public override string getParamDescription()
        {
            return "give parameters in the following format. <msgType> <arg1> <arg2> <arg...> <argn>";
        }
    }
}
