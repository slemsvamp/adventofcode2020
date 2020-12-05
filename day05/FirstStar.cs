using System;
using System.Collections.Generic;
using System.Text;

namespace day05
{
    public class FirstStar
    {
        public static string Run(Dictionary<int, Seat> seatings)
        {
            int highestSeatingId = -1;

            foreach (var seat in seatings.Values)
                if (seat.Id > highestSeatingId)
                    highestSeatingId = seat.Id;

            return highestSeatingId.ToString();
        }
    }
}
