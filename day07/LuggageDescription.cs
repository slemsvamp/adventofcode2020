using System;
using System.Collections.Generic;
using System.Text;

namespace day07
{
    public class LuggageDescription
    {
        public int Quantity;
        public string Color;
        public List<LuggageDescription> Contains;

        public LuggageDescription(int number, string description)
        {
            Quantity = number;
            Color = description;
            Contains = new List<LuggageDescription>();
        }
    }
}
