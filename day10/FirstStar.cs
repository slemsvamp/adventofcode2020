using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day10
{
    public class FirstStar
    {
        public static string Run(List<int> jolts)
        {
            int joltageRating = 0;
            var differences = new Dictionary<int, int>();

            foreach (var jolt in jolts.OrderBy(j => j))
            {
                if (jolt <= joltageRating + 3)
                {
                    var diff = jolt - joltageRating;

                    if (!differences.ContainsKey(diff))
                        differences.Add(diff, 0);
                    differences[diff] += 1;

                    joltageRating = jolt;
                }
            }

            if (!differences.ContainsKey(3))
                differences.Add(3, 0);
            differences[3] += 1;

            return (differences[1] * differences[3]).ToString();
        }
    }
}
