using System;
using System.Collections.Generic;
using System.Text;

namespace day21
{
    public class AllergenInformation
    {
        public Dictionary<string, int> Allergens;
        public Dictionary<string, Dictionary<string, int>> Ingredients;
        public HashSet<string> NonAllergenIngredients;
    }
}
