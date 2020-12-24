using System;
using System.Collections.Generic;
using System.Text;

namespace day24
{
    public class Hex
    {
        public bool White;
        public int Index;

        public Hex(int index, bool white = true)
        {
            Index = index;
            White = white;
        }
    }
}
