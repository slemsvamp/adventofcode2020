using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day24
{
    public class InputParser
    {
        internal static string[] Parse(string filename)
            => File.ReadAllLines(filename);
    }
}
