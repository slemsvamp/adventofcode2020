using System;
using System.Collections.Generic;
using System.Text;

namespace day12
{
    public class FirstStar
    {
        public static string Run(List<(Movement movement, int value)> instructions)
        {
            int east = 0;
            int north = 0;
            var facing = Facing.East;

            foreach (var instruction in instructions)
            {
                switch (instruction.movement)
                {
                    case Movement.North:
                        north += instruction.value; break;
                    case Movement.South:
                        north -= instruction.value; break;
                    case Movement.West:
                        east -= instruction.value; break;
                    case Movement.East:
                        east += instruction.value; break;
                    case Movement.Left:
                    {
                        var turns = instruction.value / 90;
                        facing = (Facing)(((int)facing + turns * 3) % 4);
                    }
                    break;
                    case Movement.Right:
                    {
                        var turns = instruction.value / 90;
                        facing = (Facing)(((int)facing + turns) % 4);
                    }
                    break;
                    case Movement.Forward:
                        switch (facing)
                        {
                            case Facing.North:
                                north += instruction.value; break;
                            case Facing.South:
                                north -= instruction.value; break;
                            case Facing.West:
                                east -= instruction.value; break;
                            case Facing.East:
                                east += instruction.value; break;
                        }
                        break;
                }
            }

            return (Math.Abs(north) + Math.Abs(east)).ToString();
        }
    }
}
