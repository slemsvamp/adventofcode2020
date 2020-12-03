using System;
using System.Collections.Generic;
using System.Text;

namespace day03
{
    public class SecondStar
    {
        public struct Vector
        {
            public int X;
            public int Y;
            
            public Vector(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        public static string Run(string[] mapSource)
        {
            var map = new string[mapSource.Length];

            Array.Copy(mapSource, map, mapSource.Length);

            var vectors = new List<Vector>()
            {
                new Vector(1, 1),
                new Vector(3, 1),
                new Vector(5, 1),
                new Vector(7, 1),
                new Vector(1, 2)
            };

            int x, y, treeProduct;
            var treeFactors = new List<int>();

            foreach (var vector in vectors)
            {
                int trees = 0;
                x = y = 0;

                while (true)
                {
                    x += vector.X;
                    y += vector.Y;

                    if (y >= mapSource.Length)
                        break;

                    x = x % map[y].Length;

                    var isTree = map[y][x].ToString() == "#";

                    if (isTree)
                        trees += 1;
                }

                treeFactors.Add(trees);
            }

            treeProduct = treeFactors[0];

            for (int treeFactorIndex = 1; treeFactorIndex < treeFactors.Count; treeFactorIndex++)
                treeProduct *= treeFactors[treeFactorIndex];

            return treeProduct.ToString();
        }
    }
}
