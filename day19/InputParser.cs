using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace day19
{
    public struct MonsterMessages
    {
        public Dictionary<int, IBaseRule> Rules;
        public List<string> Messages;
    }

    public interface IBaseRule
    {
    }

    public struct Rule : IBaseRule
    {
        public string Text;
    }

    public struct OrRule : IBaseRule
    {
        public int[] A;
        public int[] B;
    }

    public struct OneRule : IBaseRule
    {
        public int[] One;
    }

    public class InputParser
    {
        internal static MonsterMessages Parse(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            var rules = new Dictionary<int, IBaseRule>();
            var messages = new List<string>();

            int state = 0;

            foreach (var line in lines)
            {
                if (line == string.Empty)
                {
                    state = 1;
                    continue;
                }

                if (state == 0)
                {
                    var parts = line.Split(": ");
                    int key = int.Parse(parts[0]);

                    if (parts[1] == "\"a\"" || parts[1] == "\"b\"")
                        rules.Add(key, new Rule { Text = parts[1].Replace("\"", "") });
                    else if (parts[1].Contains("|"))
                    {
                        var or = parts[1].Split(" | ");

                        var firstNumbers = or[0].Split(' ').Select(n => int.Parse(n)).ToArray();
                        var secondNumbers = or[1].Split(' ').Select(n => int.Parse(n)).ToArray();

                        rules.Add(key, new OrRule
                        {
                            A = firstNumbers,
                            B = secondNumbers
                        });
                    }
                    else
                    {
                        var numbers = parts[1].Split(' ').Select(n => int.Parse(n)).ToArray();

                        rules.Add(key, new OneRule
                        {
                            One = numbers
                        });
                    }
                }
                else
                {
                    messages.Add(line);
                }
            }

            return new MonsterMessages
            {
                Rules = rules,
                Messages = messages
            };
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
