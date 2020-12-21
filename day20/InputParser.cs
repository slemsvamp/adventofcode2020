using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace day20
{
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
                if (line == string.Empty)
                {
                    tile.Map = WriteMap(map);
                    tiles.Add(tile);
                    tile = new Tile();
                    map.Clear();
                }
                else if (line.StartsWith("Tile"))
                    tile.Id = int.Parse(regex.Match(line).Groups["id"].Value);
                else
                    map.Add(line);

            tile.Map = WriteMap(map);
            tiles.Add(tile);

            return tiles;
        }

        private static char[,] WriteMap(List<string> map)
        {
            var result = new char[map.Count, map[0].Length];
            for (var y = 0; y < map.Count; y++)
                for (var x = 0; x < map[0].Length; x++)
                    result[y, x] = map[y][x];
            return result;
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
