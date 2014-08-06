using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace SmallNet
{
    /// <summary>
    /// A set of methods that run on every client and host. 
    /// </summary>
    public interface ClientModel
    {

        /******
         * FIELDS
         ******/
       

        /// <summary>
        /// Reveals if this client model is running on a client machine, or a host machine
        /// </summary>
        NetworkSide Owner { get; }
        
        /// <summary>
        /// A unique id for the model. The first model will start ID=2
        /// </summary>
        int Id { get; }
        
        /// <summary>
        /// Event that fires when a message is recieved 
        /// </summary>
        EventHandler<MessageEventArgs> MessageRecieved { get; set; }


        /******
         * METHODS
         ******/

        /// <summary>
        /// Called upon creation of the model
        /// </summary>
        /// <param name="netWriter"></param>
        /// <param name="owner"></param>
        void create(StreamWriter netWriter, NetworkSide owner);
        
        /// <summary>
        /// Called after creation of the model
        /// </summary>
        void init();
       

        /// <summary>
        /// Called every update tick
        /// </summary>
        /// <param name="time"></param>
        void update(GameTime time);
        
        /// <summary>
        /// Called when the client disconnects
        /// </summary>
        void destroy();
        
        /// <summary>
        /// Called to set the ID of the model
        /// </summary>
        /// <param name="id"></param>
        void setId(int id);

        /// <summary>
        /// Called to validatea message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>True, if the message should be allowed to be sent back to clients. </returns>
        bool validateMessage(SMessage message);
        
        /// <summary>
        /// Called when the client recieves a message.
        /// </summary>
        /// <param name="message"></param>
        void onMessage(SMessage message);
        
        /// <summary>
        /// Send a message to the host, and therefor all other clients
        /// </summary>
        /// <param name="message"></param>
        void sendMessage(SMessage message);
        
        /// <summary>
        /// Send a message to the host, and therefor all other clients
        /// </summary>
        /// <param name="rawMessage"></param>
        void sendMessage(String rawMessage);
        
        /// <summary>
        /// Called when a new player joined. THis method is called when THIS model joins aswell. 
        /// </summary>
        /// <param name="id"></param>
        void playerJoined(int id);
    }
}
