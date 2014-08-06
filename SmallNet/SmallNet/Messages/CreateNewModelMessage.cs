using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet.Messages
{
    /// <summary>
    /// simple message that notifies a new client to create a new model. This message contains the unique ID that the new client will use
    /// </summary>
    class CreateNewModelMessage : SMessage
    {
        private int id;

        public CreateNewModelMessage() : base(){ }

        public CreateNewModelMessage(Id idObject, int id)
            : base(idObject)
        {
            this.id = id;
        }

        /// <summary>
        /// The id that the client should use for itself. 
        /// </summary>
        public int Id { get { return this.id; } }

        public override string ToString()
        {
            return "FROM: " + this.SenderId + " : MINE: " + id;
        }
    }
}
