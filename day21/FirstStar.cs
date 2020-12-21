using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day21
{
    public class FirstStar
    {
        public static (string Text, AllergenInformation AllergenInformation) Run(List<Food> foods)
        {
            var allergens = new Dictionary<string, int>();
            var ingredients = new Dictionary<string, Dictionary<string, int>>();
            var nonAllergenIngredients = new HashSet<string>();

            foreach (var food in foods)
            {
                foreach (var ingredient in food.Ingredients)
                {
                    nonAllergenIngredients.Add(ingredient);

                    foreach (var allergen in food.Contains)
                    {
                        if (!ingredients.ContainsKey(ingredient))
                            ingredients.Add(ingredient, new Dictionary<string, int>());

                        if (!ingredients[ingredient].ContainsKey(allergen))
                            ingredients[ingredient].Add(allergen, 0);

                        ingredients[ingredient][allergen]++;
                    }
                }

                foreach (var allergen in food.Contains)
                {
                    if (!allergens.ContainsKey(allergen))
                        allergens.Add(allergen, 0);
                    allergens[allergen]++;
                }
            }

            foreach (var ingredient in ingredients)
                foreach (var ingredientAllergen in ingredient.Value)
                {
                    if (allergens.ContainsKey(ingredientAllergen.Key))
                        if (ingredientAllergen.Value >= allergens[ingredientAllergen.Key])
                            nonAllergenIngredients.Remove(ingredient.Key);
                        else
                            ingredients[ingredient.Key].Remove(ingredientAllergen.Key);
                }

            int count = 0;
            foreach (var food in foods)
            {
                var foodSet = new HashSet<string>(food.Ingredients);
                foreach (var nAI in nonAllergenIngredients)
                    if (foodSet.Contains(nAI))
                        count++;
            }

            return (count.ToString(), new AllergenInformation
            {
                Allergens = allergens,
                Ingredients = ingredients,
                NonAllergenIngredients = nonAllergenIngredients
            });
        }
    }
}