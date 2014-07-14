using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    class Command_GetAllIP<T, H> : CommandOption<T, H> where T : ClientModel where H:HostModel<T>
    {
        public Command_GetAllIP()
            : base("Get LAN IPs")
        {
        }

        public override string runCommand(DebugSession<T, H> debug, String[] paramString)
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
