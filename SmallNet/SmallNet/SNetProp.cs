using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet
{
    class SNetProp
    {
        public const string CLIENT_DISCONNECT_NOTIFICATION = "c_h_disc";
        public const string CLIENT_CREDENTIALS = "c_h_cred";

        public static int getPort()
        {
            return 9004;
        }

    }
}
