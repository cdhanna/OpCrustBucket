using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    public class Command_StartHost<T, H> : CommandOption<T, H> where T : ClientModel where H:HostModel<T>
    {
        public Command_StartHost()
            : base("Start Host")
        {
        }

        public override String runCommand(DebugSession<T, H> debug, String[] paramString)
        {
            if (debug.Host == null)
            {
                debug.Host = new BaseHost<T, H>();
                debug.Host.start();
                return "host created and started";
            }
            else
            {
                if (debug.Host.IsRunning)
                {
                    return "host is already running.";
                }
                else
                {
                    debug.Host.start();
                    return "host started";
                }
            }
        }

        public override string getParamDescription()
        {
            return null;
        }
    }
}
