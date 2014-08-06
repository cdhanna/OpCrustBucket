using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Net.Sockets;


namespace SmallNet
{
    /// <summary>
    /// The default host model that will be used on new SmallNet projects. It is recommended that new SmallNet host models use this class for their base class. 
    /// WARNING: all implementations must have a no-arg constructor.
    /// </summary>
    /// <typeparam name="T">The type of client model being used with the SmallNet project</typeparam>
    abstract class DefaultHostModel<T> : HostModel<T> where T:ClientModel
    {
        /// <summary>
        /// A list of all the clients running on the host
        /// </summary>
        private List<BaseClientProxy<T>> clients;

        /// <summary>
        /// A map of all client IDs on the host, pointing to the client with the same ID
        /// </summary>
        private Dictionary<int, BaseClientProxy<T>> clientIdTable;
        
        /// <summary>
        /// the number of connected clients
        /// </summary>
        public int ClientProxyCount { get { return this.clients.Count; } }


        private int clientIdIncer = SNetProp.HOST_ID ;


        /// <summary>
        /// Create a new default host model
        /// </summary>
        public DefaultHostModel()
        {
            this.clients = new List<BaseClientProxy<T>>();
            this.clientIdTable = new Dictionary<int, BaseClientProxy<T>>();
        }

        /// <summary>
        /// get the client at the given index, or null if the index is out of range
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BaseClientProxy<T> getClientProxy(int index)
        {
            return index > -1 && index < ClientProxyCount ? clients[index] : null;
        }

        /// <summary>
        /// construct a new client
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public BaseClientProxy<T> generateNewClient(Socket socket)
        {
            clientIdIncer++;
            BaseClientProxy<T> client = new BaseClientProxy<T>(socket, this, clientIdIncer);
            return client;
        }

        /// <summary>
        /// add a client to the model
        /// </summary>
        /// <param name="client"></param>
        public void addClient(BaseClientProxy<T> client)
        {

            //tell the new client about all the existing clients
            foreach (BaseClientProxy<T> proxy in this.clients)
            {
                client.playerJoined(proxy);
            }


            this.clients.Add(client);
            this.clientIdTable[clientIdIncer] = client;

            //tell the clients about the new client
            foreach (BaseClientProxy<T> proxy in this.clients)
            {
           
                proxy.playerJoined(client);
            }
        }

        /// <summary>
        /// remove a client from the model
        /// </summary>
        /// <param name="client"></param>
        public void removeClient(BaseClientProxy<T> client)
        {
            this.clients.Remove(client);
            //TODO remove map entry
        }

        /// <summary>
        /// broadcast a message to all connected clients
        /// </summary>
        /// <param name="message"></param>
        /// <param name="rawMessage">Should be the serialized form of the given SMessage</param>
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
        
        /// <summary>
        /// broadcast a message to all connected clients
        /// </summary>
        /// <param name="message"></param>
        public void sendMessageToAll(SMessage message)
        {
            String rawMessage = SNetUtil.encodeMessage(message);
            this.sendMessageToAll(message, rawMessage);
        }

        /// <summary>
        /// updates the client proxies, and THEN the host.
        /// </summary>
        /// <param name="time"></param>
        public void update(GameTime time)
        {
            foreach (BaseClientProxy<T> proxy in this.clients)
            {
                proxy.update(time);
            }
            this.updateHost(time);
        }

        /// <summary>
        /// called when the host is shutting down.
        /// </summary>
        public void onShutdown()
        {
            //send a message to all the clients, letting them know the end has come.
            SMessage msg = new Messages.DisconnectionMessage();
            foreach (BaseClientProxy<T> proxy in this.clients)
            {
                proxy.sendMessage(msg);
            }
        }

        /// <summary>
        /// new implementation must implement this. This is called when the host model should be updated
        /// </summary>
        /// <param name="time"></param>
        public abstract void updateHost(GameTime time);

        /// <summary>
        /// new implementation must define this. This is called when the model is first created.
        /// </summary>
        public abstract void init();

        /// <summary>
        /// new implementation must define this. This is called when the host is shutting down
        /// </summary>
        public abstract void destroy();

        /// <summary>
        /// validate a message.
        /// </summary>
        /// <param name="message"></param>
        /// <returns>true if the message should be allowed to propegate through the network</returns>
        public abstract bool validateMessage(SMessage message);

        /// <summary>
        /// new implementation must define this. This is called when a message is recieved
        /// </summary>
        /// <param name="message"></param>
        public abstract void onMessage(SMessage message);


    }
}
