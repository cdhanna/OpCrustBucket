using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    class Command_GetAllIP<T> : CommandOption<T> where T : ClientModel
    {
        public Command_GetAllIP()
            : base("Get LAN IPs")
        {
        }

        public override string runCommand(DebugSession<T> debug, String[] paramString)
        {
            List<String> ips = SNetUtil.discoverIps();
            String ipString = "LAN IPs: " + Environment.NewLine + "\t";

            foreach (String ip in ips){
                ipString += ip + Environment.NewLine + "\t";
            }

            return ipString;
        }

        public override string getParamDescription()
        {
            return null;
        }

    }
}
