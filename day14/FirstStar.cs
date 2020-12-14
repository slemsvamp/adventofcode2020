using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day14
{
    public class FirstStar
    {
        public static string Run(List<Segment> segments)
        {
            var memory = new Dictionary<long, long>();

            foreach (var segment in segments)
                foreach (var instruction in segment.Instructions)
                {
                    var maskedValue = instruction.Value;
                    for (int maskIndex = 35; maskIndex >= 0; maskIndex--)
                    {
                        long value = (long)1 << (35 - maskIndex);
                        if (segment.Mask.Filter[maskIndex] == '1' && (maskedValue & value) != value)
                            maskedValue += value;
                        else if (segment.Mask.Filter[maskIndex] == '0' && (maskedValue & value) == value)
                            maskedValue -= value;
                    }

                    if (!memory.ContainsKey(instruction.MemIndex))
                        memory.Add(instruction.MemIndex, 0);
                    memory[instruction.MemIndex] = maskedValue;
                }

            return memory.Values.Sum().ToString();
        }
    }
}
