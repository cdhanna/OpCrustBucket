using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Net.Sockets;
namespace SmallNet
{
    public class NetModel<T> where T: ClientModel
    {
        private List<BaseClientProxy<T>> clients;
        private Dictionary<int, BaseClientProxy<T>> clientIdTable;
        public int ClientProxyCount { get { return this.clients.Count; } }

        int clientIdIncer = SNetProp.HOST_ID + 1;

        public BaseClientProxy<T> getClientProxy(int index)
        {
            return index > -1 && index < ClientProxyCount ? clients[index] : null;
        }

        public NetModel()
        {
            this.clients = new List<BaseClientProxy<T>>();
            this.clientIdTable = new Dictionary<int, BaseClientProxy<T>>();
        }

        public BaseClientProxy<T> generateNewClient(Socket socket)
        {
            BaseClientProxy<T> client = new BaseClientProxy<T>(socket, this, clientIdIncer);
            this.clientIdTable[clientIdIncer] = client;
            clientIdIncer++;
            return client;
        }


        public void addClient(BaseClientProxy<T> client)
        {
            this.clients.Add(client);

        }
        public void removeClient(BaseClientProxy<T> client)
        {
            this.clients.Remove(client);
        }

        public void sendMessageToAll(SMessage message)
        {
            foreach (BaseClientProxy<T> proxy in this.clients)
            {
                proxy.sendMessage(message);
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
