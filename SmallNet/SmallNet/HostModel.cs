using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Net.Sockets;
namespace SmallNet
{

    /// <summary>
    /// The host model is run on the host machine. It keeps track of all connected clients.
    /// </summary>
    /// <typeparam name="T">The type of client model that will be running on the SmallNet project</typeparam>
    public interface HostModel<T> where T:ClientModel
    {
        /// <summary>
        /// get the client proxy at the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        BaseClientProxy<T> getClientProxy(int index);

        /// <summary>
        /// construct a new client proxy from this model. The new proxy will not be added to the host model
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        BaseClientProxy<T> generateNewClient(Socket socket);

        /// <summary>
        /// add a baseclient proxy to the model
        /// </summary>
        /// <param name="client"></param>
        void addClient(BaseClientProxy<T> client);

        /// <summary>
        /// remove a client from the model
        /// </summary>
        /// <param name="client"></param>
        void removeClient(BaseClientProxy<T> client);

        /// <summary>
        /// sends a message to all connected clients
        /// </summary>
        /// <param name="message"></param>
        /// <param name="rawMessage"></param>
        void sendMessageToAll(SMessage message, String rawMessage);

        /// <summary>
        /// updates all the client proxies, and then the host 
        /// </summary>
        /// <param name="time"></param>
        void update(GameTime time);

        /// <summary>
        /// called when the model is first created
        /// </summary>
        void init();

        /// <summary>
        /// updates the model
        /// </summary>
        /// <param name="time"></param>
        void updateHost(GameTime time);



        /// <summary>
        /// validates a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool validateMessage(SMessage message);

        /// <summary>
        /// called when a message is recieved
        /// </summary>
        /// <param name="message"></param>
        void onMessage(SMessage message);

        /// <summary>
        /// called when the when the host is shutting down.
        /// </summary>
        void onShutdown();
   


    }
}
