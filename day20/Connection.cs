using System;
using System.Collections.Generic;
using System.Text;

namespace day20
{
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
}
