using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day11
{
    public class InputParser
    {
        internal static string[] Parse(string filename)
            => File.ReadAllLines(filename);
    }
}
