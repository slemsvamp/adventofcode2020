using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace day20
{
    public class FirstStar
    {
        private static Dictionary<int, char[,]> _memo;

        public static string Run(List<Tile> tiles)
        {
            Console.WindowWidth = 80;
            Console.WindowHeight = 50;

            _memo = new Dictionary<int, char[,]>();

            long total = 1;
            var matched = new Dictionary<int, Match>();

            for (int sourceIndex = 0; sourceIndex < tiles.Count; sourceIndex++)
                for (int targetIndex = 0; targetIndex < tiles.Count; targetIndex++)
                {
                    if (sourceIndex == targetIndex)
                        continue;

                    var sourceTile = tiles[sourceIndex];
                    var targetTile = tiles[targetIndex];

                    var sourceId = sourceTile.Id;
                    var targetId = targetTile.Id;

                    var match = TileMatcher.CheckAllCombinations(tiles[sourceIndex], tiles[targetIndex], _memo);

                    if (!matched.ContainsKey(sourceId))
                        matched.Add(sourceId, Match.Default(false));
                    matched[sourceId] = Match.Combine(matched[sourceId], match);
                }

            var cornerPredicate = new Func<KeyValuePair<int, Match>, bool>(
                m => ((m.Value.Up ? 1 : 0) + (m.Value.Down ? 1 : 0) + (m.Value.Left ? 1 : 0) + (m.Value.Right ? 1 : 0)) == 2);

            var corners = matched.Where(cornerPredicate).Select(m => m.Key).ToList();

            foreach (var id in corners)
                total *= id;

            return total.ToString();
        }
    }
}
