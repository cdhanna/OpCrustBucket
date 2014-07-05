using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    class Command_HostAndClient<T> : CommandOption<T> where T:ClientModel
    {
        public Command_HostAndClient() : base("Start Host and Connect") { }


        public override string runCommand(DebugSession<T> debug, string[] paramString)
        {
            Command_StartHost<T> startHostCommand = new Command_StartHost<T>();
            debug.runCommand(startHostCommand, new string[] { });

            Command_ConnectTo<T> connectCommand = new Command_ConnectTo<T>();
            debug.runCommand(connectCommand, new string[] { SNetUtil.getLocalIp(), paramString[0]});

            return "success";
        }

        public override string getParamDescription()
        {
            return "Enter your credential";
        }
    }
}
