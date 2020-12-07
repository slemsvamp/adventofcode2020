using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace day07
{
    public class InputParser
    {
        internal static List<LuggageDescription> Parse(string filename)
        {
            var result = new List<LuggageDescription>();

            var containRegex = new Regex(@"^(?<container>.*) bags contain (?<containees>.*)\.$");
            var containeesRegex = new Regex(@"(?<number>\d*) (?<description>.+) bag");

            foreach (var line in File.ReadAllLines(filename))
            {
                var containResult = containRegex.Match(line);

                var luggage = new LuggageDescription(1, containResult.Groups["container"].Value);

                var containees = containResult.Groups["containees"].Value;

                if (containees == "no other bags")
                {
                    result.Add(luggage);
                    continue;
                }

                var parts = containees.Split(", ");
                foreach (var part in parts)
                {
                    var containeeResult = containeesRegex.Match(part);
                    
                    var containeeNumber = int.Parse(containeeResult.Groups["number"].Value);
                    var containeeDescription = containeeResult.Groups["description"].Value;

                    var containee = new LuggageDescription(containeeNumber, containeeDescription);

                    luggage.Contains.Add(containee);
                }

                result.Add(luggage);
            }

            return result;
        }
    }
}
