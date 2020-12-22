using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day22
{
    public class FirstStar
    {
        private static List<Player> _players;

        public static string Run(List<Player> players)
        {
            _players = Clone(players);

            int round = 0;
            bool playing = true;
            LinkedList<int> winningDeck = null;

            while (playing)
            {
                var player1Card = _players[0].Cards.First;
                var player2Card = _players[1].Cards.First;

                if (player1Card.Value > player2Card.Value)
                    WonRound(0);
                else 
                    WonRound(1);

                foreach (var player in _players)
                    if (player.Cards.Count == 0)
                    {
                        playing = false;
                        if (player.Id == 1)
                            winningDeck = _players[1].Cards;
                        else
                            winningDeck = _players[0].Cards;
                    }

                round++;
            }

            long score = CalculateScore(winningDeck);

            return score.ToString();
        }

        public static List<Player> Clone(List<Player> players)
        {
            var result = new List<Player>();

            foreach (var player in players)
            {
                var playerNext = new Player();
                playerNext.Id = player.Id;
                playerNext.Cards = new LinkedList<int>();

                foreach (var card in player.Cards)
                    playerNext.Cards.AddLast(card);

                result.Add(playerNext);
            }

            return result;
        }

        public static long CalculateScore(LinkedList<int> deck)
        {
            long total = 0;
            var deckArray = deck.ToArray();
            for (int multiplier = deck.Count; multiplier > 0; multiplier--)
                total += deckArray[deck.Count - multiplier] * multiplier;
            return total;
        }

        public static void WonRound(int winnerIndex)
        {
            var winnerDeck = _players[winnerIndex].Cards;
            var loserDeck = _players[winnerIndex == 0 ? 1 : 0].Cards;

            var firstCardWinner = winnerDeck.First;
            winnerDeck.RemoveFirst();
            winnerDeck.AddLast(firstCardWinner);
            var firstCardLoser = loserDeck.First;
            loserDeck.RemoveFirst();
            winnerDeck.AddLast(firstCardLoser);
        }
    }
}
