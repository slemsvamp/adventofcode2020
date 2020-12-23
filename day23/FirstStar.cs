using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace day23
{
    public class FirstStar
    {
        private const int ONE_HUNDRED = 100;
        private static int minValue = int.MaxValue;
        private static int maxValue = int.MinValue;

        public static string Run()
        {
            int[] cupArray = "364289715".ToCharArray().Select(c => int.Parse($"{c}")).ToArray();

            var cups = new Dictionary<int, Cup>();

            ArrangeCups(cupArray, cups);

            int currentCup = cups[cupArray[0]].Value;

            CupShuffle(currentCup, cups, ONE_HUNDRED);

            var current = cups[1].Next;
            string result = string.Empty;

            while (current.Value != 1)
            {
                result += current.Value.ToString();
                current = current.Next;
            }

            return result;
        }

        private static void CupShuffle(int currentCup, Dictionary<int, Cup> cups, int moves)
        {
            for (int move = 0; move < moves; move++)
            {
                var cup1 = cups[currentCup].Next;
                var cup2 = cup1.Next;
                var cup3 = cup2.Next;

                var label = cups[currentCup].Value;
                Cup destinationCup = null;

                while (destinationCup == null || destinationCup.Value == cup1.Value || destinationCup.Value == cup2.Value || destinationCup.Value == cup3.Value)
                {
                    label--;
                    if (label < minValue)
                        label = maxValue;
                    destinationCup = cups[label];
                }

                cups[currentCup].Next = cup3.Next;
                (cup3.Next).Previous = cups[currentCup];

                cup1.Previous = destinationCup;
                cup3.Previous = cups[currentCup];
                cup3.Next = destinationCup.Next;

                destinationCup.Next = cup1;

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

                if (value > maxValue)
                    maxValue = value;
                if (value < minValue)
                    minValue = value;
            }

            cups[cupArray[0]].Previous = cups[cupArray[cupArray.Length - 1]];
            cups[cupArray[cupArray.Length - 1]].Next = cups[cupArray[0]];
        }
    }
}
