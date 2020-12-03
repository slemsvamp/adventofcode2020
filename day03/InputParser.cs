using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day03
{
    public class InputParser
    {
        internal static string[] Parse(string filename)
        {
            return File.ReadAllLines(filename);
        }
    }
}
