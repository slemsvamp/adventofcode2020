using System;
using System.Collections.Generic;
using System.Text;

namespace day17
{
    public class SecondStar
    {
        private static int[] _dirX = new[] { -1, 0, 1 };
        private static int[] _dirY = new[] { -1, 0, 1 };
        private static int[] _dirZ = new[] { -1, 0, 1 };
        private static int[] _dirW = new[] { -1, 0, 1 };

        private const int HALF_UINT16 = UInt16.MaxValue / 2;

        private struct Point3D
        {
            public long X;
            public long Y;
            public long Z;
            public long W;

            public static Point3D Empty
                => new Point3D { X = 0, Y = 0, Z = 0, W = 0 };

            public ulong Hash
                => (ulong)((X + HALF_UINT16) + ((Y + HALF_UINT16) << 16) + ((Z + HALF_UINT16) << 32) + ((W + HALF_UINT16) << 48));
        }

        private static long _minX;
        private static long _maxX;
        private static long _minY;
        private static long _maxY;
        private static long _minZ;
        private static long _maxZ;
        private static long _minW;
        private static long _maxW;

        public static string Run(string[] cubeSlice)
        {
            var points = new Dictionary<ulong, Point3D>();

            for (var y = 0; y < cubeSlice.Length; y++)
                for (var x = 0; x < cubeSlice[0].Length; x++)
                {
                    if (cubeSlice[y][x] == '#')
                    {
                        var point = new Point3D
                        {
                            X = x,
                            Y = y,
                            Z = 0,
                            W = 0
                        };
                        points.Add(point.Hash, point);

                        if (x < _minX)
                            _minX = x;
                        if (x > _maxX)
                            _maxX = x;
                        if (y < _minY)
                            _minY = y;
                        if (y > _maxY)
                            _maxY = y;
                    }
                }

            for (int cycle = 0; cycle < 6; cycle++)
                points = Cycle(points);

            return points.Count.ToString();
        }

        private static Dictionary<ulong, Point3D> Cycle(Dictionary<ulong, Point3D> points)
        {
            var nextPoints = new Dictionary<ulong, Point3D>();

            long newMinX = _minX;
            long newMaxX = _maxX;
            long newMinY = _minY;
            long newMaxY = _maxY;
            long newMinZ = _minZ;
            long newMaxZ = _maxZ;
            long newMinW = _minW;
            long newMaxW = _maxW;

            for (long sourceW = _minW - 1; sourceW <= _maxW + 1; sourceW++)
                for (long sourceZ = _minZ - 1; sourceZ <= _maxZ + 1; sourceZ++)
                    for (long sourceY = _minY - 1; sourceY <= _maxY + 1; sourceY++)
                        for (long sourceX = _minX - 1; sourceX <= _maxX + 1; sourceX++)
                        {
                            var sourcePoint = new Point3D
                            {
                                X = sourceX,
                                Y = sourceY,
                                Z = sourceZ,
                                W = sourceW
                            };
                            var sourceHash = sourcePoint.Hash;

                            int activeNeighbours = 0;
                            for (long w = 0; w < 3; w++)
                                for (long z = 0; z < 3; z++)
                                    for (long y = 0; y < 3; y++)
                                        for (long x = 0; x < 3; x++)
                                        {
                                            var point = new Point3D
                                            {
                                                X = sourcePoint.X + _dirX[x],
                                                Y = sourcePoint.Y + _dirY[y],
                                                Z = sourcePoint.Z + _dirZ[z],
                                                W = sourcePoint.W + _dirW[w]
                                            };

                                            var hash = point.Hash;

                                            if (hash == sourceHash)
                                                continue;

                                            if (points.ContainsKey(hash))
                                                activeNeighbours++;
                                        }

                            bool isActive = points.ContainsKey(sourceHash);
                            var targetPoint = new Point3D
                            {
                                X = sourcePoint.X,
                                Y = sourcePoint.Y,
                                Z = sourcePoint.Z,
                                W = sourcePoint.W
                            };

                            if ((isActive && (activeNeighbours == 2 || activeNeighbours == 3))
                                || (!isActive && (activeNeighbours == 3)))
                            {
                                if (targetPoint.X < newMinX)
                                    newMinX = targetPoint.X;
                                if (targetPoint.X > newMaxX)
                                    newMaxX = targetPoint.X;
                                if (targetPoint.Y < newMinY)
                                    newMinY = targetPoint.Y;
                                if (targetPoint.Y > newMaxY)
                                    newMaxY = targetPoint.Y;
                                if (targetPoint.Z < newMinZ)
                                    newMinZ = targetPoint.Z;
                                if (targetPoint.Z > newMaxZ)
                                    newMaxZ = targetPoint.Z;
                                if (targetPoint.W < newMinW)
                                    newMinW = targetPoint.W;
                                if (targetPoint.W > newMaxW)
                                    newMaxW = targetPoint.W;

                                nextPoints.Add(sourceHash, targetPoint);
                            }
                        }

            _minX = newMinX;
            _maxX = newMaxX;
            _minY = newMinY;
            _maxY = newMaxY;
            _minZ = newMinZ;
            _maxZ = newMaxZ;
            _minW = newMinW;
            _maxW = newMaxW;

            return nextPoints;
        }
    }
}
