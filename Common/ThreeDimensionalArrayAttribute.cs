using System;
using System.Collections.Generic;
using System.Text;

namespace Remoting
{
    public class ThreeDimensionalArrayAttribute : Attribute
    {
        private int x;
        private int y;
        private int z;
        public ThreeDimensionalArrayAttribute(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public int Z
        {
            get { return z; }
            set { z = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

    }
}
