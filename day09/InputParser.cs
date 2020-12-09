using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day09
{
    public class InputParser
    {
        internal static List<long> Parse(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            List<long> numbers = new List<long>();

            foreach (var line in lines)
            {
                numbers.Add(long.Parse(line));
            }

            return numbers;
        }

        public static List<int> ParseCSV(string filename)
        {
            var lines = File.ReadAllLines(filename);

            var numbers = new List<int>();
            var numberStrings = lines[0].Split(new[] { "," }, StringSplitOptions.None);

            Array.ForEach(numberStrings, n => numbers.Add(int.Parse(n)));

            return numbers;
        }
    }
}
