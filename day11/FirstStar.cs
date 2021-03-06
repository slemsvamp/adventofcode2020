using System;
using System.Collections.Generic;
using System.Text;

namespace day11
{
    public class FirstStar
    {
        public static int[] _dirX = new[] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
        public static int[] _dirY = new[] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };

        public static string Run(string[] input)
        {
            var height = input.Length;
            var width = input[0].Length;

            char[,] seatings = new char[width, height];
            var newSeatings = new char[width, height];

            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    seatings[x, y] = input[y][x];

            while (true)
            {
                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        if (x < 0 || x >= width || y < 0 || y >= height)
                            continue;

                        int emptySeatCount = 0;
                        int occupiedSeatCount = 0;

                        for (int direction = 0; direction < 9; direction++)
                        {
                            var checkX = x + _dirX[direction];
                            var checkY = y + _dirY[direction];
                            if (checkX < 0 || checkX >= width || checkY < 0 || checkY >= height)
                                continue;

                            if (seatings[checkX, checkY] == 'L')
                                emptySeatCount++;

                            if (seatings[checkX, checkY] == '#')
                                occupiedSeatCount++;

                            if (seatings[x, y] == 'L' && occupiedSeatCount == 0)
                                newSeatings[x, y] = '#';
                            else if (seatings[x, y] == '#' && occupiedSeatCount > 4)
                                newSeatings[x, y] = 'L';
                            else
                                newSeatings[x, y] = seatings[x, y];
                        }
                    }

                int changes = 0;

                for (int y = 0; y < height; y++)
                    for (int x = 0; x < width; x++)
                    {
                        if (seatings[x, y] != newSeatings[x, y])
                            changes++;
                        seatings[x, y] = newSeatings[x, y];
                    }

                if (changes == 0)
                    break;
            }

            int result = 0;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    if (seatings[x, y] == '#')
                        result++;

            return result.ToString();
        }
    }
}
