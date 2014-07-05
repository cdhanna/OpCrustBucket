using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    class Command_Disconnect<T> : CommandOption<T> where T : ClientModel
    {
        public Command_Disconnect()
            : base("Disconnect")
        {
        }

        public override string runCommand(DebugSession<T> debug, string[] paramString)
        {
            if (debug.Client == null)
            {
                return "Client does not exist";
            }
            else if (debug.Client.IsRunning)
            {
                debug.Client.disconnect();
                return "Client disconnected";
            }
            else
            {
                return "Client already disconnected";
            }
        }

        public override string getParamDescription()
        {
            return null;
        }
    }
}
