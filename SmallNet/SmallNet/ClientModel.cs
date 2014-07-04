using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace SmallNet
{
    interface ClientModel
    {

        double TargetTime { get; set; }
        string Owner { get; }

        void create(StreamWriter netWriter, string owner);
        void init();
        void keepTime();
        void update(GameTime time);
        void destroy();

        bool validateMessage(string msgType, params string[] parameters);
        void onMessage(string msgType, params string[] parameters);
        void sendMessage(string msgType, params object[] parameters);

    }
}
