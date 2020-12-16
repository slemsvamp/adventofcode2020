using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day16
{
    public class FirstStar
    {
        public static string Run(TicketInformation ticketInformation)
        {
            var invalidNumbers = new List<int>();
            foreach (var nearbyTicket in ticketInformation.NearbyTickets)
                foreach (var value in nearbyTicket)
                {
                    bool validForAtleastOneField = false;
                    foreach (var detail in ticketInformation.Details)
                    {
                        var ranges = detail.Value;

                        if ((ranges[0].From <= value && ranges[0].To >= value)
                            || (ranges[1].From <= value && ranges[1].To >= value))
                        {
                            validForAtleastOneField = true;
                            break;
                        }
                    }
                    if (!validForAtleastOneField)
                        invalidNumbers.Add(value);
                }

            return invalidNumbers.Sum().ToString();
        }
    }
}
