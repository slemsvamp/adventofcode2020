using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day10
{
    public class SecondStar
    {
        public static string Run(List<int> jolts)
        {
            int index = 1;
            var adapters = new int[jolts.Count + 2];
            var memo = new long[adapters.Length];

            adapters[0] = 0;
            foreach (var jolt in jolts.OrderBy(j => j))
                adapters[index++] = jolt;

            adapters[jolts.Count + 1] = jolts.Max() + 3;
            memo[adapters.Length - 1] = 1;

            for (int playhead = adapters.Length - 2; playhead >= 0; playhead--)
            {
                long paths = 0;

                for (int width = playhead + 1; width < adapters.Length; width++)
                    if (adapters[width] - adapters[playhead] <= 3)
                        paths += memo[width];
                    else break;

                memo[playhead] = paths;
            }

            return memo[0].ToString();
        }
    }
}
