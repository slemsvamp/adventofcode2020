using System;
using System.Collections.Generic;
using System.Text;

namespace day18
{
    public class FirstStar
    {
        public static string Run(List<List<Token>> calculations)
        {
            long sum = 0;

            var tokenStack = new Stack<Token>();

            foreach (var calculation in calculations)
            {
                var output = new Stack<Token>();
                var input = new Queue<Token>();

                for (int index = 0; index < calculation.Count; index++)
                    input.Enqueue(calculation[index]);

                output.Push(input.Dequeue());

                while (output.Count > 0)
                {
                    if (input.Count == 0)
                        break;

                    Token current = input.Dequeue();

                    if (current.Type == TokenType.EndParenthesis)
                    {
                        var token = output.Pop();

                        var substack = new Stack<Token>();
                        while (token.Type != TokenType.StartParenthesis)
                        {
                            substack.Push(token);
                            token = output.Pop();
                        }

                        long number = substack.Pop().Value;
                        while (substack.Count > 0)
                        {
                            var subtoken = substack.Pop();
                            if (subtoken.Type == TokenType.Add)
                                number += substack.Pop().Value;
                            if (subtoken.Type == TokenType.Multiply)
                                number *= substack.Pop().Value;
                        }

                        output.Push(new Token { Type = TokenType.Number, Value = number });
                    }
                    else
                        output.Push(current);
                }

                var whatever = new Stack<Token>();
                while (output.Count > 0)
                    whatever.Push(output.Pop());

                long total = whatever.Pop().Value;
                while (whatever.Count > 0)
                {
                    var subtoken = whatever.Pop();
                    if (subtoken.Type == TokenType.Add)
                        total += whatever.Pop().Value;
                    if (subtoken.Type == TokenType.Multiply)
                        total *= whatever.Pop().Value;
                }
                sum += total;
            }

            return sum.ToString();
        }

        //public static long Delve(int depth, List<Token> calculation, long value, TokenType? op)
        //{
        //    Token? lookAhead = null;
        //    if (depth < calculation.Count - 1)
        //        lookAhead = calculation[depth + 1];

        //    Token current = calculation[depth];

        //    if (current.Type == TokenType.StartParenthesis)
        //        value += Delve(depth + 1, calculation, value);
        //    else if (current.Type == TokenType.EndParenthesis)
        //    {

        //    }
        //    else if (current.Type == TokenType.Add)
        //    {
        //        while 
        //    }

        //        return value;
        //}
    }
}
