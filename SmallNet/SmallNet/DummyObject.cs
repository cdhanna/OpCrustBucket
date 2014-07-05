using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmallNet
{
    class DummyObject
    {
        protected int x;
        protected string words;

        public DummyObject()
        {
            int x = 4;
        }

        public void setString(string str)
        {
            this.words = str;
        }
    }
}
