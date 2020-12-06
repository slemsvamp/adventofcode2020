using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day06
{
    public class InputParser
    {
        internal static List<GroupAnswers> Parse(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            var groupAnswers = new List<GroupAnswers>();

            var groupAnswer = new GroupAnswers();

            foreach (var line in lines)
            {
                if (line == string.Empty)
                {
                    groupAnswers.Add(groupAnswer);
                    groupAnswer = new GroupAnswers();
                }
                else
                {
                    groupAnswer.Answers.Add(line);
                }
            }
            groupAnswers.Add(groupAnswer);

            return groupAnswers;
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
