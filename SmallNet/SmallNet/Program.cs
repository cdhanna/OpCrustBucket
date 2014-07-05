#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace SmallNet
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Messages.ConnectionMessage m1 = new Messages.ConnectionMessage("Howdy");
            //string str = Serializer.serialize(m1);
            //Messages.ConnectionMessage m2 = (Messages.ConnectionMessage)Serializer.deserialize(str);
            //SimpleNetTest test = new SimpleNetTest();
            using (var game = new Test())
                game.Run();
        }
    }
#endif
}
