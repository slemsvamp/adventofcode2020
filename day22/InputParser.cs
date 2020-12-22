using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace day22
{
    public class Player
    {
        public int Id;
        public LinkedList<int> Cards;
    }

    public class InputParser
    {
        internal static List<Player> Parse(string filename)
        {
            string[] lines = File.ReadAllLines(filename);

            var players = new List<Player>();
            var player = new Player();
            var cards = new List<int>();

            foreach (var line in lines)
            {
                if(line.StartsWith("Player"))
                    player.Id = int.Parse(line.Replace("Player ", "").Trim(':'));
                else if (line == string.Empty)
                {
                    player.Cards = new LinkedList<int>(cards);
                    players.Add(player);
                    cards = new List<int>();
                    player = new Player();
                }
                else
                    cards.Add(int.Parse(line));
            }

            player.Cards = new LinkedList<int>(cards);
            players.Add(player);

            return players;
        }
    }
}
