using System;
using System.Collections.Generic;
using System.Text;

namespace day04
{
    public class FirstStar
    {
        public static string Run(List<Passport> passports)
        {
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
            if (passport.Data.Count == 8 || (passport.Data.Count == 7 && !passport.Data.ContainsKey("cid")))
                return true;

            return false;
        }
    }
}
