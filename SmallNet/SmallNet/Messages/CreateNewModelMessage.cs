using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.Messages
{
    class CreateNewModelMessage : SMessage
    {
        private int id;

        public CreateNewModelMessage() : base(){ }

        public CreateNewModelMessage(Id idObject, int id)
            : base(idObject)
        {
            this.id = id;
        }

        public int Id { get { return this.id; } }

        public override string ToString()
        {
            return "FROM: " + this.SenderId + " : MINE: " + id;
        }
    }
}
