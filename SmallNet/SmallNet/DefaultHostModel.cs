using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Net.Sockets;


namespace SmallNet
{
    abstract class DefaultHostModel<T> : HostModel<T> where T:ClientModel
    {
        private List<BaseClientProxy<T>> clients;
        private Dictionary<int, BaseClientProxy<T>> clientIdTable;
        public int ClientProxyCount { get { return this.clients.Count; } }

        int clientIdIncer = SNetProp.HOST_ID + 1;


        public BaseClientProxy<T> getClientProxy(int index)
        {
            return index > -1 && index < ClientProxyCount ? clients[index] : null;
        }

        public DefaultHostModel()
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

        public void sendMessageToAll(SMessage message, String rawMessage)
        {
            if (!this.validateMessage(message))
                return;

            //TODO add flag to only send to host
            this.onMessage(message);

            foreach (BaseClientProxy<T> proxy in this.clients)
            {
                //proxy.sendMessage(message);
                proxy.sendMessage(rawMessage);
            }
        }
        public void sendMessageToAll(SMessage message)
        {
            String rawMessage = SNetUtil.encodeMessage(message);
            this.sendMessageToAll(message, rawMessage);
        }

        public void update(GameTime time)
        {
            foreach (BaseClientProxy<T> proxy in this.clients)
            {
                proxy.update(time);
            }
            this.updateHost(time);
        }

        public void onShutdown()
        {
            SMessage msg = new Messages.DisconnectionMessage();
            foreach (BaseClientProxy<T> proxy in this.clients)
            {
                proxy.sendMessage(msg);
            }
        }

        public abstract void updateHost(GameTime time);
        public abstract void init();

        public abstract void destroy();

        public abstract bool validateMessage(SMessage message);

        public abstract void onMessage(SMessage message);

       
        //public void sendMessage(SMessage message)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
