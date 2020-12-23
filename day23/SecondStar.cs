using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day23
{
    public class SecondStar
    {
        public const int ONE_MILLION = 1_000_000;
        public const int TEN_MILLION = 10_000_000;

        private static int _minValue = int.MaxValue;
        private static int _maxValue = int.MinValue;

        public static string Run(object parameter)
        {
            int[] cupArray = "364289715".ToCharArray().Select(c => int.Parse($"{c}")).ToArray();
            var cups = new Dictionary<int, Cup>();

            ArrangeCups(cupArray, cups);

            int currentCupValue = cups[cupArray[0]].Value;

            CupShuffle(currentCupValue, cups, TEN_MILLION);

            long result = (long)cups[1].Next.Value * cups[1].Next.Next.Value;

            return result.ToString();
        }

        private static void CupShuffle(int currentCup, Dictionary<int, Cup> cups, int movesToComplete)
        {
            for (int move = 0; move < movesToComplete; move++)
            {
                var cup1 = cups[currentCup].Next;
                var cup2 = cup1.Next;
                var cup3 = cup2.Next;

                var label = cups[currentCup].Value;
                Cup destinationCup = null;
                while (destinationCup == null || destinationCup.Value == cup1.Value || destinationCup.Value == cup2.Value || destinationCup.Value == cup3.Value)
                {
                    label--;
                    if (label < _minValue)
                        label = _maxValue;
                    destinationCup = cups[label];
                }

                var cn = cup3.Next;
                cups[currentCup].Next = cn;
                cn.Previous = cups[currentCup];

                var dn = destinationCup.Next;
                cup3.Next = dn;
                dn.Previous = cup3;

                destinationCup.Next = cup1;
                cup1.Previous = destinationCup;

                currentCup = cups[currentCup].Next.Value;
            }
        }

        private static void ArrangeCups(int[] cupArray, Dictionary<int, Cup> cups)
        {
            for (var cupIndex = 0; cupIndex < cupArray.Length; cupIndex++)
            {
                var value = cupArray[cupIndex];
                cups.Add(value, new Cup()
                {
                    Value = value,
                    Previous = cupIndex > 0 ? cups[cupArray[cupIndex - 1]] : null,
                    Next = null
                });

                if (cupIndex > 0)
                    cups[cupArray[cupIndex - 1]].Next = cups[cupArray[cupIndex]];

                if (value > _maxValue)
                    _maxValue = value;
                if (value < _minValue)
                    _minValue = value;
            }
            
            Cup previous = cups[cupArray[cupArray.Length - 1]];
            int nextValue = _maxValue + 1;

            while (cups.Count < ONE_MILLION)
            {
                var nextCup = new Cup()
                {
                    Value = nextValue,
                    Previous = previous,
                    Next = null
                };

                cups.Add(nextValue, nextCup);
                previous.Next = nextCup;
                previous = nextCup;

                if (nextValue > _maxValue)
                    _maxValue = nextValue;

                nextValue++;
            }

            cups[cupArray[0]].Previous = cups[nextValue - 1];
            cups[nextValue - 1].Next = cups[cupArray[0]];
        }
    }
}
