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
            var rightX = target.GetLength(1) - 1;
            for (int y = 0; y < target.GetLength(0); y++)
                if (source[y, 0] != target[y, rightX])
                    return false;
            return true;
        }

        public static bool MatchRight(this char[,] source, char[,] target)
        {
            var rightX = source.GetLength(1) - 1;
            for (int y = 0; y < target.GetLength(0); y++)
                if (source[y, rightX] != target[y, 0])
                    return false;
            return true;
        }

        public static void Draw(this char[,] source, Point at)
        {
            for (var y = 0; y < source.GetLength(0); y++)
                for (var x = 0; x < source.GetLength(1); x++)
                {
                    Console.SetCursorPosition(at.X + x, at.Y + y);
                    Console.Write(source[y, x]);
                }
        }

        public static char[,] Copy(this char[,] source)
        {
            (int maxY, int maxX) = (source.GetLength(0), source.GetLength(1));
            var result = new char[maxY, maxX];

            for (int y = 0; y < maxY; y++)
                for (int x = 0; x < maxX; x++)
                    result[y, x] = source[y, x];
            return result;
        }

        public static char[,] Rotate(this char[,] source)
        {
            (int maxY, int maxX) = (source.GetLength(0), source.GetLength(1));
            var result = new char[maxY, maxX];

            for (int y = 0; y < maxY; y++)
                for (int x = 0; x < maxX; x++)
                    result[x, maxX - y - 1] = source[y, x];
            return result;
        }

        public static char[,] Flip(this char[,] source)
        {
            (int maxY, int maxX) = (source.GetLength(0), source.GetLength(1));

            var result = new char[maxY, maxX];
                for (int y = 0; y < maxY; y++)
                    for (int x = 0; x < maxX; x++)
                        result[y, maxX - x - 1] = source[y, x];
            return result;
        }

    }
}
