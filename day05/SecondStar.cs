using System;
using System.Collections.Generic;
using System.Text;

namespace day05
{
    public class SecondStar
    {
        public static string Run(Dictionary<int, Seat> seatings)
        {
            int seatingId = -1;

            foreach (var seat in seatings.Values)
                if (seat.Id > seatingId)
                    seatingId = seat.Id;

            while (seatings.ContainsKey(--seatingId)) ;

            return seatingId.ToString();
        }
    }
}
