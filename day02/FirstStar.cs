using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace day02
{
    public class FirstStar
    {
        public static string Run(List<PasswordCheck> passwordChecks)
        {
            int validCount = 0;

            foreach (var passwordCheck in passwordChecks)
                if (IsValid(passwordCheck))
                    validCount++;

            return validCount.ToString();
        }

        private static bool IsValid(PasswordCheck passwordCheck)
        {
            int occurrences = passwordCheck.Password.Count(letter => letter.ToString() == passwordCheck.Letter);
            return occurrences >= passwordCheck.First && occurrences <= passwordCheck.Second;
        }
    }
}
