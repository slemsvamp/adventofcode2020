using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace day01
{
    public class FirstStar
    {
        public static Result Run(List<int> parameter)
        {
            for (int i = 0; i < parameter.Count; i++)
            {
                for (int y = 0; y < parameter.Count; y++)
                {
                    if (parameter[i] + parameter[y] == 2020)
                    {
                        return new Result
                        {
                            Data = parameter[i] * parameter[y],
                            Text = (parameter[i] * parameter[y]).ToString()
                        };
                    }
                }
            }
            return new Result();
        }
    }
}
