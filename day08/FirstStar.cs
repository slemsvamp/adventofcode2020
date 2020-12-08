using System;
using System.Collections.Generic;
using System.Text;

namespace day08
{
    public class FirstStar
    {
        public static string Run(List<Operation> operations)
        {
            var @operator = new Operator(operations);
            var result = @operator.Run();

            return result.Accumulator.ToString();
        }
    }
}
