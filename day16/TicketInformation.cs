using System;
using System.Collections.Generic;
using System.Text;

namespace day16
{
    public class TicketInformation
    {
        public Dictionary<string, TicketRange[]> Details;
        public int[] MyTicket;
        public List<int[]> NearbyTickets;

        public TicketInformation()
        {
            Details = new Dictionary<string, TicketRange[]>();
            NearbyTickets = new List<int[]>();
        }
    }
}
