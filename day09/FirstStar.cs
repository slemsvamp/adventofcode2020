using System;
using System.Collections.Generic;
using System.Text;

namespace day09
{
    public class FirstStar
    {
        public static string Run(List<long> numbers)
        {
            int preamble = 25;
            var terms = new Queue<long>();

            for (int preambleIndex = 0; preambleIndex < preamble; preambleIndex++)
                terms.Enqueue(numbers[preambleIndex]);

            long targetSum = 0;

            for (int playhead = preamble; playhead < numbers.Count; playhead++)
            {
                bool valid = false;
                targetSum = numbers[playhead];

                var termsArray = terms.ToArray();

                for (int leftTermIndex = 0; leftTermIndex < preamble && playhead + leftTermIndex < numbers.Count; leftTermIndex++)
                    for (int rightTermIndex = leftTermIndex + 1; rightTermIndex < preamble; rightTermIndex++)
                        if (termsArray[leftTermIndex] + termsArray[rightTermIndex] == targetSum)
                            valid = true;

                terms.Dequeue();
                terms.Enqueue(numbers[playhead]);

                if (!valid)
                    break;
            }

            return targetSum.ToString();
        }
    }
}
