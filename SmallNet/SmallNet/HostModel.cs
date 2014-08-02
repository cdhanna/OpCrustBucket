using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Net.Sockets;
namespace SmallNet
{
    public interface HostModel<T> where T:ClientModel
    {

        BaseClientProxy<T> getClientProxy(int index);
        BaseClientProxy<T> generateNewClient(Socket socket);
        void addClient(BaseClientProxy<T> client);
        void removeClient(BaseClientProxy<T> client);
        void sendMessageToAll(SMessage message);
        void update(GameTime time);

        void init();
        void updateHost(GameTime time);
        void destroy();

        bool validateMessage(SMessage message);
        void onMessage(SMessage message);

        void onShutdown();
       // void sendMessage(SMessage message);


    }
}
