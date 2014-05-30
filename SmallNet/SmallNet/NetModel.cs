using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet
{
    class NetModel
    {
        private List<BaseClientProxy> clients;

        public int ClientProxyCount { get { return this.clients.Count; } }
        public BaseClientProxy getClientProxy(int index)
        {
            return index > -1 && index < ClientProxyCount ? clients[index] : null;
        }

        public NetModel()
        {
            this.clients = new List<BaseClientProxy>();
        }

        public void addClient(BaseClientProxy client)
        {
            this.clients.Add(client);
        }
        public void removeClient(BaseClientProxy client)
        {
            this.clients.Remove(client);
        }

        public void sendMessageToAll(string msgType, params object[] parameters)
        {
            foreach (BaseClientProxy proxy in this.clients)
            {
                proxy.sendMessage(msgType, parameters);
            }
        }
    }
}
