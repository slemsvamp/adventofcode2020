using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace day01
{
    public class SecondStar
    {
        public static Result Run(List<int> parameter)
        {
            for (int i = 0; i < parameter.Count; i++)
            {
                for (int y = 0; y < parameter.Count; y++)
                {
                    for (int z = 0; z < parameter.Count; z++)
                    {
                        if (parameter[i] + parameter[y] + parameter[z] == 2020)
                        {
                            return new Result
                            {
                                Data = parameter[i] * parameter[y] * parameter[z],
                                Text = (parameter[i] * parameter[y] * parameter[z]).ToString()
                            };
                        }
                    }
                }
            }
            return new Result();
        }
    }
}
