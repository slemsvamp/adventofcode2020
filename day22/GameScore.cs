using System;
using System.Collections.Generic;
using System.Text;

namespace day22
{
    public struct GameScore
    {
        public long Score;
        public int WinnerIndex;

        public static GameScore NoWinner
        {
            get
            {
                return new GameScore
                {
                    Score = -1,
                    WinnerIndex = -1
                };
            }
        }
    }
}
