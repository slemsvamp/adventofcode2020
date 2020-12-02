using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace day02
{
    public class InputParser
    {
        internal static List<PasswordCheck> Parse(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            var results = new List<PasswordCheck>();
            var regex = new Regex(@"(?<first>[\d]+)-(?<second>[\d]+) (?<letter>[\w]): (?<password>[\w]*)");

            foreach (var line in lines)
            {
                var match = regex.Match(line);

                results.Add(new PasswordCheck
                {
                    First = int.Parse(match.Groups["first"].Value),
                    Second = int.Parse(match.Groups["second"].Value),
                    Letter = match.Groups["letter"].Value,
                    Password = match.Groups["password"].Value
                });
            }

            return results;
        }
    }
}
