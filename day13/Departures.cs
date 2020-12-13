using System;
using System.Collections.Generic;
using System.Text;

namespace day13
{
    public struct Departures
    {
        public int EarliestDepartureTimestamp;
        public List<(int Id, int Order)> Buses;
        public (int Id, int Order) BusWithHighestId;
    }
}
