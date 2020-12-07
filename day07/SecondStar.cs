using System;
using System.Collections.Generic;
using System.Text;

namespace day07
{
    public class SecondStar
    {
        public static string Run(List<LuggageDescription> luggageDescriptions)
        {
            var result = Search(1, "shiny gold", luggageDescriptions);

            var sum = 0;
            foreach (var luggage in result)
                sum += RecursiveCount(luggage);

            return sum.ToString();
        }

        public static List<LuggageDescription> Search(int quantity, string color, List<LuggageDescription> luggageDescriptions)
        {
            var result = new List<LuggageDescription>();
            foreach (var container in luggageDescriptions)
            {
                if (container.Color == color)
                {
                    var luggage = new LuggageDescription(quantity, container.Color);
                    
                    foreach (var containee in container.Contains)
                    {
                        var contains = Search(containee.Quantity, containee.Color, luggageDescriptions);

                        foreach (var contain in contains)
                            luggage.Contains.Add(contain);
                    }

                    result.Add(luggage);
                }
            }
            return result;
        }

        public static int RecursiveCount(LuggageDescription luggage)
        {
            int sum = 0;
            foreach (var containee in luggage.Contains)
                sum += containee.Quantity + (containee.Quantity * RecursiveCount(containee));

            return sum;
        }
    }
}
