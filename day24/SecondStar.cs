using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace day24
{
    public class SecondStar
    {
        private const int MAX_WIDTH = 1000;

        private static int[] _dX = new[] { -1, 1, -1, 1, -2, 2 };
        private static int[] _dY = new[] { -1, -1, 1, 1, 0, 0 };

        public static List<Hex> _hexesToAdd;
        public static HashSet<int> _hexesToFlip;

        public static string Run(Dictionary<int, Hex> hexes)
        {
            _hexesToAdd = new List<Hex>();
            _hexesToFlip = new HashSet<int>();

            int days = 100;

            for (int dayIndex = 0; dayIndex < days; dayIndex++)
            {
                foreach (var hex in hexes)
                    CreateNeighbours(hexes, hex.Key);

                foreach (var hexToAdd in _hexesToAdd)
                    if (!hexes.ContainsKey(hexToAdd.Index))
                        hexes.Add(hexToAdd.Index, hexToAdd);

                _hexesToAdd.Clear();

                var hexKeys = hexes.Keys.ToArray();

                for (int hexIndex = 0; hexIndex < hexes.Count; hexIndex++)
                {
                    var key = hexKeys[hexIndex];
                    var hex = hexes[key];

                    var blackTiles = CountBlackNeighbourTiles(hexes, hex.Index);

                    if ((hex.White == false && (blackTiles == 0 || blackTiles > 2)) || (hex.White && blackTiles == 2))
                        if (!_hexesToFlip.Contains(hex.Index))
                            _hexesToFlip.Add(hex.Index);
                }

                foreach (var hexToFlip in _hexesToFlip)
                    hexes[hexToFlip].White = !hexes[hexToFlip].White;

                foreach (var hexToAdd in _hexesToAdd)
                    if (!hexes.ContainsKey(hexToAdd.Index))
                        hexes.Add(hexToAdd.Index, hexToAdd);

                _hexesToAdd.Clear();
                _hexesToFlip.Clear();
            }

            return hexes.Values.Count(hex => !hex.White).ToString();
        }

        public static int CountBlackNeighbourTiles(Dictionary<int, Hex> hexes, int sourceIndex)
        {
            int result = 0;

            for (int neighbourIndex = 0; neighbourIndex < _dX.Length; neighbourIndex++)
            {
                int x = sourceIndex % MAX_WIDTH;
                int y = sourceIndex / MAX_WIDTH;

                int index = (y + _dY[neighbourIndex]) * MAX_WIDTH + (x + _dX[neighbourIndex]);

                if (hexes.ContainsKey(index))
                    result += hexes[index].White ? 0 : 1;
            }

            return result;
        }

        public static void CreateNeighbours(Dictionary<int, Hex> hexes, int sourceIndex)
        {
            for (int neighbourIndex = 0; neighbourIndex < _dX.Length; neighbourIndex++)
            {
                int x = sourceIndex % MAX_WIDTH;
                int y = sourceIndex / MAX_WIDTH;

                int index = (y + _dY[neighbourIndex]) * MAX_WIDTH + (x + _dX[neighbourIndex]);

                if (!hexes.ContainsKey(index) && hexes[sourceIndex].White == false)
                    _hexesToAdd.Add(new Hex(index));
            }
        }
    }
}
