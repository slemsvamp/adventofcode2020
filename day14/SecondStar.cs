using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day14
{
    public class SecondStar
    {
        public static string Run(List<Segment> segments)
        {
            var memory = new Dictionary<long, long>();

            foreach (var segment in segments)
                foreach (var instruction in segment.Instructions)
                {
                    var maskedIndex = instruction.MemIndex;
                    var floatingIndices = new List<int>();
                    for (int maskIndex = 35; maskIndex >= 0; maskIndex--)
                    {
                        long value = (long)1 << (35 - maskIndex);
                        if (segment.Mask.Filter[maskIndex] == '1' && (maskedIndex & value) != value)
                            maskedIndex += value;
                        else if (segment.Mask.Filter[maskIndex] == 'X')
                            floatingIndices.Add(35 - maskIndex);
                    }

                    foreach (var index in CalculateIndices(maskedIndex, floatingIndices))
                    {
                        if (!memory.ContainsKey(index))
                            memory.Add(index, 0);
                        memory[index] = instruction.Value;
                    }
                }

            return memory.Values.Sum().ToString();
        }

        private static HashSet<long> CalculateIndices(long maskedIndex, List<int> floatingIndices)
        {
            var calculatedIndices = new HashSet<long>();
            RecursiveCrawl(0, maskedIndex, floatingIndices, calculatedIndices);
            return calculatedIndices;
        }

        public static void RecursiveCrawl(int depth, long maskedIndex, List<int> floatingIndices, HashSet<long> calculatedIndices)
        {
            if (depth >= floatingIndices.Count)
                return;

            var value = (long)1 << floatingIndices[depth];

            var maskedIndexOne = Mask(maskedIndex, 1, value);
            var maskedIndexZero = Mask(maskedIndex, 0, value);

            calculatedIndices.Add(maskedIndexOne);
            calculatedIndices.Add(maskedIndexZero);

            if (depth < floatingIndices.Count)
            {
                RecursiveCrawl(depth + 1, maskedIndexOne, floatingIndices, calculatedIndices);
                RecursiveCrawl(depth + 1, maskedIndexZero, floatingIndices, calculatedIndices);
            }
        }

        public static long Mask(long index, long bit, long value)
        {
            if (bit == 1 && (index & value) != value)
                index += value;
            else if (bit == 0 && (index & value) == value)
                index -= value;
            return index;
        }
    }
}
