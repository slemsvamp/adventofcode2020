using System;
using System.Collections.Generic;
using System.Text;

namespace day12
{
    public class SecondStar
    {
        public static string Run(List<(Movement movement, int value)> instructions)
        {
            int east = 0;
            int north = 0;
            int waypointEast = 10;
            int waypointNorth = 1;

            foreach (var instruction in instructions)
            {
                switch (instruction.movement)
                {
                    case Movement.North:
                        waypointNorth += instruction.value; break;
                    case Movement.South:
                        waypointNorth -= instruction.value; break;
                    case Movement.West:
                        waypointEast -= instruction.value; break;
                    case Movement.East:
                        waypointEast += instruction.value; break;
                    case Movement.Left:
                    {
                        for (int turn = 0; turn < instruction.value / 90; turn++)
                            (waypointEast, waypointNorth) = (-waypointNorth, waypointEast);
                    }
                    break;
                    case Movement.Right:
                    {
                        for (int turn = 0; turn < instruction.value / 90; turn++)
                            (waypointEast, waypointNorth) = (waypointNorth, -waypointEast);
                    }
                    break;
                    case Movement.Forward:
                    {
                        north += instruction.value * waypointNorth;
                        east += instruction.value * waypointEast;
                    }
                    break;
                }
            }

            return (Math.Abs(north) + Math.Abs(east)).ToString();
        }
    }
}
