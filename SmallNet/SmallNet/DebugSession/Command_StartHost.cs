using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    public class Command_StartHost<T> : CommandOption<T> where T : ClientModel
    {
        public Command_StartHost()
            : base("Start Host")
        {
        }

        public override String runCommand(DebugSession<T> debug, String[] paramString)
        {
            if (debug.Host == null)
            {
                debug.Host = new BaseHost<T>();
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
