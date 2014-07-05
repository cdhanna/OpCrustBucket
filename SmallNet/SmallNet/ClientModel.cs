using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace SmallNet
{
    public interface ClientModel
    {

        double TargetTime { get; set; }
        string Owner { get; }

        EventHandler<MessageEventArgs> MessageRecieved { get; set; }

        void create(StreamWriter netWriter, string owner);
        void init();
        void keepTime();
        void update(GameTime time);
        void destroy();

        bool validateMessage(SMessage message);
        void onMessage(SMessage message);
        void sendMessage(SMessage message);

    }
}
