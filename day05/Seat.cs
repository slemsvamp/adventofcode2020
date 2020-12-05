using System;
using System.Collections.Generic;
using System.Text;

namespace day05
{
    public struct Seat
    {
        public int Row;
        public int Column;
        public int Id;

        public Seat(int row, int column)
        {
            Row = row;
            Column = column;
            Id = row * 8 + column;
        }
    }
}
