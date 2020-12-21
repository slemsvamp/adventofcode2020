using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace day20
{
    public static class MapExtensions
    {
        public static bool MatchUp(this char[,] source, char[,] target)
        {
            var bottomY = target.GetLength(0) - 1;
            for (int x = 0; x < target.GetLength(1); x++)
                if (source[0, x] != target[bottomY, x])
                    return false;
            return true;
        }

        public static bool MatchDown(this char[,] source, char[,] target)
        {
            var bottomY = source.GetLength(0) - 1;
            for (int x = 0; x < target.GetLength(1); x++)
                if (source[bottomY, x] != target[0, x])
                    return false;
            return true;
        }

        public static bool MatchLeft(this char[,] source, char[,] target)
        {
            var rightX = source.GetLength(1) - 1;
            for (int y = 0; y < target.GetLength(0); y++)
                if (source[y, rightX] != target[y, 0])
                    return false;
            return true;
        }

        public static bool MatchRight(this char[,] source, char[,] target)
        {
            var rightX = target.GetLength(1) - 1;
            for (int y = 0; y < target.GetLength(0); y++)
                if (source[y, 0] != target[y, rightX])
                    return false;
            return true;
        }


        public static void Draw(this char[,] map, Point at)
        {
            for (var y = 0; y < map.GetLength(0); y++)
                for (var x = 0; x < map.GetLength(1); x++)
                {
                    Console.SetCursorPosition(at.X + x, at.Y + y);
                    Console.Write(map[y, x]);
                }
        }

        public static char[,] Copy(this char[,] source)
        {
            (int mapY, int mapX) = (source.GetLength(0), source.GetLength(1));
            var result = new char[mapY, mapX];

            for (int y = 0; y < mapY; y++)
                for (int x = 0; x < mapX; x++)
                    result[y, x] = source[y, x];
            return result;
        }

        public static char[,] Rotate(this char[,] map)
        {
            (int mapY, int mapX) = (map.GetLength(0), map.GetLength(1));
            var result = new char[mapY, mapX];

            for (int y = 0; y < mapY; y++)
                for (int x = 0; x < mapX; x++)
                    result[x, mapX - y - 1] = map[y, x];
            return result;
        }

        public static char[,] Flip(this char[,] map)
        {
            (int mapY, int mapX) = (map.GetLength(0), map.GetLength(1));

            var result = new char[mapY, mapX];

                for (int y = 0; y < mapY; y++)
                    for (int x = 0; x < mapX; x++)
                        result[y, mapX - x - 1] = map[y, x];
            return result;
        }

    }
}
