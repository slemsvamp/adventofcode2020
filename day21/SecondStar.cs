using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day21
{
    public class SecondStar
    {
        public static string Run(List<Food> foods, AllergenInformation allergenInformation)
        {
            var allergens = allergenInformation.Allergens;
            var ingredients = allergenInformation.Ingredients;
            var nonAllergenIngredients = allergenInformation.NonAllergenIngredients;

            foreach (var nAI in nonAllergenIngredients)
                if (ingredients.ContainsKey(nAI))
                    ingredients.Remove(nAI);

            var allergenDictionary = new Dictionary<string, string>();
            var allergensToRemove = new HashSet<string>();
            
            while (ingredients.Count > 0)
            {
                var ingredientsToRemove = new HashSet<string>();
                foreach (var ingredient in ingredients)
                {
                    if (ingredient.Value.Count == 1)
                    {
                        var allergen = ingredient.Value.First().Key;
                        allergenDictionary.Add(allergen, ingredient.Key);
                        allergensToRemove.Add(allergen);
                    }
                    else if (ingredient.Value.Count == 0)
                    {
                        ingredientsToRemove.Add(ingredient.Key);
                    }
                }

                foreach (var ingredientToRemove in ingredientsToRemove)
                    ingredients.Remove(ingredientToRemove);

                foreach (var allergenToRemove in allergensToRemove)
                {
                    foreach (var ingredient in ingredients)
                    {
                        if (ingredient.Value.ContainsKey(allergenToRemove))
                            ingredient.Value.Remove(allergenToRemove);
                    }
                    ingredients.Remove(allergenToRemove);
                }

                allergensToRemove.Clear();
            }

            var alphabeticOrder = allergenDictionary.Keys.OrderBy(a => a);

            var finalList = new List<string>();

            foreach (var alphabeticItem in alphabeticOrder)
                finalList.Add(allergenDictionary[alphabeticItem]);

            return string.Join(",", finalList);
        }
    }
}
