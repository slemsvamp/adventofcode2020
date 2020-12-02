using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day01
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
    }
}
