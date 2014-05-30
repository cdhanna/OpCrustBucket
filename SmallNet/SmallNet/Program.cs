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
            SimpleNetTest test = new SimpleNetTest();
           // using (var game = new Test())
           //     game.Run();
        }
    }
#endif
}
