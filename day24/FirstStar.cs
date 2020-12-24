using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day24
{
    public class FirstStar
    {
        private const int MAX_WIDTH = 1000;

        public static (string Result, Dictionary<int, Hex> Hexes) Run(string[] tiles)
        {
            Dictionary<int, Hex> hexes = new Dictionary<int, Hex>();

            int startX = 100;
            int startY = 100;
            int startIndex = startY * MAX_WIDTH + startX;

            hexes.Add(startIndex, new Hex(startIndex));

            foreach (var tile in tiles)
            {
                int y = startY;
                int x = startX;

                for (int tileIndex = 0; tileIndex < tile.Length; tileIndex++)
                {
                    var direction = Direction.None;

                    char tileInstruction = tile[tileIndex];
                    char nextTileInstruction = '\0';

                    if (tileIndex < tile.Length - 1)
                        nextTileInstruction = tile[tileIndex + 1];

                    if (tileInstruction == 'n' && nextTileInstruction == 'e')
                    {
                        direction = Direction.NorthEast;
                        tileIndex++;
                    }
                    else if (tileInstruction == 'n' && nextTileInstruction == 'w')
                    {
                        direction = Direction.NorthWest;
                        tileIndex++;
                    }

                    if (tileInstruction == 's' && nextTileInstruction == 'e')
                    {
                        direction = Direction.SouthEast;
                        tileIndex++;
                    }
                    else if (tileInstruction == 's' && nextTileInstruction == 'w')
                    {
                        direction = Direction.SouthWest;
                        tileIndex++;
                    }

                    if (tileInstruction == 'w')
                        direction = Direction.West;
                    else if (tileInstruction == 'e')
                        direction = Direction.East;

                    switch (direction)
                    {
                        case Direction.NorthWest:
                            y -= 1;
                            x -= 1;
                            break;
                        case Direction.NorthEast:
                            y -= 1;
                            x += 1;
                            break;
                        case Direction.SouthWest:
                            y += 1;
                            x -= 1;
                            break;
                        case Direction.SouthEast:
                            y += 1;
                            x += 1;
                            break;
                        case Direction.West:
                            x -= 2;
                            break;
                        case Direction.East:
                            x += 2;
                            break;
                    }
                }

                int index = y * MAX_WIDTH + x;

                if (!hexes.ContainsKey(index))
                    hexes.Add(index, new Hex(index));
                hexes[index].White = !hexes[index].White;
            }

            return (hexes.Values.Count(hex => !hex.White).ToString(), hexes);
        }
    }
}
