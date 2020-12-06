using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;

namespace day06
{
    public class SecondStar
    {
        public static string Run(List<GroupAnswers> groupAnswers)
        {
            var answeredByAllInGroup = 0;

            foreach (var group in groupAnswers)
            {
                foreach (var answers in group.Answers)
                {
                    foreach (var answer in answers)
                    {
                        if (!group.Answered.ContainsKey(answer))
                            group.Answered[answer] = 0;
                        group.Answered[answer] += 1;
                    }
                }

                foreach (var answers in group.Answered)
                    if (answers.Value == group.Answers.Count)
                        answeredByAllInGroup++;
            }

            return answeredByAllInGroup.ToString();
        }
    }
}
