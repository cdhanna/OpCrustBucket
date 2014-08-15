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
            //Messages.DummyMessage m1 = new Messages.DummyMessage(new DummyIdClass(),"Howdy",2.2f,3.0f);
            //string str = Serializer.serialize(m1);
            //Messages.DummyMessage m2 = (Messages.DummyMessage)Serializer.deserialize(str);
            //SimpleNetTest test = new SimpleNetTest();
           // Test t = new Test();
            
            using (var game = new Test())
            {
                game.Run();
            }

            //SNetConnector snc = new SNetConnector();

        }
    }
#endif
}
