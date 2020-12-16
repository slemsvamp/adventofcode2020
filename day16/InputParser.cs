using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day16
{
    public class InputParser
    {
        /*
            departure location: 43-237 or 251-961
            departure station: 27-579 or 586-953
            departure platform: 31-587 or 608-967
            departure track: 26-773 or 784-973
            departure date: 41-532 or 552-956
            departure time: 33-322 or 333-972
            arrival location: 30-165 or 178-965
            arrival station: 31-565 or 571-968
            arrival platform: 36-453 or 473-963
            arrival track: 35-912 or 924-951
            class: 39-376 or 396-968
            duration: 31-686 or 697-974
            price: 28-78 or 96-971
            route: 32-929 or 943-955
            row: 40-885 or 896-968
            seat: 26-744 or 765-967
            train: 46-721 or 741-969
            type: 30-626 or 641-965
            wagon: 48-488 or 513-971
            zone: 34-354 or 361-973

            your ticket:
            151,71,67,113,127,163,131,59,137,103,73,139,107,101,97,149,157,53,109,61

            nearby tickets:
            680,418,202,55,792,800,896,801,312,252,721,702,24,112,608,837,98,222,797,364
         */

        internal static TicketInformation Parse(string filename)
        {
            var lines = File.ReadAllLines(filename);

            int inputMode = 0;

            var result = new TicketInformation();

            for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
            {
                var line = lines[lineIndex];

                if (line == string.Empty)
                {
                    inputMode++;
                    continue;
                }

                if (line == "your ticket:" || line == "nearby tickets:")
                    continue;

                switch (inputMode)
                {
                    case 0:
                    {
                        var parts = line.Split(": ");
                        var ranges = parts[1].Split(" or ");
                        var rangeA = ranges[0].Split("-").Select(n => int.Parse(n)).ToArray();
                        var rangeB = ranges[1].Split("-").Select(n => int.Parse(n)).ToArray();

                        result.Details.Add(parts[0], new TicketRange[]
                        {
                            new TicketRange(rangeA[0], rangeA[1]),
                            new TicketRange(rangeB[0], rangeB[1])
                        });
                    }
                    break;
                    case 1:
                    {
                        var numbers = line.Split(',');
                        result.MyTicket = numbers.Select(n => int.Parse(n)).ToArray();
                    }
                    break;
                    case 2:
                    {
                        var numbers = line.Split(',');
                        result.NearbyTickets.Add(numbers.Select(n => int.Parse(n)).ToArray());
                    }
                    break;
                }

            }

            return result;
        }
    }
}
