using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace day20
{
    public class SecondStar
    {
        #region Models
        public class MapTile
        {
            public Tile Self;

            public byte Rotation;
            public byte HorizontalFlip;
            public byte VerticalFlip;

            public MapTile Up;
            public MapTile Down;
            public MapTile Left;
            public MapTile Right;

            public MapTile(Tile self, byte rotation, byte horizontal, byte vertical, MapTile up = null, MapTile down = null, MapTile left = null, MapTile right = null)
            {
                Self = self;
                Up = up;
                Down = down;
                Left = left;
                Right = right;
            }

            public int GenerateHashCode()
                => HashCode.Combine(Self.Id, Rotation, HorizontalFlip, VerticalFlip);
        }

        public enum Direction
        {
            Up, Down, Left, Right
        }

        public struct Connection
        {
            public int SourceId;
            public int TargetId;
            public Direction Direction;
            public string SourceType;
            public string TargetType;

            public Connection(int sourceId, int targetId, Direction direction, string sourceType, string targetType)
            {
                SourceId = sourceId;
                TargetId = targetId;
                Direction = direction;
                SourceType = sourceType;
                TargetType = targetType;
            }
        }

        public struct Match
        {
            public bool Up;
            public bool Down;
            public bool Left;
            public bool Right;
        }

        public struct MatchInfo
        {
            public int Source;
            public int Target;
            public byte Rotation;
            public byte Horizontal;
            public byte Vertical;
            public Direction Direction;
        }
        #endregion

        private static Dictionary<int, char[,]> _memo;
        private static List<MapTile> _maptiles;

        private static string[] _seaMonster;
        private static char[,] _board;
        private static int[,] _mapIds;
        public static int _maxBoardWriteX = 0;
        public static int _maxBoardWriteY = 0;

        public static string Run(List<Tile> input)
        {
            Console.WindowWidth = 80;
            Console.WindowHeight = 50;

            _board = new char[96, 96];
            _mapIds = new int[12, 12];

            var tiles = new Dictionary<int, Tile>();
            tiles = input.ToDictionary(t => t.Id);

            _memo = new Dictionary<int, char[,]>();
            _maptiles = new List<MapTile>();

            _seaMonster = new string[]
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   "
            };

            var cornersAndEdges = FindCornersAndEdges(input);

#if RUBBISH
            var connections = new Dictionary<int, List<Connection>>();
#endif
            var connections = new Dictionary<int, List<Connection>>();
            var memo = new Dictionary<int, List<MatchInfo>>();

            var masterCorner = cornersAndEdges.MasterCornerId;
            var masterCornerMatch = cornersAndEdges.MasterCornerMatch;

            int nextTileId = masterCorner;
            bool connecting = true;
            var links = new HashSet<int>();

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

                    combinations.AddRange(CheckCombinations(tiles[nextTileId], tiles[scoutId]));

                    if (combinations.Count > 0)
                    {
                        links.Add(hash);

                        if (combinations.Count > 1)
                            System.Diagnostics.Debug.WriteLine($"{nextTileId} -> {scoutId}, Combinations: {combinations.Count}");

                        var map = Rotate(tiles[scoutId].Map, combinations[0].Rotation);
                        map = Flip(map, combinations[0].Horizontal, combinations[0].Vertical);

                        tiles[scoutId] = new Tile
                        {
                            Id = scoutId,
                            Map = map
                        };

                        if (combinations[0].Direction == Direction.Up)
                        {
                            y -= 8;
                            mapIdY--;
                        }
                        if (combinations[0].Direction == Direction.Down)
                        {
                            y += 8;
                            mapIdY++;
                        }
                        if (combinations[0].Direction == Direction.Left)
                        {
                            x -= 8;
                            mapIdX--;
                        }
                        if (combinations[0].Direction == Direction.Right)
                        {
                            x += 8;
                            mapIdX++;
                        }

                        DrawMapToBoard(map, x, y);

                        _mapIds[mapIdY, mapIdX] = scoutId;

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
            {
                if (!cornersAndEdges.Corners.Contains(keyValuePair.Key) && !cornersAndEdges.Edges.Contains(keyValuePair.Key))
                    restOfTiles.Add(keyValuePair.Key, keyValuePair.Value);
            }

            y = 8;

            for (int middleY = 1; middleY < _mapIds.GetLength(0) - 1; middleY++)
            {
                x = 8;

                for (int middleX = 1; middleX < _mapIds.GetLength(1) - 1; middleX++)
                {
                    int aboveTileId = _mapIds[middleY - 1, middleX];
                    var tileKeysToRemove = new HashSet<int>();

                    foreach (var tile in restOfTiles)
                    {
                        var combinations = CheckCombinations(tiles[aboveTileId], tile.Value);

                        if (combinations.Count > 0)
                        {
                            var match = combinations.First(c => c.Direction == Direction.Down);

                            var map = tile.Value.Map;
                            if (match.Rotation > 0)
                                map = Rotate(map, match.Rotation);
                            if (match.Horizontal > 0 || match.Vertical > 0)
                                map = Flip(map, match.Horizontal, match.Vertical);

                            tileKeysToRemove.Add(tile.Key);
                            _mapIds[middleY, middleX] = tile.Key;

                            tiles[tile.Key] = new Tile
                            {
                                Id = tile.Key,
                                Map = map
                            };

                            DrawMapToBoard(map, x, y);
                        }
                    }

                    foreach (var tileKeyToRemove in tileKeysToRemove)
                        restOfTiles.Remove(tileKeyToRemove);

                    tileKeysToRemove.Clear();
                            
                    x += 8;
                }

                y += 8;
            }

            // TO REFACTOR:
            // I tried my way forward, found sea monsters by flipping once and it was correct.
            // This should ofcourse search through the different options and test for more cases
            // like most things do in this problem, but I've worked on this for far too long,
            // so it will be a job for future me.

            // 2020-12-21, me

            _board = Flip(_board, 0, 1);

            bool looking = true;
            int lookingX = 0;
            int lookingY = 0;

            while (looking)
            {
                Console.Clear();

                var maxBoardY = Math.Min(lookingY + 49, _board.GetLength(0));
                var maxBoardX = Math.Min(lookingX + 80, _board.GetLength(1));

                for (int boardY = lookingY; boardY < maxBoardY; boardY++)
                    for (int boardX = lookingX; boardX < maxBoardX; boardX++)
                    {
                        var screenX = 1 + boardX - lookingX;
                        var screenY = 1 + boardY - lookingY;

                        if (screenX > 0 && screenY > 0 && screenX < Console.WindowWidth - 1 && screenY < Console.WindowHeight - 1)
                        {
                            Console.SetCursorPosition(screenX, screenY);
                            Console.Write(_board[boardY, boardX]);
                        }
                    }

                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow: lookingY -= 7; break;
                    case ConsoleKey.DownArrow: lookingY += 7; break;
                    case ConsoleKey.LeftArrow: lookingX -= 7; break;
                    case ConsoleKey.RightArrow: lookingX += 7; break;
                    case ConsoleKey.F: FindSeaMonster(); break;
                    case ConsoleKey.C: Console.WriteLine(); return CountRoughSea();
                    case ConsoleKey.Q: looking = false; break;
                }

                if (lookingY < 0) lookingY = 0;
                if (lookingY > maxBoardY - 7) lookingY = maxBoardY - 7;
                if (lookingX < 0) lookingX = 0;
                if (lookingX > maxBoardX - 7) lookingX = maxBoardX - 7;
            }

            //foreach (var edge in cornersAndEdges.Edges)
            //{
            //    var combinations = CheckCombinations(tiles[masterCorner], tiles[edge]);

            //    if (combinations.Count > 0)
            //    {
            //        var map = Rotate(tiles[edge].Map, combinations[0].Rotation);
            //        tiles[edge] = new Tile
            //        {
            //            Id = combinations[0].Target,
            //            Map = map
            //        };
            //        connections.Add(masterCorner, new List<Connection>());
            //        connections[masterCorner].Add(new Connection(masterCorner, edge, combinations[0].Direction, "Corner", "Edge"));
            //        break;
            //    }

            //}

#if RUBBISH
            #region This is a load of rubbish for now
            // find a corner that has exactly two matches
            foreach (var cornerId in cornersAndEdges.Corners)
            {
                var corner = tiles[cornerId];
                var connects = new Dictionary<int, List<MatchInfo>>();
                var hash = new HashSet<int>();

                foreach (var edgeId in cornersAndEdges.Edges)
                {
                    var edge = tiles[edgeId];

                    var matchinfo = new List<MatchInfo>();

                    var key = HashCode.Combine(corner.Id, edge.Id);
                    if (memo.ContainsKey(key))
                        matchinfo = memo[key];
                    else
                    {
                        matchinfo = CheckCombinations(corner, edge);
                        memo.Add(key, matchinfo);
                    }

                    foreach (var info in matchinfo)
                    {
                        if (!connects.ContainsKey(info.Source))
                            connects.Add(info.Source, new List<MatchInfo>());
                        connects[info.Source].Add(info);
                    }
                }

                foreach (var connect in connects)
                {
                    if (!connections.ContainsKey(connect.Key))
                        connections.Add(connect.Key, new List<Connection>());
                    foreach (var match in connect.Value)
                    {
                        var newConnection = new Connection(match.Source, match.Target, match.Direction, "Corner", "Edge");
                        if (!connections[connect.Key].Contains(newConnection))
                            connections[connect.Key].Add(newConnection);
                        hash.Add(match.Source);
                        hash.Add(match.Target);
                    }
                }
            }

            // trace edge
            foreach (var cornerId in cornersAndEdges.Corners)
                ConnectingEdges(cornerId, tiles, connections, cornersAndEdges.Edges, memo);

            // run through edges to connect them up
            foreach (var connection in connections.Where(c => c.Value.Count == 1))
            {
                int not = connection.Value[0].SourceId;
                foreach (var edge in cornersAndEdges.Edges)
                {
                    if (connection.Key == edge || edge == not)
                        continue;

                    var endchainCombination = CheckCombinations(tiles[connection.Key], tiles[edge]);

                    if (endchainCombination.Count > 0)
                    {
                        var eC = endchainCombination[0];
                        connections[connection.Key].Add(new Connection(eC.Source, eC.Target, eC.Direction, "Edge", "Edge"));
                    }
                }
            }

            bool quit = false;
            int top = 0;
            int left = 0;



            Console.Clear();

            while (!quit)
            {
                var cornerTile = tiles[cornersAndEdges.Corners[0]];
                Draw(new Point(40, 20), cornerTile.Map, 0, 0, 0);
                var firstEdge1 = tiles[connections[cornerTile.Id][1].TargetId];
                var key = HashCode.Combine(cornerTile.Id, firstEdge1.Id);
                var matchInfo = memo[key].Where(m => m.Target == firstEdge1.Id).First();
                Draw(new Point(51, 20), firstEdge1.Map, matchInfo.Rotation, matchInfo.Horizontal, matchInfo.Vertical);

                var firstEdge2 = tiles[connections[firstEdge1.Id][1].TargetId];
                key = HashCode.Combine(firstEdge1.Id, firstEdge2.Id);
                matchInfo = memo[key].Where(m => m.Target == firstEdge2.Id).First();
                Draw(new Point(62, 20), firstEdge2.Map, matchInfo.Rotation, matchInfo.Horizontal, 0);

                var secondEdge1 = tiles[connections[cornerTile.Id][0].TargetId];
                key = HashCode.Combine(cornerTile.Id, secondEdge1.Id);
                matchInfo = memo[key].Where(m => m.Target == secondEdge1.Id).First();
                Draw(new Point(40, 9), secondEdge1.Map, matchInfo.Rotation, matchInfo.Horizontal, matchInfo.Vertical);

                var answer = Console.ReadKey(true);

                if (answer.Key == ConsoleKey.Q)
                    quit = true;
                if (answer.Key == ConsoleKey.DownArrow)
                    top++;
                if (answer.Key == ConsoleKey.UpArrow)
                    top--;
                if (answer.Key == ConsoleKey.LeftArrow)
                    left--;
                if (answer.Key == ConsoleKey.RightArrow)
                    left++;
            }

            // look through all other edges until it connects to one of our corner-edges
            #endregion
#endif

            return "";
        }

        public static string CountRoughSea()
        {
            int roughSea = 0;
            for (int y = 0; y < _board.GetLength(0); y++)
                for (int x = 0; x < _board.GetLength(1); x++)
                {
                    if (_board[y, x] == '#')
                        roughSea++;
                }
            return roughSea.ToString();
        }

        public static void DrawMapToBoard(char[,] map, int x, int y)
        {
            for (int yIndex = y; yIndex < y + 8; yIndex++)
                for (int xIndex = x; xIndex < x + 8; xIndex++)
                {
                    _board[yIndex, xIndex] = map[yIndex - y + 1, xIndex - x + 1];
                    if (yIndex > _maxBoardWriteY)
                        _maxBoardWriteY = yIndex;
                    if (xIndex > _maxBoardWriteX)
                        _maxBoardWriteX = xIndex;
                }
        }

        public static void FindSeaMonster()
        {
            // find and mark, so we count # later

            var checkForSearMonster = new Func<int, int, bool>((int bX, int bY) =>
            {
                for (int y = 0; y < _seaMonster.Length; y++)
                    for (int x = 0; x < _seaMonster[0].Length; x++)
                    {
                        if (_seaMonster[y][x] == '#' && _board[bY + y, bX + x] != _seaMonster[y][x])
                            return false;
                    }

                // rewrite board here if we want to
                for (int y = 0; y < _seaMonster.Length; y++)
                    for (int x = 0; x < _seaMonster[0].Length; x++)
                    {
                        if (_seaMonster[y][x] == '#')
                            _board[bY + y, bX + x] = 'O';
                    }

                return true;
            });

            var monstersFound = 0;

            for (int y = 0; y < _board.GetLength(0); y++)
                for (int x = 0; x < _board.GetLength(1); x++)
                {
                    if (x + _seaMonster[0].Length >= _board.GetLength(1)
                        || y + _seaMonster.Length >= _board.GetLength(0)
                        || !checkForSearMonster(x, y))
                        continue;
                    else
                        monstersFound++;
                }

            Console.WriteLine();
            Console.WriteLine($"Found {monstersFound} monster(s)!");
        }

        public static void Draw(Point at, char[,] map, byte rotations, byte horizontal, byte vertical)
        {
            if (rotations > 0)
                map = Rotate(map, rotations);
            if (horizontal > 0 || vertical > 0)
                map = Flip(map, horizontal, vertical);

            for (var y = 0; y < map.GetLength(0); y++)
                for (var x = 0; x < map.GetLength(1); x++)
                {
                    Console.SetCursorPosition(at.X + x, at.Y + y);
                    Console.Write(map[y, x]);
                }
        }

        public static void ConnectingEdges(
            int cornerId,
            Dictionary<int, Tile> tiles,
            Dictionary<int, List<Connection>> connections,
            List<int> edges,
            Dictionary<int, List<MatchInfo>> memo)
        {
            foreach (var connection in connections[cornerId])
            {
                var edgeId = connection.TargetId;
                var edgeTile = tiles[edgeId];

                bool search = true;
                int previous = -1;

                while (search)
                {
                    var matches = new List<MatchInfo>();
                    foreach (var edge in edges)
                    {
                        if (edge == edgeTile.Id)
                            continue;

                        if (edge == previous)
                            continue;

                        var combinations = new List<MatchInfo>();
                        var key = HashCode.Combine(edgeTile.Id, edge);
                        if (memo.ContainsKey(key))
                            combinations = memo[key];
                        else
                        {
                            combinations = CheckCombinations(edgeTile, tiles[edge]);
                            memo.Add(key, combinations);
                        }

                        foreach (var combination in combinations)
                            matches.Add(combination);
                    }

                    if (matches.Count > 0)
                    {
                        var distinct = matches.Select(m => m.Target).Distinct();

                        if (distinct.Count() == 1)
                        {
                            var connectingId = distinct.First();
                            //Console.WriteLine($"Edge pieces connect: {edgeTile.Id} <-> {connectingId}");

                            if (!connections.ContainsKey(edgeTile.Id))
                                connections.Add(edgeTile.Id, new List<Connection>());
                            connections[edgeTile.Id].Add(new Connection(edgeTile.Id, connectingId, matches.First().Direction, "Edge", "Edge"));

                            previous = edgeTile.Id;
                            edgeTile = tiles[connectingId];
                        }
                    }
                    else
                        search = false;
                }
            }
        }

        public static (List<int> Corners, List<int> Edges, int MasterCornerId, Match MasterCornerMatch) FindCornersAndEdges(List<Tile> tiles)
        {
            var matchedTiles = new Dictionary<int, Match>();

            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles.Count; j++)
                {
                    if (i == j) continue;
                    var idSource = tiles[i].Id;
                    var idTarget = tiles[j].Id;

                    var allMatches = CheckAllMatches(tiles[i], tiles[j]);
                    if (!matchedTiles.ContainsKey(idSource))
                        matchedTiles.Add(idSource, new Match());


                    var newMatch = new Match();
                    newMatch.Up = allMatches.Up | matchedTiles[idSource].Up;
                    newMatch.Down = allMatches.Down | matchedTiles[idSource].Down;
                    newMatch.Left = allMatches.Left | matchedTiles[idSource].Left;
                    newMatch.Right = allMatches.Right | matchedTiles[idSource].Right;

                    matchedTiles[idSource] = newMatch;
                }
            }

            var corners = matchedTiles
                .Where(m => ((m.Value.Up ? 1 : 0) + (m.Value.Down ? 1 : 0) + (m.Value.Left ? 1 : 0) + (m.Value.Right ? 1 : 0)) == 2)
                .Select(k => k.Key)
                .ToList();

            var edges = matchedTiles
                .Where(m => ((m.Value.Up ? 1 : 0) + (m.Value.Down ? 1 : 0) + (m.Value.Left ? 1 : 0) + (m.Value.Right ? 1 : 0)) == 3)
                .Select(k => k.Key)
                .ToList();

            int masterCorner = matchedTiles.Where(m => m.Value.Right && m.Value.Down && !m.Value.Left && !m.Value.Up).Select(m => m.Key).FirstOrDefault();
            if (masterCorner == 0) masterCorner = corners.First();

            var masterCornerMatch = matchedTiles[masterCorner];

            return (corners, edges, masterCorner, masterCornerMatch);
        }

        public static void Test()
        {
            char[,] map = new char[10, 10];

            var mapStrings = new List<string>()
            {
                "xxxxxxxxxy",
                "wxxxxxxxyy",
                "wwxxxxxyyy",
                "wwwxxxyyyy",
                "wwwwxyyyyy",
                "wwwwwzyyyy",
                "wwwwzzzyyy",
                "wwwzzzzzyy",
                "wwzzzzzzzy",
                "wzzzzzzzzz"
            };

            Draw(new Point(1, 1), Clone(map), 0, 0, 0);
            Console.ForegroundColor = ConsoleColor.Gray;

            Draw(new Point(12, 1), Clone(map), 1, 0, 0);
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Draw(new Point(24, 1), Clone(map), 2, 0, 0);
            Console.ForegroundColor = ConsoleColor.White;

            Draw(new Point(36, 1), Clone(map), 3, 0, 0);

            Console.WriteLine();
        }

        private static Match CheckAllMatches(Tile source, Tile target)
        {
            var result = new Match();

            for (byte rotationIndex = 0; rotationIndex < 4; rotationIndex++)
                for (byte horizontalIndex = 0; horizontalIndex < 2; horizontalIndex++)
                    for (byte verticalIndex = 0; verticalIndex < 2; verticalIndex++)
                    {
                        if (horizontalIndex == 1 && verticalIndex == 1)
                            continue;

                        var hash = HashCode.Combine(target.Id, rotationIndex, horizontalIndex, verticalIndex);
                        var map = new char[0, 0];

                        if (_memo.ContainsKey(hash))
                            map = Clone(_memo[hash]);
                        else
                        {
                            map = Clone(target.Map);

                            if (rotationIndex > 0)
                                map = Rotate(map, rotationIndex);
                            if (horizontalIndex > 0 || verticalIndex > 0)
                                map = Flip(map, horizontalIndex, verticalIndex);

                            _memo.Add(hash, map);
                        }

                        if (MatchUp(source.Map, map))
                            result.Up = true;
                        if (MatchDown(source.Map, map))
                            result.Down = true;
                        if (MatchLeft(source.Map, map))
                            result.Left = true;
                        if (MatchRight(source.Map, map))
                            result.Right = true;
                    }

            return result;
        }

        public static List<MatchInfo> CheckCombinations(Tile source, Tile target)
        {
            var result = new List<MatchInfo>();

            for (byte rotationIndex = 0; rotationIndex < 4; rotationIndex++)
                for (byte horizontalIndex = 0; horizontalIndex < 2; horizontalIndex++)
                    for (byte verticalIndex = 0; verticalIndex < 2; verticalIndex++)
                    {
                        if (horizontalIndex == 1 && verticalIndex == 1)
                            continue;

                        var hash = HashCode.Combine(target.Id, rotationIndex, horizontalIndex, verticalIndex);
                        var map = new char[0, 0];

                        if (_memo.ContainsKey(hash))
                            map = Clone(_memo[hash]);
                        else
                        {
                            map = Clone(target.Map);

                            if (rotationIndex > 0)
                                map = Rotate(map, rotationIndex);
                            if (horizontalIndex > 0 || verticalIndex > 0)
                                map = Flip(map, horizontalIndex, verticalIndex);

                            _memo.Add(hash, map);
                        }

                        if (MatchUp(source.Map, map))
                        {
                            result.Add(new MatchInfo
                            {
                                Source = source.Id,
                                Target = target.Id,
                                Rotation = rotationIndex,
                                Horizontal = horizontalIndex,
                                Vertical = verticalIndex,
                                Direction = Direction.Up
                            });
                        }

                        if (MatchDown(source.Map, map))
                        {
                            result.Add(new MatchInfo
                            {
                                Source = source.Id,
                                Target = target.Id,
                                Rotation = rotationIndex,
                                Horizontal = horizontalIndex,
                                Vertical = verticalIndex,
                                Direction = Direction.Down
                            });
                        }

                        if (MatchLeft(source.Map, map))
                        {
                            result.Add(new MatchInfo
                            {
                                Source = source.Id,
                                Target = target.Id,
                                Rotation = rotationIndex,
                                Horizontal = horizontalIndex,
                                Vertical = verticalIndex,
                                Direction = Direction.Left
                            });
                        }

                        if (MatchRight(source.Map, map))
                        {
                            result.Add(new MatchInfo
                            {
                                Source = source.Id,
                                Target = target.Id,
                                Rotation = rotationIndex,
                                Horizontal = horizontalIndex,
                                Vertical = verticalIndex,
                                Direction = Direction.Right
                            });
                        }
                    }

            return result;
        }

        public static bool MatchUp(char[,] source, char[,] target)
        {
            var bottomY = target.GetLength(0) - 1;
            for (int x = 0; x < target.GetLength(1); x++)
                if (source[0, x] != target[bottomY, x])
                    return false;
            return true;
        }

        public static bool MatchDown(char[,] source, char[,] target)
        {
            var bottomY = source.GetLength(0) - 1;
            for (int x = 0; x < target.GetLength(1); x++)
                if (source[bottomY, x] != target[0, x])
                    return false;
            return true;
        }

        public static bool MatchRight(char[,] source, char[,] target)
        {
            var rightX = source.GetLength(1) - 1;
            for (int y = 0; y < target.GetLength(0); y++)
                if (source[y, rightX] != target[y, 0])
                    return false;
            return true;
        }

        public static bool MatchLeft(char[,] source, char[,] target)
        {
            var rightX = target.GetLength(1) - 1;
            for (int y = 0; y < target.GetLength(0); y++)
                if (source[y, 0] != target[y, rightX])
                    return false;
            return true;
        }

        public static char[,] Rotate(char[,] map, int rotations)
        {
            var mapY = map.GetLength(0);
            var mapX = map.GetLength(1);
            var mapNext = new char[mapY, mapX];

            mapNext = Clone(map);

            int layers = mapX / 2;

            if (mapY / 2 != layers)
                throw new Exception("Rotate only works if the X and Y dimensions are the same.");

            for (var rotation = 0; rotation < rotations; rotation++)
            {
                for (var layerIndex = 0; layerIndex < layers; layerIndex++)
                {
                    int iterations = mapX - layerIndex * 2;

                    for (int iteration = 0; iteration < iterations; iteration++)
                        (mapNext[layerIndex + 0, layerIndex + 0 + iteration], mapNext[layerIndex + 0 + iteration, mapX - layerIndex - 1], mapNext[mapY - layerIndex - 1, mapX - layerIndex - iteration - 1], mapNext[mapY - layerIndex - iteration - 1, layerIndex + 0])
                            = (map[mapY - layerIndex - iteration - 1, layerIndex + 0], map[layerIndex + 0, layerIndex + 0 + iteration], map[layerIndex + 0 + iteration, mapX - layerIndex - 1], map[mapY - layerIndex - 1, mapX - layerIndex - iteration - 1]);
                }

                map = Clone(mapNext);
            }

            return mapNext;
        }

        public static char[,] Clone(char[,] source)
        {
            var mapY = source.GetLength(0);
            var mapX = source.GetLength(1);
            var target = new char[mapY, mapX];
            for (int y = 0; y < mapY; y++)
                for (int x = 0; x < mapX; x++)
                    target[y, x] = source[y, x];
            return target;
        }

        public static char[,] Flip(char[,] map, byte horizontal, byte vertical)
        {
            var mapY = map.GetLength(0);
            var mapX = map.GetLength(1);
            var mapNext = new char[mapY, mapX];

            mapNext = Clone(map);

            if (horizontal > 0)
            {
                for (int y = 0; y < mapY; y++)
                {
                    (mapNext[y, 0], mapNext[y, mapX - 1]) = (map[y, mapX - 1], map[y, 0]);

                    for (int x = 1; x < mapX / 2; x++)
                        (mapNext[y, x], mapNext[y, mapX - x - 1]) = (map[y, mapX - x - 1], map[y, x]);
                }
            }

            if (vertical > 0)
            {
                for (int x = 0; x < mapX; x++)
                {
                    (mapNext[0, x], mapNext[mapY - 1, x]) = (mapNext[mapY - 1, x], mapNext[0, x]);

                    for (int y = 1; y < mapY / 2; y++)
                        (mapNext[y, x], mapNext[mapY - y - 1, x]) = (map[mapY - y - 1, x], map[y, x]);
                }
            }

            return mapNext;
        }
    }
}
