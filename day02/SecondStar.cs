using System;
using System.Collections.Generic;
using System.Text;

namespace day02
{
    public class SecondStar
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
            bool first = passwordCheck.Password.Length >= passwordCheck.First &&
                passwordCheck.Password[passwordCheck.First - 1].ToString() == passwordCheck.Letter;
            
            bool second = passwordCheck.Password.Length >= passwordCheck.Second &&
                passwordCheck.Password[passwordCheck.Second - 1].ToString() == passwordCheck.Letter;

            return !(first && second) && (first || second);
        }
    }
}
