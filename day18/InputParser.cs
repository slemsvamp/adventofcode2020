using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day18
{
    public enum TokenType
    {
        Add, Multiply, Number,
        StartParenthesis, EndParenthesis,
    }

    public struct Token
    {
        public TokenType Type;
        public long Value;
    }

    public class InputParser
    {
        internal static List<List<Token>> Parse(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            var calculations = new List<List<Token>>();

            foreach (var line in lines)
            {
                var calculation = new List<Token>();

                foreach (var character in line)
                {
                    if (character == ' ')
                        continue;
                    else if (character == '+')
                        calculation.Add(new Token { Type = TokenType.Add });
                    else if (character == '*')
                        calculation.Add(new Token { Type = TokenType.Multiply });
                    else if (character == '(')
                        calculation.Add(new Token { Type = TokenType.StartParenthesis });
                    else if (character == ')')
                        calculation.Add(new Token { Type = TokenType.EndParenthesis });
                    else
                        calculation.Add(new Token { Type = TokenType.Number, Value = int.Parse(character.ToString()) });
                }

                calculations.Add(calculation);
            }

            return calculations;
        }
    }
}
