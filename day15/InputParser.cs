using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day15
{
    public class InputParser
    {
        public static List<int> Parse(string filename)
        {
            var lines = File.ReadAllLines(filename);

            var numbers = new List<int>();
            var numberStrings = lines[0].Split(new[] { "," }, StringSplitOptions.None);

            Array.ForEach(numberStrings, n => numbers.Add(int.Parse(n)));

            return numbers;
        }
    }
}
