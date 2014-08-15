using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace SmallNet.Messages
{
    class DummySubObject
    {
        private int x;
        private Vector2 mypos;

        public DummySubObject(int x, float a, float b)
        {
            this.x = x;
            this.mypos = new Vector2(a, b);
        }
        public DummySubObject()
        {
        }
    }
}
