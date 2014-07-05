using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.DebugSession
{
    public abstract class CommandOption<T> where T:ClientModel
    {

        private String name;

        public CommandOption(String name)
        {
            this.name = name;
        }


        public String getName()
        {
            return this.name;
        }

        public abstract String runCommand(DebugSession<T> debug, String[] paramString);
        public abstract String getParamDescription();
        
        
        public override String ToString()
        {
            return this.name;
        }
    }
}
