using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace day01
{
    public class FirstStar
    {
        public static string Run(List<int> parameter)
        {
            for (int firstNumberIndex = 0; firstNumberIndex < parameter.Count; firstNumberIndex++)
            {
                for (int secondNumberIndex = 0; secondNumberIndex < parameter.Count; secondNumberIndex++)
                {
                    var firstNumber = parameter[firstNumberIndex];
                    var secondNumber = parameter[secondNumberIndex];

                    if (firstNumber + secondNumber == 2020)
                    {
                        var product = firstNumber * secondNumber;
                        return product.ToString();
                    }
                }
            }
            return string.Empty;
        }
    }
}
