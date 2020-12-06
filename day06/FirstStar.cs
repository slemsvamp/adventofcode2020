using System;
using System.Collections.Generic;
using System.Text;

namespace day06
{
    public class FirstStar
    {
        public static string Run(List<GroupAnswers> groupAnswers)
        {
            var distinctGroupAnswers = 0;
            foreach (var group in groupAnswers)
            {
                var distinctLetters = new HashSet<char>();
                foreach (var answers in group.Answers)
                {
                    foreach (var answer in answers)
                    {
                        if (!distinctLetters.Contains(answer))
                            distinctLetters.Add(answer);
                    }
                }
                distinctGroupAnswers += distinctLetters.Count;
            }

            return distinctGroupAnswers.ToString();
        }
    }
}
