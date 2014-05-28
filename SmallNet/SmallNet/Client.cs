using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet
{
    interface Client
    {
        /// <summary>
        /// Attempts to connect to the given ip address with the credentials
        /// </summary>
        /// <param name="ipAddress">The ip address to connect to</param>
        /// <param name="credentials">The credientals to send to the host, if they need any, to accept a connection</param>
        void connectTo(string ipAddress, string credentials);
        
        /// <summary>
        /// Disconnect from the current host, and inform the host of the disconnect.
        /// </summary>
        void disconnect();

        /// <summary>
        /// Shutdown the server for good.
        /// </summary>
        void shutdown();

        protected void receieveMessage(string msgType, params string[] paramterStrings);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgType"></param>
        /// <param name="parameters"></param>
        void sendMessage(string msgType, params object[] parameters);
    }
}
