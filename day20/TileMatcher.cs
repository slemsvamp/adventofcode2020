using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace day20
{
    class TileMatcher
    {
        /// <summary>
        /// Iterates through the 8 different permutations and tries matching target above, below, to the left and to the right of the source tile.
        /// </summary>
        /// <param name="source">The tile we will test around.</param>
        /// <param name="target">The tile that we attempt to fit onto each side of the source tile.</param>
        /// <param name="memo">If you want the tile permutations in a memo, this one will be filled. The hash is combined through tile Id and permutation index.</param>
        /// <returns></returns>
        public static List<MatchInfo> CheckAllCombinations(Tile source, Tile target, Dictionary<int, char[,]> memo = null)
        {
            var result = new List<MatchInfo>();
            var localMemo = new Dictionary<int, char[,]>();
            var maps = new List<char[,]>();
            var firstPermutationKey = HashCode.Combine(target.Id, 0);

            if (memo != null && memo.ContainsKey(firstPermutationKey))
                for (int permutation = 0; permutation < 8; permutation++)
                    maps.Add(memo[HashCode.Combine(target.Id, permutation)]);
            else
            {
                localMemo.Add(firstPermutationKey, target.Map.Copy());
                localMemo.Add(HashCode.Combine(target.Id, 1), target.Map.Flip());
                for (int permutation = 2; permutation < 8; permutation++)
                {
                    var key = HashCode.Combine(target.Id, permutation);
                    if (permutation % 2 == 0)
                        localMemo.Add(key, localMemo[HashCode.Combine(target.Id, permutation - 2)].Rotate());
                    else
                        localMemo.Add(key, localMemo[HashCode.Combine(target.Id, permutation - 1)].Flip());
                }

                if (memo != null)
                    foreach (var kvp in localMemo)
                        memo.Add(kvp.Key, kvp.Value);

                foreach (var map in localMemo.Values)
                    maps.Add(map);
            }

            {
                // You might think this scope is a mistake, but no!
                // I just want to reuse the variable "permutation", but it would conflict unless scoped.

                int permutation = 0;
                foreach (var map in maps)
                {
                    if (source.Map.MatchUp(map))
                        result.Add(new MatchInfo
                        {
                            Target = target.Id,
                            Direction = Direction.Up,
                            Permutation = permutation
                        });

                    if (source.Map.MatchDown(map))
                        result.Add(new MatchInfo
                        {
                            Target = target.Id,
                            Direction = Direction.Down,
                            Permutation = permutation
                        });

                    if (source.Map.MatchLeft(map))
                        result.Add(new MatchInfo
                        {
                            Target = target.Id,
                            Direction = Direction.Left,
                            Permutation = permutation
                        });

                    if (source.Map.MatchRight(map))
                        result.Add(new MatchInfo
                        {
                            Target = target.Id,
                            Direction = Direction.Right,
                            Permutation = permutation
                        });

                    permutation++;
                }
            }

            return result;
        }
    }
}
