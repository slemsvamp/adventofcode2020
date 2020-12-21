using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace day20
{
    public struct Tile
    {
        public int Id;
        public char[,] Map;
    }

    public class InputParser
    {
        internal static List<Tile> Parse(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            var tiles = new List<Tile>();
            var regex = new Regex(@"^Tile (?<id>\d*):$");
            
            Tile tile = default;
            var map = new List<string>();

            foreach (var line in lines)
            {
                if (line == string.Empty)
                {
                    var mapSave = new char[map.Count, map[0].Length];
                    for (var y = 0; y < map.Count; y++)
                        for (var x = 0; x < map[0].Length; x++)
                            mapSave[y, x] = map[y][x];
                    tile.Map = mapSave;
                    tiles.Add(tile);
                    tile = new Tile();
                    map = new List<string>();
                }
                else if (line.StartsWith("Tile"))
                {
                    var match = regex.Match(line);
                    tile.Id = int.Parse(match.Groups["id"].Value);
                }
                else
                    map.Add(line);
            }

            var mapSave2 = new char[map.Count, map[0].Length];
            for (var y = 0; y < map.Count; y++)
                for (var x = 0; x < map[0].Length; x++)
                    mapSave2[y, x] = map[y][x];
            tile.Map = mapSave2;
            tiles.Add(tile);

            return tiles;
        }

        public static List<int> ParseCSV(string filename)
        {
            var lines = File.ReadAllLines(filename);

            var numbers = new List<int>();
            var numberStrings = lines[0].Split(new[] { "," }, StringSplitOptions.None);

            Array.ForEach(numberStrings, n => numbers.Add(int.Parse(n)));

            return numbers;
        }
    }
}
