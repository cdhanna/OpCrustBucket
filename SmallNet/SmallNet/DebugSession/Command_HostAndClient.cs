using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    class Command_HostAndClient<T, H> : CommandOption<T, H> where T:ClientModel where H:HostModel<T>
    {
        public Command_HostAndClient() : base("Start Host and Connect") { }


        public override string runCommand(DebugSession<T, H> debug, string[] paramString)
        {
            Command_StartHost<T, H> startHostCommand = new Command_StartHost<T, H>();
            debug.runCommand(startHostCommand, new string[] { });

            Command_ConnectTo<T, H> connectCommand = new Command_ConnectTo<T, H>();
            debug.runCommand(connectCommand, new string[] { SNetUtil.getLocalIp(), paramString[0]});

            return "success";
        }

        public override string getParamDescription()
        {
            return "Enter your credential";
        }
    }
}
