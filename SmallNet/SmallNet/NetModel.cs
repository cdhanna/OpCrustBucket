using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SmallNet
{
    class NetModel<T> where T: ClientModel
    {
        private List<BaseClientProxy<T>> clients;

        public int ClientProxyCount { get { return this.clients.Count; } }
        public BaseClientProxy<T> getClientProxy(int index)
        {
            return index > -1 && index < ClientProxyCount ? clients[index] : null;
        }

        public NetModel()
        {
            this.clients = new List<BaseClientProxy<T>>();
        }

        public void addClient(BaseClientProxy<T> client)
        {
            this.clients.Add(client);
        }
        public void removeClient(BaseClientProxy<T> client)
        {
            this.clients.Remove(client);
        }

        public void sendMessageToAll(string msgType, params object[] parameters)
        {
            foreach (BaseClientProxy<T> proxy in this.clients)
            {
                proxy.sendMessage(msgType, parameters);
            }
        }

        public void update(GameTime time)
        {
            foreach (BaseClientProxy<T> proxy in this.clients)
            {
                proxy.update(time);
            }
        }
    }
}
