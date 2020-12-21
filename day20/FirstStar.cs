using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace day20
{
    public class FirstStar
    {
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

        private static Dictionary<int, char[,]> _memo;
        private static List<MapTile> _maptiles;

        public static string Run(List<Tile> tiles)
        {
            Console.WindowWidth = 80;
            Console.WindowHeight = 50;

            Tile first = tiles[0];

            _memo = new Dictionary<int, char[,]>();

            _maptiles = new List<MapTile>();
            _maptiles.Add(new MapTile(first, 0, 0, 0));

            //var tile1951 = tiles.Where(t => t.Id == 1951).Single();
            //var tile2311 = tiles.Where(t => t.Id == 2311).Single();

            //var map1951 = Flip(tile1951.Map, 0, 1);
            //var map2311 = Flip(tile2311.Map, 0, 1);

            //var match = MatchRight(map1951, map2311);

            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Draw(new Point(1, 1), map1951, 0, 0, 0);
            //Console.ForegroundColor = ConsoleColor.Red;
            //Draw(new Point(11, 1), map2311, 0, 0, 0);
            //Console.ForegroundColor = ConsoleColor.Blue;
            //Draw(new Point(1, 11), tile1951.Map, 0, 0, 0);
            //Console.ForegroundColor = ConsoleColor.Green;
            //Draw(new Point(11, 11), tile2311.Map, 0, 0, 0);

            //Console.ForegroundColor = ConsoleColor.White;

            //Test();
            //Test2(tiles);

            //Start(tiles);

            //var nw = _maptiles.Where(m => m.Up == null && m.Left == null && m.Right != null && m.Down != null).Single();
            //var ne = _maptiles.Where(m => m.Up == null && m.Left != null && m.Right == null && m.Down != null).Single();
            //var sw = _maptiles.Where(m => m.Up != null && m.Left == null && m.Right != null && m.Down == null).Single();
            //var se = _maptiles.Where(m => m.Up != null && m.Left != null && m.Right == null && m.Down == null).Single();

            return Test2(tiles).ToString(); // ((long)nw.Self.Id * ne.Self.Id * sw.Self.Id * se.Self.Id).ToString();
        }

        public static long Test2(List<Tile> tiles)
        {
            var matchedTiles = new Dictionary<int, Match>();

            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles.Count; j++)
                {
                    if (i == j) continue;
                    var match = CheckCombinations2(new MapTile(tiles[i], 0, 0, 0), tiles[j]);
                    if (!matchedTiles.ContainsKey(tiles[i].Id))
                        matchedTiles.Add(tiles[i].Id, new Match());
                    var matchNew = new Match();
                    matchNew.Up = matchedTiles[tiles[i].Id].Up | match.Up;
                    matchNew.Down = matchedTiles[tiles[i].Id].Down | match.Down;
                    matchNew.Left = matchedTiles[tiles[i].Id].Left | match.Left;
                    matchNew.Right = matchedTiles[tiles[i].Id].Right | match.Right;
                    matchedTiles[tiles[i].Id] = matchNew;
                }
            }

            var matched = matchedTiles.Where(m => ((m.Value.Up ? 1 : 0) + (m.Value.Down ? 1 : 0) + (m.Value.Left ? 1 : 0) + (m.Value.Right ? 1 : 0)) == 2);
            
            var corners = matched.Select(m => m.Key);

            long total = 1;

            foreach (var corner in corners)
                total *= corner;

            return total;
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

        public static void Start(List<Tile> tiles)
        {
            while (_maptiles.Count < tiles.Count)
            {
                MapTile maptileToAdd = null;

                foreach (var maptile in _maptiles)
                {
                    for (int tileIndex = 0; tileIndex < tiles.Count; tileIndex++)
                    {
                        var tile = tiles[tileIndex];

                        if (maptile.Self.Id == tile.Id)
                            continue;

                        // find edge tiles first?

                        var matchingTile = CheckCombinations(maptile, tile);

                        if (matchingTile != null)
                        {
                            // if it fits, add it here after fixing it up
                            maptileToAdd = matchingTile;
                            break;
                        }
                    }

                    if (maptileToAdd != null)
                        break;
                }

                _maptiles.Add(maptileToAdd);
            }
        }

        private struct Match
        {
            public bool Up;
            public bool Down;
            public bool Left;
            public bool Right;
        }

        private static Match CheckCombinations2(MapTile maptile, Tile tile)
        {
            var matches = new HashSet<int>();

            var match = new Match();

            // possible refactor
            // HRVRHRVR (8 permutations, horizontal -> rotation -> vertical -> rotation -> horizontal ..)

            for (byte rotationIndex = 0; rotationIndex < 4; rotationIndex++)
                for (byte horizontalIndex = 0; horizontalIndex < 2; horizontalIndex++)
                    for (byte verticalIndex = 0; verticalIndex < 2; verticalIndex++)
                    {
                        if (horizontalIndex == 1 && verticalIndex == 1)
                            continue;

                        var hash = HashCode.Combine(tile.Id, rotationIndex, horizontalIndex, verticalIndex);
                        var map = new char[0, 0];

                        if (_memo.ContainsKey(hash))
                            map = Clone(_memo[hash]);
                        else
                        {
                            map = Clone(tile.Map);

                            if (rotationIndex > 0)
                                map = Rotate(map, rotationIndex);
                            if (horizontalIndex > 0 || verticalIndex > 0)
                                map = Flip(map, horizontalIndex, verticalIndex);

                            _memo.Add(hash, map);
                        }

                        if (MatchUp(maptile.Self.Map, map))
                            match.Up = true;
                        if (MatchDown(maptile.Self.Map, map))
                            match.Down = true;
                        if (MatchLeft(maptile.Self.Map, map))
                            match.Left = true;
                        if (MatchRight(maptile.Self.Map, map))
                            match.Right = true;
                    }

            return match;
        }

        public static MapTile CheckCombinations(MapTile maptile, Tile tile)
        {
            for (byte rotationIndex = 0; rotationIndex < 4; rotationIndex++)
                for (byte horizontalIndex = 0; horizontalIndex < 2; horizontalIndex++)
                    for (byte verticalIndex = 0; verticalIndex < 2; verticalIndex++)
                    {
                        if (horizontalIndex == 1 && verticalIndex == 1)
                            continue;

                        var hash = HashCode.Combine(tile.Id, rotationIndex, horizontalIndex, verticalIndex);
                        var map = new char[0, 0];

                        if (_memo.ContainsKey(hash))
                            map = Clone(_memo[hash]);
                        else
                        {
                            map = Clone(tile.Map);

                            if (rotationIndex > 0)
                                map = Rotate(map, rotationIndex);
                            if (horizontalIndex > 0 || verticalIndex > 0)
                                map = Flip(map, horizontalIndex, verticalIndex);

                            _memo.Add(hash, map);
                        }

                        var existingMaptile = _maptiles.Where(m => m.Self.Id == tile.Id).ToArray();
                        if (maptile.Up == null && MatchUp(maptile.Self.Map, map))
                        {
                            if (existingMaptile.Length > 0)
                            {
                                maptile.Up = existingMaptile[0];
                                existingMaptile[0].Down = maptile;
                            }
                            else
                                maptile.Up = new MapTile(tile, rotationIndex, horizontalIndex, verticalIndex, null, maptile, null, null);
                            return maptile.Up;
                        }
                        
                        if (maptile.Down == null && MatchDown(maptile.Self.Map, map))
                        {
                            if (existingMaptile.Length > 0)
                            {
                                maptile.Down = existingMaptile[0];
                                existingMaptile[0].Up = maptile;
                            }
                            else
                                maptile.Down = new MapTile(tile, rotationIndex, horizontalIndex, verticalIndex, maptile, null, null, null);
                            return maptile.Down;
                        }
                        
                        if (maptile.Left == null && MatchLeft(maptile.Self.Map, map))
                        {
                            if (existingMaptile.Length > 0)
                            {
                                maptile.Left = existingMaptile[0];
                                existingMaptile[0].Right = maptile;
                            }
                            else
                                maptile.Left = new MapTile(tile, rotationIndex, horizontalIndex, verticalIndex, null, null, null, maptile);
                            return maptile.Left;
                        }
                        
                        if (maptile.Right == null && MatchRight(maptile.Self.Map, map))
                        {
                            if (existingMaptile.Length > 0)
                            {
                                maptile.Right = existingMaptile[0];
                                existingMaptile[0].Left = maptile;
                            }
                            else
                                maptile.Right = new MapTile(tile, rotationIndex, horizontalIndex, verticalIndex, null, null, maptile, null);
                            return maptile.Right;
                        }
                    }

            return null;
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

        public static bool MatchLeft(char[,] source, char[,] target)
        {
            var rightX = source.GetLength(1) - 1;
            for (int y = 0; y < target.GetLength(0); y++)
                if (source[y, rightX] != target[y, 0])
                    return false;
            return true;
        }

        public static bool MatchRight(char[,] source, char[,] target)
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
