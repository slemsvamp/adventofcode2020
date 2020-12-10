using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day10
{
    public class SecondStar
    {
        private static Dictionary<int, long> _memo;
        private static int[] _adapters;

        public static string Run(List<int> jolts)
        {
            int index = 1;

            _adapters = new int[jolts.Count + 2];
            _memo = new Dictionary<int, long>();

            _adapters[0] = 0;
            foreach (var jolt in jolts.OrderBy(j => j))
                _adapters[index++] = jolt;

            _adapters[jolts.Count + 1] = jolts.Max() + 3;

            var permutations = RecursiveCrawl(0);

            return permutations.ToString();
        }

        public static long RecursiveCrawl(int index)
        {
            if (index == _adapters.Length - 1)
                return 1;
            if (_memo.ContainsKey(index))
                return _memo[index];
            long ans = 0;
            for (int subIndex = index + 1; subIndex < _adapters.Length; subIndex++)
                if (_adapters[subIndex] - _adapters[index] <= 3)
                    ans += RecursiveCrawl(subIndex);

            _memo[index] = ans;
            return ans;
        }
    }
}
