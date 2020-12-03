using System;
using System.Collections.Generic;
using System.Text;

namespace day03
{
    public class FirstStar
    {
        public static string Run(string[] mapSource)
        {
            var map = new string[mapSource.Length];

            Array.Copy(mapSource, map, mapSource.Length);

            int x = 0;
            int y = 0;

            int trees = 0;

            while (true)
            {
                x += 3;
                y += 1;

                if (y >= mapSource.Length)
                    break;

                if (x >= map[y].Length)
                    Extend(mapSource, map);

                var isTree = map[y][x].ToString() == "#";

                if (isTree)
                    trees += 1;
            }


            return trees.ToString();
        }

        public static void Extend(string[] mapSource, string[] map)
        {
            for (int mapSourceIndex = 0; mapSourceIndex < mapSource.Length; mapSourceIndex++)
                map[mapSourceIndex] += mapSource[mapSourceIndex];
        }
    }
}
