using System;
using System.Collections.Generic;
using System.Text;

namespace day18
{
    public class SecondStar
    {
        public static string Run(List<List<Token>> calculations)
        {
            long sum = 0;

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

                        if (output.Count > 0 && output.Peek().Type == TokenType.Add)
                        {
                            output.Pop();
                            output.Push(new Token { Type = TokenType.Number, Value = number + output.Pop().Value });
                        }
                        else
                            output.Push(new Token { Type = TokenType.Number, Value = number });
                    }
                    else if (current.Type == TokenType.Add)
                    {
                        if (input.Peek().Type == TokenType.Number)
                        {
                            long newnumber = output.Pop().Value;
                            newnumber += input.Dequeue().Value;
                            output.Push(new Token { Type = TokenType.Number, Value = newnumber });
                        }
                        else
                        {
                            output.Push(current);
                        }
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
    }
}
