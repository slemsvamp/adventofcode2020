using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day09
{
    public class SecondStar
    {
        public static string Run(List<long> numbers, string part1Answer)
        {
            long targetSum = long.Parse(part1Answer);
            var terms = new List<long>();

            for (int playhead = 0; playhead < numbers.Count; playhead++)
            {
                long sum = numbers[playhead];

                for (int width = 1; playhead + width < numbers.Count; width++)
                {
                    sum += numbers[playhead + width];

                    if (sum > targetSum)
                        break;

                    if (sum == targetSum)
                        for (int termsIndex = playhead; termsIndex <= playhead + width; termsIndex++)
                            terms.Add(numbers[termsIndex]);
                }

                if (terms.Count > 0)
                    break;
            }

            return (terms.Min() + terms.Max()).ToString();
        }
    }
}
