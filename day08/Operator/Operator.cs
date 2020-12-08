using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace day08
{
    public class Operator
    {
        public static Regex OperationRegex
            = new Regex(@"^(?<operation>acc|jmp|nop) (?<value>[+|-][\d]+)$");

        private List<Operation> _operations;

        public Operator(List<Operation> operations)
            => _operations = operations;

        public static Operation Parse(string operationLine)
        {
            var regexResult = OperationRegex.Match(operationLine);
            return new Operation
            {
                Type = (OperationType)Enum.Parse(typeof(OperationType), regexResult.Groups["operation"].Value, true),
                Value = int.Parse(regexResult.Groups["value"].Value)
            };
        }

        public OperatorResult Run()
        {
            var accumulator = 0;
            var positions = new HashSet<int>();
            for (int position = 0; position < _operations.Count; position++)
            {
                if (positions.Contains(position))
                    return new OperatorResult
                    {
                        EndedOnRecurrence = true,
                        Accumulator = accumulator
                    };

                positions.Add(position);

                var operation = _operations[position];

                switch (operation.Type)
                {
                    case OperationType.Nop:
                        break;
                    case OperationType.Acc:
                        accumulator += operation.Value;
                        break;
                    case OperationType.Jmp:
                        position += operation.Value - 1;
                        break;
                }
            }

            return new OperatorResult
            {
                EndedOnRecurrence = false,
                Accumulator = accumulator
            };
        }
    }
}
