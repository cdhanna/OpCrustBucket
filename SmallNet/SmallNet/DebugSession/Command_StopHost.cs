using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    class Command_StopHost<T> : CommandOption<T> where T:ClientModel
    {
        public Command_StopHost() : base("Stop Host")
        {
        }

        public override String runCommand(DebugSession<T> debug, String[] paramString)
        {
            if (debug.Host == null)
            {
                return "Host has not been created";
            }
            else if (debug.Host.IsRunning)
            {
                debug.Host.shutdown();
                return "Host shutdown";
            }
            else
            {
                return "Host already shutdown";
            }
       

        }

        public override string getParamDescription()
        {
            return null;
        }

    }
}
