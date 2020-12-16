using System;
using System.Collections.Generic;
using System.Text;

namespace day16
{
    public struct TicketRange
    {
        public int From;
        public int To;

        public TicketRange(int from, int to)
        {
            From = from;
            To = to;
        }
    }
}
