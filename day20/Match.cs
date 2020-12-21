using System;
using System.Collections.Generic;
using System.Text;

namespace day20
{
    public class Match
    {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;

        public Match(bool up, bool down, bool left, bool right)
            => (Up, Down, Left, Right) = (up, down, left, right);

        public static Match Combine(Match source, List<MatchInfo> matches)
        {
            var result = new Match(source.Up, source.Down, source.Left, source.Right);
            foreach (var match in matches)
            {
                switch (match.Direction)
                {
                    case Direction.Up: result.Up = true; break;
                    case Direction.Down: result.Down = true; break;
                    case Direction.Left: result.Left = true; break;
                    case Direction.Right: result.Right = true; break;
                }
            }
            return result;
        }

        public static Match Combine(Match m1, Match m2)
            => new Match(m1.Up | m2.Up, m1.Down | m2.Down, m1.Left | m2.Left, m1.Right | m2.Right);

        public static Match Default(bool all)
            => new Match(all, all, all, all);
    }
}
