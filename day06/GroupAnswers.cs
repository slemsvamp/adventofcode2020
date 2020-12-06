using System;
using System.Collections.Generic;
using System.Text;

namespace day06
{
    public class GroupAnswers
    {
        public List<string> Answers;
        public Dictionary<char, int> Answered;
        public GroupAnswers()
        {
            Answers = new List<string>();
            Answered = new Dictionary<char, int>();
        }
    }
}
