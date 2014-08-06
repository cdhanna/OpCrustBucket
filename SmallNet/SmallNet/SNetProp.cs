using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet
{

    /// <summary>
    /// A collection of SmallNet related constants. 
    /// </summary>
    class SNetProp
    {
        //TODO, convert all of this to read in from an .xml file

        public const int HOST_ID = 1; //means player ID will start at 2
        public static int getPort()
        {
            return 5005;
        }

        
    }
}
