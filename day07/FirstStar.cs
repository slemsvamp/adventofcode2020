using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace day07
{
    public class FirstStar
    {
        public static string Run(List<LuggageDescription> luggageDescriptions)
        {
            var descriptions = new HashSet<string>();

            foreach (var luggage in RecursiveSearch("shiny gold", luggageDescriptions))
            {
                if (!descriptions.Contains(luggage.ContaineeColor))
                    descriptions.Add(luggage.ContaineeColor);
                if (!descriptions.Contains(luggage.ContainerColor))
                    descriptions.Add(luggage.ContainerColor);
            }

            if (descriptions.Contains("shiny gold"))
                descriptions.Remove("shiny gold");

            var uniqueDescriptions = descriptions.Count();

            return $"{uniqueDescriptions}";
        }

        public static List<Luggage> RecursiveSearch(string color, List<LuggageDescription> luggageDescriptions)
        {
            var result = new List<Luggage>();
            foreach (var container in luggageDescriptions)
            {
                foreach (var containee in container.Contains)
                {
                    if (containee.Color == color)
                    {
                        result.Add(new Luggage
                        {
                            ContainerColor = container.Color,
                            ContaineeColor = containee.Color,
                            ContaineeQuantity = containee.Quantity
                        });

                        var luggageResult = RecursiveSearch(container.Color, luggageDescriptions);

                        foreach (var luggage in luggageResult)
                            result.Add(luggage);
                    }
                }
            }
            return result;
        }
    }
}
