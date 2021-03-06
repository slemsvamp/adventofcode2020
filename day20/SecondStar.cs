using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace day20
{
    public class SecondStar
    {
        private static Dictionary<int, char[,]> _memo;
        private static string[] _seaMonster;
        private static char[,] _board;
        private static int[,] _mapIds;

        public static string Run(List<Tile> input)
        {
            Console.WindowWidth = 80;
            Console.WindowHeight = 50;

            _memo = new Dictionary<int, char[,]>();
            _board = new char[96, 96];
            _mapIds = new int[12, 12];

            var connections = new Dictionary<int, List<Connection>>();
            var memo = new Dictionary<int, List<MatchInfo>>();

            var tiles = new Dictionary<int, Tile>();
            tiles = input.ToDictionary(t => t.Id);

            _seaMonster = new string[]
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   "
            };

            var cornersAndEdges = FindCornersAndEdges(input);

            var masterCorner = cornersAndEdges.MasterCornerId;
            var masterCornerMatch = cornersAndEdges.MasterCornerMatch;

            int x = 0;
            int y = 0;

            int mapIdY = 0;
            int mapIdX = 0;

            if (masterCornerMatch.Up)
            {
                y += _board.GetLength(0) - 10;
                mapIdY += 10;
            }
            if (masterCornerMatch.Left)
            {
                x += _board.GetLength(1) - 10;
                mapIdX += 10;
            }

            DrawMapToBoard(tiles[masterCorner].Map, x, y);

            int nextTileId = masterCorner;
            bool connecting = true;
            var links = new HashSet<int>();

            Func<List<int>, bool> check = new Func<List<int>, bool>(ces =>
            {
                var combinations = new List<MatchInfo>();

                foreach (var scoutId in ces)
                {
                    if (scoutId == nextTileId)
                        continue;

                    var hash = nextTileId > scoutId ? HashCode.Combine(scoutId, nextTileId) : HashCode.Combine(nextTileId, scoutId);

                    if (links.Contains(hash))
                        continue;

                    combinations.AddRange(
                            TileMatcher.CheckAllCombinations(tiles[nextTileId], tiles[scoutId], _memo)
                        );

                    if (combinations.Count > 0)
                    {
                        links.Add(hash);

                        if (combinations.Count > 1)
                            System.Diagnostics.Debug.WriteLine($"{nextTileId} -> {scoutId}, Combinations: {combinations.Count}");

                        var key = HashCode.Combine(combinations[0].Target, combinations[0].Permutation);
                        var direction = combinations[0].Direction;
                        var map = _memo[key];

                        switch (direction)
                        {
                            case Direction.Up: (y, mapIdY) = (y - 8, mapIdY - 1); break;
                            case Direction.Down: (y, mapIdY) = (y + 8, mapIdY + 1); break;
                            case Direction.Left: (x, mapIdX) = (x - 8, mapIdX - 1); break;
                            case Direction.Right: (x, mapIdX) = (x + 8, mapIdX + 1); break;
                        }

                        _mapIds[mapIdY, mapIdX] = scoutId;
                        tiles[scoutId] = new Tile
                        {
                            Id = scoutId,
                            Map = map
                        };
                        DrawMapToBoard(map, x, y);

                        nextTileId = scoutId;
                        return true;
                    }
                }

                return false;
            });

            while (connecting)
            {
                var isCorner = cornersAndEdges.Corners.Contains(nextTileId);
                var isEdge = cornersAndEdges.Edges.Contains(nextTileId);

                var foundEdge = check(cornersAndEdges.Edges);
                if (foundEdge)
                    continue;

                bool foundCorner = false;

                if (!foundEdge && isEdge)
                    foundCorner = check(cornersAndEdges.Corners);

                if (!foundEdge && !foundCorner)
                    break;
            }

            var restOfTiles = new Dictionary<int, Tile>();

            foreach (var keyValuePair in tiles)
                if (!cornersAndEdges.Corners.Contains(keyValuePair.Key) && !cornersAndEdges.Edges.Contains(keyValuePair.Key))
                    restOfTiles.Add(keyValuePair.Key, keyValuePair.Value);

            for (int middleY = 1; middleY < _mapIds.GetLength(0) - 1; middleY++)
                for (int middleX = 1; middleX < _mapIds.GetLength(1) - 1; middleX++)
                {
                    int aboveTileId = _mapIds[middleY - 1, middleX];
                    var tileKeysToRemove = new HashSet<int>();

                    foreach (var tile in restOfTiles)
                    {
                        var combinations = TileMatcher.CheckAllCombinations(tiles[aboveTileId], tile.Value, _memo);

                        if (combinations.Count > 0)
                        {
                            var match = combinations.First(c => c.Direction == Direction.Down);
                            var key = HashCode.Combine(combinations[0].Target, combinations[0].Permutation);
                            var direction = combinations[0].Direction;
                            var map = _memo[key];

                            _mapIds[middleY, middleX] = tile.Key;
                            tiles[tile.Key] = new Tile
                            {
                                Id = tile.Key,
                                Map = map
                            };
                            
                            DrawMapToBoard(map, middleX * 8, middleY * 8);

                            tileKeysToRemove.Add(tile.Key);
                        }
                    }

                    foreach (var tileKeyToRemove in tileKeysToRemove)
                        restOfTiles.Remove(tileKeyToRemove);

                    tileKeysToRemove.Clear();
                }

            var boards = new Dictionary<int, char[,]>();

            boards.Add(0, _board.Copy());
            boards.Add(1, _board.Flip());
            for (int permutation = 2; permutation < 8; permutation++)
            {
                if (permutation % 2 == 0)
                    boards.Add(permutation, boards[permutation - 2].Rotate());
                else
                    boards.Add(permutation, boards[permutation - 1].Flip());
            }

            foreach (var board in boards)
                if (FindSeamonsters(board.Value) > 0)
                    return CountRoughSea(board.Value);

            return "Failed to find any seamonsters! This ofcourse, NEVER happens! :)";
        }

        private static int FindSeamonsters(char[,] board)
        {
            var checkForSeamonster = new Func<int, int, char[,], bool>((int bX, int bY, char[,] board) =>
            {
                for (int y = 0; y < _seaMonster.Length; y++)
                    for (int x = 0; x < _seaMonster[0].Length; x++)
                    {
                        if (_seaMonster[y][x] == '#' && board[bY + y, bX + x] != _seaMonster[y][x])
                            return false;
                    }

                for (int y = 0; y < _seaMonster.Length; y++)
                    for (int x = 0; x < _seaMonster[0].Length; x++)
                    {
                        if (_seaMonster[y][x] == '#')
                            board[bY + y, bX + x] = 'O';
                    }

                return true;
            });

            var monstersFound = 0;

            for (int y = 0; y < board.GetLength(0); y++)
                for (int x = 0; x < board.GetLength(1); x++)
                {
                    if (x + _seaMonster[0].Length >= board.GetLength(0)
                        || y + _seaMonster.Length >= board.GetLength(1)
                        || !checkForSeamonster(x, y, board))
                        continue;
                    else
                        monstersFound++;
                }

            return monstersFound;
        }

        private static string CountRoughSea(char[,] board)
        {
            int roughSea = 0;
            int seamonsterPart = 0;
            for (int y = 0; y < board.GetLength(0); y++)
                for (int x = 0; x < board.GetLength(1); x++)
                {
                    if (board[y, x] == '#')
                        roughSea++;
                    else if (board[y, x] == 'O')
                        seamonsterPart++;
                }
            return roughSea.ToString();
        }

        private static void DrawMapToBoard(char[,] map, int x, int y)
        {
            for (int yIndex = y; yIndex < y + 8; yIndex++)
                for (int xIndex = x; xIndex < x + 8; xIndex++)
                    _board[yIndex, xIndex] = map[yIndex - y + 1, xIndex - x + 1];
        }

        private static CornersAndEdges FindCornersAndEdges(List<Tile> tiles)
        {
            var matchedTiles = new Dictionary<int, Match>();

            for (int sourceIndex = 0; sourceIndex < tiles.Count; sourceIndex++)
                for (int targetIndex = 0; targetIndex < tiles.Count; targetIndex++)
                {
                    if (sourceIndex == targetIndex) continue;

                    var sourceTile = tiles[sourceIndex];
                    var targetTile = tiles[targetIndex];

                    var sourceId = sourceTile.Id;
                    var targetId = targetTile.Id;

                    var match = TileMatcher.CheckAllCombinations(sourceTile, targetTile, _memo);
                    if (!matchedTiles.ContainsKey(sourceId))
                        matchedTiles.Add(sourceId, Match.Default(false));
                    matchedTiles[sourceId] = Match.Combine(matchedTiles[sourceId], match);
                }

            var cornerPredicate = new Func<KeyValuePair<int, Match>, bool>(
                m => ((m.Value.Up ? 1 : 0) + (m.Value.Down ? 1 : 0) + (m.Value.Left ? 1 : 0) + (m.Value.Right ? 1 : 0)) == 2);

            var edgePredicate = new Func<KeyValuePair<int, Match>, bool>(
                m => ((m.Value.Up ? 1 : 0) + (m.Value.Down ? 1 : 0) + (m.Value.Left ? 1 : 0) + (m.Value.Right ? 1 : 0)) == 3);

            var corners = matchedTiles.Where(cornerPredicate).Select(k => k.Key).ToList();
            var edges = matchedTiles.Where(edgePredicate).Select(k => k.Key).ToList();

            int masterCorner = matchedTiles.Where(m => m.Value.Right && m.Value.Down && !m.Value.Left && !m.Value.Up).Select(m => m.Key).FirstOrDefault();
            if (masterCorner == 0) masterCorner = corners.First();

            var masterCornerMatch = matchedTiles[masterCorner];

            return new CornersAndEdges
            {
                Corners = corners,
                Edges = edges,
                MasterCornerId = masterCorner,
                MasterCornerMatch = masterCornerMatch
            };
        }
    }
}
