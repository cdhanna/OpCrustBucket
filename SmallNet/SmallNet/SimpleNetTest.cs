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

            BaseHost<TestClientModel> host = new BaseHost<TestClientModel>();
            log.Debug("IPaddress " + host.IpAddress);
            host.Debug = true;
            host.start();

            BaseClient<TestClientModel> client = new BaseClient<TestClientModel>();
            client.Debug = true;
            client.connectTo(host.IpAddress, "notBen");

            client.sendMessage("testType", "abc", "123");

            System.Threading.Thread.Sleep(500);

            client.shutdown();
            host.shutdown();
        }

    }
}
