using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day05
{
    public class InputParser
    {
        internal static List<int> Parse(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            List<int> numbers = new List<int>();

            foreach (var line in lines)
            {
                numbers.Add(int.Parse(line));
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
