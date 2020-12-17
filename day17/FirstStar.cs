using System;
using System.Collections.Generic;
using System.Text;

namespace day17
{
    public class FirstStar
    {
        private static int[] _dirX = new[] { -1, 0, 1 };
        private static int[] _dirY = new[] { -1, 0, 1 };
        private static int[] _dirZ = new[] { -1, 0, 1 };

        private struct Point3D
        {
            public int X;
            public int Y;
            public int Z;

            public static Point3D Empty
                => new Point3D { X = 0, Y = 0, Z = 0 };

            public long Hash
                => (X * 18397) + (Y * 20483) + (Z * 29303);
        }

        private static int _minX;
        private static int _maxX;
        private static int _minY;
        private static int _maxY;
        private static int _minZ;
        private static int _maxZ;

        public static string Run(string[] cubeSlice)
        {
            var points = new Dictionary<long, Point3D>();

            for (var y = 0; y < cubeSlice.Length; y++)
                for (var x = 0; x < cubeSlice[0].Length; x++)
                {
                    if (cubeSlice[y][x] == '#')
                    {
                        var point = new Point3D
                        {
                            X = x,
                            Y = y,
                            Z = 0
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

        private static Dictionary<long, Point3D> Cycle(Dictionary<long, Point3D> points)
        {
            var nextPoints = new Dictionary<long, Point3D>();

            int newMinX = _minX;
            int newMaxX = _maxX;
            int newMinY = _minY;
            int newMaxY = _maxY;
            int newMinZ = _minZ;
            int newMaxZ = _maxZ;

            for (int sourceZ = _minZ - 1; sourceZ <= _maxZ + 1; sourceZ++)
                for (int sourceY = _minY - 1; sourceY <= _maxY + 1; sourceY++)
                    for (int sourceX = _minX - 1; sourceX <= _maxX + 1; sourceX++)
                    {
                        var sourcePoint = new Point3D
                        {
                            X = sourceX,
                            Y = sourceY,
                            Z = sourceZ
                        };
                        var sourceHash = sourcePoint.Hash;

                        int activeNeighbours = 0;

                        for (int z = 0; z < 3; z++)
                            for (int y = 0; y < 3; y++)
                                for (int x = 0; x < 3; x++)
                                {
                                    var point = new Point3D
                                    {
                                        X = sourcePoint.X + _dirX[x],
                                        Y = sourcePoint.Y + _dirY[y],
                                        Z = sourcePoint.Z + _dirZ[z]
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
                            Z = sourcePoint.Z
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
                        
                            nextPoints.Add(sourceHash, targetPoint);
                        }
                    }

            _minX = newMinX;
            _maxX = newMaxX;
            _minY = newMinY;
            _maxY = newMaxY;
            _minZ = newMinZ;
            _maxZ = newMaxZ;

            return nextPoints;
        }
    }
}
