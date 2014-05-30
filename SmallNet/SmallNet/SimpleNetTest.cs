using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;[assembly: log4net.Config.XmlConfigurator(Watch = true)]


namespace SmallNet
{
    class SimpleNetTest
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public SimpleNetTest()
        {
           
            log.Debug("Starting");

            BaseHost host = new BaseHost();
            host.Debug = true;
            host.start();

            BaseClient client = new BaseClient();
            client.Debug = true;
            client.connectTo(host.IpAddress, "notBen");

            client.sendMessage("testType", "abc", "123");
        }

    }
}
