using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day04
{
    public class Passport
    {
        public Dictionary<string, string> Data;

        public Passport()
        {
            Data = new Dictionary<string, string>();
        }
    }

    public class InputParser
    {

        internal static List<Passport> Parse(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            var passports = new List<Passport>();
            Passport passport = new Passport();

            foreach (var line in lines)
            {
                if (line == string.Empty)
                {
                    passports.Add(passport);
                    passport = new Passport();
                }
                else
                {
                    var snippets = line.Split(" ");

                    foreach (var snippet in snippets)
                    {
                        var keyValue = snippet.Split(":");

                        passport.Data.Add(keyValue[0], keyValue[1]);
                    }
                }
            }

            passports.Add(passport);

            return passports;
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
