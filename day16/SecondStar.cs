using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day16
{
    public class SecondStar
    {
        public static string Run(TicketInformation ticketInformation)
        {
            var invalidNumbers = new List<int>();
            var validTickets = new List<int[]>();
            var suspectedKeys = new Dictionary<int, List<string>>();

            for (int n = 0; n < ticketInformation.MyTicket.Length; n++)
                suspectedKeys.Add(n, ticketInformation.Details.Keys.ToList());

            foreach (var nearbyTicket in ticketInformation.NearbyTickets)
            {
                bool validTicket = true;
                for (var index = 0; index < nearbyTicket.Length; index++)
                {
                    var value = nearbyTicket[index];

                    bool validForAtleastOneField = false;

                    foreach (var detail in ticketInformation.Details)
                    {
                        var ranges = detail.Value;

                        if ((ranges[0].From <= value && ranges[0].To >= value)
                            || (ranges[1].From <= value && ranges[1].To >= value))
                            validForAtleastOneField = true;
                    }
                    
                    if (!validForAtleastOneField)
                    {
                        invalidNumbers.Add(value);
                        validTicket = false;
                    }
                }
                if (validTicket)
                    validTickets.Add(nearbyTicket);
            }

            foreach (var ticket in validTickets)
                for (int index = 0; index < ticket.Length; index++)
                {
                    var value = ticket[index];

                    foreach (var detail in ticketInformation.Details)
                    {
                        var ranges = detail.Value;
                        var notInRange = (ranges[0].From > value || ranges[0].To < value)
                            && (ranges[1].From > value || ranges[1].To < value);
                        
                        if (notInRange && suspectedKeys[index].Contains(detail.Key))
                            suspectedKeys[index].Remove(detail.Key);
                    }
                }

            var listOfKeys = new HashSet<string>(ticketInformation.Details.Keys);

            while (listOfKeys.Count > 0)
            {
                var lonelyKey = suspectedKeys.Where(k => k.Value.Count == 1 && listOfKeys.Contains(k.Value[0])).First().Value.Single();
                foreach (var keyValuePair in suspectedKeys)
                    if (keyValuePair.Value.Count > 1 && keyValuePair.Value.Contains(lonelyKey))
                        suspectedKeys[keyValuePair.Key].Remove(lonelyKey);
                listOfKeys.Remove(lonelyKey);
            }

            // Unnecessary rewrite to "detail => value" form, but I wanted it in that form as a design choice.
            var detailInformation = new Dictionary<string, int>();
            foreach (var suspectedKey in suspectedKeys)
                detailInformation.Add(suspectedKey.Value[0], suspectedKey.Key);

            long total = 1;
            foreach (var detail in detailInformation.Where(k => k.Key.StartsWith("departure")))
                total *= ticketInformation.MyTicket[detail.Value];

            return total.ToString();
        }
    }
}
