using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet
{
    interface Host
    {
        void acceptConnection();
        void acceptDisconnect();
        
        void sendMessage();
        void receiveMessage();
        void broadCastMessage();
        

    }
}
