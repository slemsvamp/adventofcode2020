using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace day01
{
    public class SecondStar
    {
        public static string Run(List<int> parameter)
        {
            for (int firstNumberIndex = 0; firstNumberIndex < parameter.Count; firstNumberIndex++)
            {
                for (int secondNumberIndex = 0; secondNumberIndex < parameter.Count; secondNumberIndex++)
                {
                    for (int thirdNumberIndex = 0; thirdNumberIndex < parameter.Count; thirdNumberIndex++)
                    {
                        var firstNumber = parameter[firstNumberIndex];
                        var secondNumber = parameter[secondNumberIndex];
                        var thirdNumber = parameter[thirdNumberIndex];

                        if (firstNumber + secondNumber + thirdNumber == 2020)
                        {
                            var product = firstNumber * secondNumber * thirdNumber;
                            return product.ToString();
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
