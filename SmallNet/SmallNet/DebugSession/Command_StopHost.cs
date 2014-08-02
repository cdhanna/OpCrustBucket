using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    class Command_StopHost<T, H> : CommandOption<T, H> where T:ClientModel where H:HostModel<T>
    {
        public Command_StopHost() : base("Stop Host")
        {
        }

        public override String runCommand(DebugSession<T, H> debug, String[] paramString)
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
