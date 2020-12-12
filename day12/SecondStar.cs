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
            
            var facing = Facing.East;
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
                        var turns = instruction.value / 90;

                        for (int turn = 0; turn < turns; turn++)
                        {
                            var newFacing = (Facing)(((int)facing + 4 - 1) % 4);

                            switch (newFacing)
                            {
                                case Facing.East:
                                case Facing.North:
                                {
                                    var save = waypointNorth;
                                    waypointNorth = waypointEast;
                                    waypointEast = -save;
                                }
                                break;
                                case Facing.West:
                                case Facing.South:
                                {
                                    var save = waypointEast;
                                    waypointEast = -waypointNorth;
                                    waypointNorth = save;
                                }
                                break;
                            }

                            facing = newFacing;
                        }
                    }
                    break;
                    case Movement.Right:
                    {
                        var turns = instruction.value / 90;

                        for (int turn = 0; turn < turns; turn++)
                        {
                            var newFacing = (Facing)(((int)facing + 1) % 4);

                            switch (newFacing)
                            {
                                case Facing.East:
                                case Facing.North:
                                {
                                    var save = waypointEast;
                                    waypointEast = waypointNorth;
                                    waypointNorth = -save;
                                }
                                break;
                                case Facing.West:
                                case Facing.South:
                                {
                                    var save = waypointNorth;
                                    waypointNorth = -waypointEast;
                                    waypointEast = save;
                                }
                                break;
                            }

                            facing = newFacing;
                        }
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
