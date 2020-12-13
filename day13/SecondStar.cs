using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day13
{
    public class SecondStar
    {
        public static string Run(Departures departures)
        {
            long time = 0, step = 1;

            for (var busIndex = 0; busIndex < departures.Buses.Count(); busIndex++)
            {
                var id = departures.Buses[busIndex].Id;
                var order = departures.Buses[busIndex].Order;

                while ((time + order) % id != 0)
                    time += step;

                step = LowestCommonMultiplier(step, id);
            }

            return time.ToString();
        }

        private static long LowestCommonMultiplier(long left, long right)
        {
            if (left < right)
                (left, right) = (right, left);

            for (long iteration = 1; iteration < right; iteration++)
            {
                long mult = left * iteration;
                if (mult % right == 0)
                    return mult;
            }
            return left * right;
        }

        public static string RunBruteforce(Departures departures)
        {
            long timestamp;
            int success;

            // I bruteforced this one, so I'm starting at the earliest factor I found.
            long factor = 631924057460;

            while (true)
            {
                success = 0;
                timestamp = (factor * departures.BusWithHighestId.Id) - departures.BusWithHighestId.Order;
                foreach (var bus in departures.Buses)
                {
                    if ((timestamp + bus.Order) % bus.Id == 0)
                        success++;
                    else
                        break;
                }
                if (success == departures.Buses.Count)
                    return timestamp.ToString();
                factor++;
            }
        }
    }
}
