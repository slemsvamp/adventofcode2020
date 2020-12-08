using System;
using System.Collections.Generic;
using System.Text;

namespace day08
{
    public class SecondStar
    {
        public static string Run(List<Operation> operations)
        {
            for (int change = 0; change < operations.Count; change++)
            {
                var changedOperations = new List<Operation>(operations);

                if (operations[change].Type == OperationType.Nop && operations[change].Value != 0)
                {
                    changedOperations[change] = new Operation
                    {
                        Type = OperationType.Jmp,
                        Value = operations[change].Value
                    };

                    var @operator = new Operator(changedOperations);
                    var result = @operator.Run();

                    if (result.EndedOnRecurrence == false)
                        return result.Accumulator.ToString();
                }
                else if (operations[change].Type == OperationType.Jmp)
                {
                    changedOperations[change] = new Operation
                    {
                        Type = OperationType.Nop
                    };

                    var @operator = new Operator(changedOperations);
                    var result = @operator.Run();

                    if (result.EndedOnRecurrence == false)
                        return result.Accumulator.ToString();
                }
            }

            return "Failed";
        }
    }
}
