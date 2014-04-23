using System;
using System.Collections.Generic;
using System.Text;

namespace Remoting
{
    public class TwoDimensionalArrayAttribute : Attribute
    {
        private int x;
        private int y;
        public TwoDimensionalArrayAttribute(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }
    }
}
