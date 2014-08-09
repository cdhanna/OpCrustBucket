using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace SmallNet
{
    /// <summary>
    /// core player object.
    /// </summary>
    public abstract class DefaultPlayer
    {

        private int id;

        public DefaultPlayer(int id)
        {
            this.id = id;
        }

        public int Id { get { return this.id; } }

        public abstract void update(GameTime time);
        public abstract void draw(object drawdata); //TODO replace with spritebatch and primitivebatch and camera
        

    }
}
