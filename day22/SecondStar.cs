using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day22
{
    public class SecondStar
    {
        public static Dictionary<string, GameScore> _games;

        public static string Run(List<Player> players)
        {
            _games = new Dictionary<string, GameScore>();

            var playersNextGame = Clone(players, false);
            var score = PlayGame(playersNextGame);

            return score.Score.ToString();
        }

        public static GameScore PlayGame(List<Player> players)
        {
            var played = new HashSet<string>();
            GameScore result;

            while (true)
            {
                var hash = GenerateHandHash(players);

                // Hand played before, player 1 wins
                if (played.Contains(hash))
                {
                    result = new GameScore
                    {
                        Score = CalculateScore(players[0].Cards),
                        WinnerIndex = 0
                    };
                    break;
                }

                played.Add(hash);

                // Any deck out of cards? Other player wins
                var beforeRoundScore = CheckWinCondition(players);
                if (beforeRoundScore.Score >= 0 && beforeRoundScore.WinnerIndex >= 0)
                {
                    result = beforeRoundScore;
                    break;
                }

                var player1Card = players[0].Cards.First.Value;
                var player2Card = players[1].Cards.First.Value;

                // Do we need a sub game?
                if (player1Card < players[0].Cards.Count && player2Card < players[1].Cards.Count)
                {
                    GameScore subgameScore = default;

                    if (_games.ContainsKey(hash))
                        subgameScore = _games[hash];
                    else
                    {
                        var playersSubgame = Clone(players, true);
                        subgameScore = PlayGame(playersSubgame);
                        _games.Add(hash, subgameScore);
                    }

                    // Who won subgame, or won the cached copy of this subgame?
                    if (subgameScore.WinnerIndex == 0)
                        WonRound(players[0], players[1]);
                    else
                        WonRound(players[1], players[0]);
                }
                else
                {
                    // If we do not need a subgame, who had the larger card?
                    if (player1Card > player2Card)
                        WonRound(players[0], players[1]);
                    else
                        WonRound(players[1], players[0]);
                }

                // Any deck out of cards? Other player wins
                var afterRoundScore = CheckWinCondition(players);
                if (afterRoundScore.Score >= 0 && afterRoundScore.WinnerIndex >= 0)
                {
                    result = afterRoundScore;
                    break;
                }
            }

            return result;
        }

        public static GameScore CheckWinCondition(List<Player> players)
        {
            if (players[0].Cards.Count == 0)
                return new GameScore
                {
                    Score = CalculateScore(players[1].Cards),
                    WinnerIndex = 1
                };
            else if (players[1].Cards.Count == 0)
                return new GameScore
                {
                    Score = CalculateScore(players[0].Cards),
                    WinnerIndex = 0
                };

            return GameScore.NoWinner;
        }

        public static string GenerateHandHash(List<Player> players)
        {
            string result = string.Empty;
            foreach (var player in players)
            {
                var bytes = new List<byte>();
                foreach (var card in player.Cards)
                    bytes.Add((byte)card);
                result += Convert.ToBase64String(bytes.ToArray());
                result += "-";
            }
            return result.Trim('-');
        }

        public static List<Player> Clone(List<Player> players, bool subgameClone)
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

            if (subgameClone)
                foreach (var player in result)
                {
                    var cardValue = player.Cards.First.Value;
                    player.Cards.RemoveFirst();

                    var numberOfCardsToRemove = player.Cards.Count - cardValue;
                    for (int removeIndex = 0; removeIndex < numberOfCardsToRemove; removeIndex++)
                        player.Cards.RemoveLast();
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

        public static void WonRound(Player winner, Player loser)
        {
            var winnerDeck = winner.Cards;
            var loserDeck = loser.Cards;

            var firstCardWinner = winnerDeck.First;
            winnerDeck.RemoveFirst();
            winnerDeck.AddLast(firstCardWinner);
            var firstCardLoser = loserDeck.First;
            loserDeck.RemoveFirst();
            winnerDeck.AddLast(firstCardLoser);

            //Console.WriteLine($"Game Won by Player {winner.Id}, Cards: {string.Join(",", winner.Cards)}");
        }
    }
}
