using System;
using System.Collections.Generic;
using System.Text;

namespace day15
{
    public static class AgeGamePlayer
    {
        private const int NOTHING = -1337;

        private static Dictionary<int, int[]> _timestamps;
        private static int _lastSpoken;

        public static string Run(List<int> startingAgeList, int targetTimestamp)
        {
            _lastSpoken = NOTHING;
            _timestamps = new Dictionary<int, int[]>();

            for (int speak = 1; speak <= startingAgeList.Count; speak++)
                Speak(speak, startingAgeList[speak - 1]);

            for (int timestamp = startingAgeList.Count + 1; timestamp <= targetTimestamp; timestamp++)
                if (_timestamps.ContainsKey(_lastSpoken) && _timestamps[_lastSpoken][1] == NOTHING)
                    Speak(timestamp, 0);
                else
                    Speak(timestamp, _timestamps[_lastSpoken][0] - _timestamps[_lastSpoken][1]);

            return _lastSpoken.ToString();
        }

        private static void Speak(int timestamp, int number)
        {
            if (!_timestamps.ContainsKey(number))
                _timestamps.Add(number, new int[2] { timestamp, NOTHING });
            else
                (_timestamps[number][0], _timestamps[number][1]) = (timestamp, _timestamps[number][0]);
            _lastSpoken = number;
        }
    }
}
