using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SmallNet.Samples.BasicMove
{
    class BasicPlayer
    {

        private Vector2 pos;
        private Vector2 vel;
        private float size = 50;
        private int id;
        public BasicPlayer(int id, Vector2 startPos)
        {
            this.id = id;
            this.pos = startPos;
            vel = Vector2.Zero;
        }


        public Vector2 Position { get { return pos; } set { pos = value; } }
        public Vector2 Velocity { get { return vel; } set { vel = value; } }
        public int Id { get { return id; } }

        //////

        public void update()
        {
            pos += vel;
        }

        public void draw(PrimitiveBatch prim, SpriteBatch spr, SpriteFont font)
        {
            prim.Begin(Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleStrip);
            {
                prim.AddVertex(pos + new Vector2(1, 1), Color.Red);
                prim.AddVertex(pos - size * Vector2.UnitX, Color.Green);
                prim.AddVertex(pos - size * Vector2.UnitY, Color.Yellow);
            }
            prim.End();

            spr.Begin();
            spr.DrawString(font, "ID: " + this.id, pos, Color.Black);
            spr.End();

        }

    }
}
