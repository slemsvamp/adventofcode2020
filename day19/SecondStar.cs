using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day19
{
    public class SecondStar
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
            var startRule = _rules[0];
            int at = 0;

            var validation = new List<(bool valid, int at)>();

            // Story time.

            // I could refactor to make this a lot faster by returning
            // a more detailed answer than "bool", like "did we pass the
            // message length" and such to determine if we want to continue
            // or not. This solution has hardcoded rulecounts after I solved
            // it, just to make it a bit speedier.

            // Technically you could replace the message.Length / 8 with
            // something smarter, but the "depth" of the rules are 8 characters.

            // Like the problem said:
            // "(Remember, you only need to handle the rules you have;
            // building a solution that could handle any hypothetical
            // combination of rules would be significantly more difficult.)"

            // I've decided that I just solve this specific case, and not
            // worry about making something that solves every possible message.

            for (int rule8Count = 1; rule8Count < message.Length / 8; rule8Count++)
                for (int rule11Count = 1; rule11Count < message.Length / 8; rule11Count++)
                {
                    var invalid8 = SubRule8(message, rule8Count, ref at);
                    
                    if (at == 0) continue;
                    
                    var invalid11 = SubRule11(message, rule11Count, ref at);

                    bool valid = !invalid8 && !invalid11 && message.Length == at;

                    validation.Add((valid, at));

                    at = 0;
                }

            return validation.Any(v => v.valid);
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
                    bool invalid = false;
                    invalid = ValidateWithRule(_rules[subRule], message, ref atTemp);
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
                    bool invalid = false;
                    invalid = ValidateWithRule(_rules[subRule], message, ref atTemp);
                    validationA.Add((!invalid, atTemp));
                }

                atTemp = at;
                foreach (var subRule in orRule.B)
                {
                    bool invalid = false;
                    invalid = ValidateWithRule(_rules[subRule], message, ref atTemp);
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

        private static bool SubRule8(string message, int times, ref int at)
        {
            bool failedValidation = false;
            int atTemp = at;

            for (int timesIndex = 0; timesIndex < times; timesIndex++)
                failedValidation |= ValidateWithRule(_rules[42], message, ref atTemp);

            if (!failedValidation)
                at = atTemp;

            return failedValidation;
        }

        private static bool SubRule11(string message, int times, ref int at)
        {
            bool failedValidation = false;
            int atTemp = at;

            for (int timesIndex = 0; timesIndex < times; timesIndex++)
                failedValidation |= ValidateWithRule(_rules[42], message, ref atTemp);

            for (int timesIndex = 0; timesIndex < times; timesIndex++)
                failedValidation |= ValidateWithRule(_rules[31], message, ref atTemp);

            if (!failedValidation)
                at = atTemp;

            return failedValidation;
        }
    }
}
