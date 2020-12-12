using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day12
{
    public class InputParser
    {
        internal static List<(Movement movement, int value)> Parse(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            var instructions = new List<(Movement movement, int value)>();

            foreach (var line in lines)
            {
                switch (line[0])
                {
                    case 'N':
                        instructions.Add((Movement.North, int.Parse(line.Substring(1))));
                        break;
                    case 'S':
                        instructions.Add((Movement.South, int.Parse(line.Substring(1))));
                        break;
                    case 'W':
                        instructions.Add((Movement.West, int.Parse(line.Substring(1))));
                        break;
                    case 'E':
                        instructions.Add((Movement.East, int.Parse(line.Substring(1))));
                        break;
                    case 'L':
                        instructions.Add((Movement.Left, int.Parse(line.Substring(1))));
                        break;
                    case 'R':
                        instructions.Add((Movement.Right, int.Parse(line.Substring(1))));
                        break;
                    case 'F':
                        instructions.Add((Movement.Forward, int.Parse(line.Substring(1))));
                        break;
                }
            }

            return instructions;
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
