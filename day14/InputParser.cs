using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace day14
{
    public class Segment
    {
        public Mask Mask;
        public Instruction[] Instructions;

        public Segment()
        {

        }
    }

    public class Mask
    {
        public string Filter;
    }

    public class Instruction
    {
        public long MemIndex;
        public long Value;
    }

    public class InputParser
    {
        static Regex maskRegex = new Regex(@"^mask = (?<mask>[0|X|1]*)$");
        static Regex memRegex = new Regex(@"^mem\[(?<memindex>\d+)\] = (?<value>\d+)$");

        internal static List<Segment> Parse(string filename)
        {
            /*
             * mask = 0X10110X1001000X10X00X01000X01X01101
                mem[49559] = 97
                mem[18692] = 494387917
                mem[9337] = 615452
            */

            var lines = File.ReadAllLines(filename);
            var segments = new List<Segment>();
            Segment segment = null;
            var instructions = new List<Instruction>();

            foreach (var line in lines)
            {
                if (line.StartsWith("mask"))
                {
                    if (segment != null)
                    {
                        segment.Instructions = instructions.ToArray();
                        segments.Add(segment);
                        instructions = new List<Instruction>();
                    }

                    segment = new Segment()
                    {
                        Mask = new Mask()
                        {
                            Filter = line.Substring(7)
                        }
                    };
                }
                else
                {
                    var mem = memRegex.Match(line);
                    instructions.Add(new Instruction()
                    {
                        MemIndex = long.Parse(mem.Groups["memindex"].Value),
                        Value = long.Parse(mem.Groups["value"].Value)
                    });
                }
            }

            segment.Instructions = instructions.ToArray();
            segments.Add(segment);

            return segments;
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
