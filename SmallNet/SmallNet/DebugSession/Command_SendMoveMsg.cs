using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    class Command_SendMoveMsg<T, H> : CommandOption<T, H> where T : ClientModel where H:HostModel<T>
    {
        public Command_SendMoveMsg()
            : base("Sent Move Message")
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
                
                debug.Client.sendMessage(new Messages.MoveMessage(debug.Client, int.Parse(paramString[0]), int.Parse(paramString[1])));
                return "Sent message";
            }
        }

        public override string getParamDescription()
        {
            return "Give parameters in the following format; <X> <Y>";
        }
    }
}
