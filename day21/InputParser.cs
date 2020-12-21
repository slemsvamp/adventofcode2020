using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day21
{
    public struct Food
    {
        public string[] Contains;
        public string[] Ingredients;
    }

    public class InputParser
    {
        internal static List<Food> Parse(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            var foods = new List<Food>();

            foreach (var line in lines)
            {
                int index = line.IndexOf('(');
                var food = new Food
                {
                    Contains = line.Substring(index).Replace("(contains ", "").Trim(')').Split(new[] { ", " }, StringSplitOptions.None),
                    Ingredients = line.Substring(0, index - 1).Split(' ')
                };
                foods.Add(food);
            }

            return foods;
        }

    }
}
