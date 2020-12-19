using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day19
{
    public class FirstStar
    {
        private static Dictionary<int, IBaseRule> _rules;

        public static string Run(MonsterMessages monsterMessages)
        {
            _rules = monsterMessages.Rules;

            var validMessages = 0;
            foreach (var message in monsterMessages.Messages)
                validMessages += ValidMessage(message) ? 1 : 0;

            return validMessages.ToString();
        }

        private static bool ValidMessage(string message)
        {
            bool failedValidation;

            var startRule = _rules[0];
            int startAt = 0;

            failedValidation = ValidateWithRule(startRule, message, ref startAt);

            if (message.Length != startAt)
                failedValidation = true;

            return !failedValidation;
        }

        private static bool ValidateWithRule(IBaseRule baseRule, string message, ref int at)
        {
            bool failedValidation = false;

            if (baseRule is Rule rule)
            {
                if (at >= message.Length)
                    failedValidation = true;
                else
                    failedValidation = message[at++] != rule.Text[0];
            }
            else if (baseRule is OneRule oneRule)
            {
                var validation = new List<(bool valid, int at)>();

                var atTemp = at;
                foreach (var subRule in oneRule.One)
                {
                    bool invalid = ValidateWithRule(_rules[subRule], message, ref atTemp);
                    validation.Add((!invalid, atTemp));
                }

                var valid = validation.All(v => v.valid);

                failedValidation = !valid;

                if (valid)
                    at = validation.Max(v => v.at);
            }
            else if (baseRule is OrRule orRule)
            {
                var validationA = new List<(bool valid, int at)>();
                var validationB = new List<(bool valid, int at)>();

                int atTemp = at;
                foreach (var subRule in orRule.A)
                {
                    bool invalid = ValidateWithRule(_rules[subRule], message, ref atTemp);
                    validationA.Add((!invalid, atTemp));
                }

                atTemp = at;
                foreach (var subRule in orRule.B)
                {
                    bool invalid = ValidateWithRule(_rules[subRule], message, ref atTemp);
                    validationB.Add((!invalid, atTemp));
                }

                var validA = validationA.All(v => v.valid);
                var validB = validationB.All(v => v.valid);

                failedValidation = !validA && !validB;

                if (failedValidation == false)
                {
                    if (validA)
                        at = validationA.Max(v => v.at);
                    else if (validB)
                        at = validationB.Max(v => v.at);
                }
            }

            return failedValidation;
        }
    }
}
