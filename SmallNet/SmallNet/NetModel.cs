using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet
{
    class NetModel
    {
        private List<BaseClientProxy> clients;

        public NetModel()
        {
            this.clients = new List<BaseClientProxy>();
        }

        public void addClient(BaseClientProxy client)
        {
            this.clients.Add(client);
        }
    }
}
