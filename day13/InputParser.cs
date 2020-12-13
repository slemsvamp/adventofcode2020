using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day13
{
    public class InputParser
    {
        internal static Departures Parse(string filename)
        {
            var lines = File.ReadAllLines(filename);

            var result = new Departures()
            {
                EarliestDepartureTimestamp = int.Parse(lines[0]),
                Buses = new List<(int Id, int Order)>(),
                BusWithHighestId = (0, 0)
            };

            var ids = lines[1].Split(',');
            int id = -1;

            for (int index = 0; index < ids.Length; index++)
                if (int.TryParse(ids[index], out id))
                {
                    result.Buses.Add((id, index));
                    if (result.BusWithHighestId.Id < id)
                        result.BusWithHighestId = (id, index);
                }

            return result;
        }
    }
}
