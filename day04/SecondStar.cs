using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace day04
{
    public class SecondStar
    {
        public static Regex _heightRegex;
        public static Regex _hairColorRegex;
        public static Regex _eyeColorRegex;
        public static Regex _passportIdRegex;

        public static string Run(List<Passport> passports)
        {
            _heightRegex = new Regex(@"^(?<number>\d+)(?<type>in|cm)$");
            _hairColorRegex = new Regex(@"^#(\d|[a-f]){6}$");
            _eyeColorRegex = new Regex(@"^(amb|blu|brn|gry|grn|hzl|oth)$");
            _passportIdRegex = new Regex(@"^(\d{9})$");

            var validCount = 0;
            foreach (var passport in passports)
            {
                bool valid = Validate(passport);
                if (valid)
                    validCount++;
            }

            return validCount.ToString();
        }

        public static bool Validate(Passport passport)
        {
            if (passport.Data.Count < 7)
                return false;

            if (passport.Data.Count == 7 && passport.Data.ContainsKey("cid"))
                return false;

            int birthYear;
            bool birthYearParsed = int.TryParse(passport.Data["byr"], out birthYear);

            if (!birthYearParsed || birthYear < 1920 || birthYear > 2002)
                return false;

            int issueYear;
            bool issueYearParsed = int.TryParse(passport.Data["iyr"], out issueYear);

            if (!issueYearParsed || issueYear < 2010 || issueYear > 2020)
                return false;

            int expirationYear;
            bool expirationYearParsed = int.TryParse(passport.Data["eyr"], out expirationYear);

            if (!expirationYearParsed || expirationYear < 2020 || expirationYear > 2030)
                return false;

            var heightMatch = _heightRegex.Match(passport.Data["hgt"]);
            if (!heightMatch.Success)
                return false;

            if (heightMatch.Groups["type"].Value.ToLower() == "cm")
            {
                int height;
                var heightParsed = int.TryParse(heightMatch.Groups["number"].Value, out height);

                if (!heightParsed || height < 150 || height > 193)
                    return false;
            }
            else if (heightMatch.Groups["type"].Value.ToLower() == "in")
            {
                int height;
                var heightParsed = int.TryParse(heightMatch.Groups["number"].Value, out height);

                if (!heightParsed || height < 59 || height > 76)
                    return false;
            }

            var hairColorMatch = _hairColorRegex.Match(passport.Data["hcl"]);
            if (!hairColorMatch.Success)
                return false;

            var eyeColorMatch = _eyeColorRegex.Match(passport.Data["ecl"]);
            if (!eyeColorMatch.Success)
                return false;

            var passportIdMatch = _passportIdRegex.Match(passport.Data["pid"]);
            if (!passportIdMatch.Success)
                return false;

            return true;
        }
    }
}
