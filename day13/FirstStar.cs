using System;
using System.Collections.Generic;
using System.Text;

namespace day13
{
    public class FirstStar
    {
        public static string Run(Departures departures)
        {
            int timestamp = departures.EarliestDepartureTimestamp;

            while (true)
            {
                foreach (var bus in departures.Buses)
                {
                    if (timestamp % bus.Id == 0)
                        return ((timestamp - departures.EarliestDepartureTimestamp) * bus.Id).ToString();
                }
                timestamp++;
            }
        }
    }
}
