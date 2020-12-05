using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day05
{
    public class InputParser
    {
        internal static Dictionary<int, Seat> Parse(string filename)
        {
            var result = new Dictionary<int, Seat>();
            var seatings = File.ReadAllLines(filename);

            foreach (var seating in seatings)
            {
                string frontBack = seating.Substring(0, 7);
                string leftRight = seating.Substring(7);

                int rowMin = 0;
                int rowMax = 127;
                foreach (var fB in frontBack)
                {
                    var half = (rowMax - rowMin) / 2 + 1;
                    if (fB == 'F')
                        rowMax -= half;
                    else
                        rowMin += half;
                }

                int columnMin = 0;
                int columnMax = 7;
                foreach (var lR in leftRight)
                {
                    var half = (columnMax - columnMin) / 2 + 1;
                    if (lR == 'L')
                        columnMax -= half;
                    else
                        columnMin += half;
                }

                var seat = new Seat(rowMin, columnMin);
                result.Add(seat.Id, seat);
            }

            return result;
        }
    }
}
