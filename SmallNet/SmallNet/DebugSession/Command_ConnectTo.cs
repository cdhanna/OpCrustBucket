using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    class Command_ConnectTo<T, H> : CommandOption<T, H> where T : ClientModel where H:HostModel<T>
    {

        public Command_ConnectTo()
            : base("Connect to")
        {
        }

        public override string runCommand(DebugSession<T, H> debug, string[] paramString)
        {
            if (paramString.Length != 2)
            {
                return "Must give an IP and a credential";
            }
            string ip = paramString[0];
            string cred = paramString[1];

            if (!SNetUtil.discoverIps().Contains(ip) && ip != SNetUtil.getLocalIp())
            {
                return "given IP address does not exist";
            }

            if (debug.Client == null)
            {
                debug.Client = new BaseClient<T>();
                debug.Client.connectTo(ip, cred);

                return "client created and connected";
            }
            else if (!debug.Client.IsRunning)
            {
                debug.Client.connectTo(ip, cred);
                return "client connected";
            }
            else
            {
                return "client already connected";
            }

        }

        public override string getParamDescription()
        {
            return "Give the ip address to connect to. In the form of 192.168.1.1";
        }
    }
}
