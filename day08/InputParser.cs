using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace day08
{
    public class InputParser
    {
        internal static List<Operation> Parse(string filename)
        {
            return File.ReadAllLines(filename)
                .Select(line => Operator.Parse(line))
                .ToList();
        }
    }
}
