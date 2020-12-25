using System;
using System.Collections.Generic;
using System.Text;

namespace day25
{
    public class FirstStar
    {
        public static string Run()
        {
            long publicKey1 = 14788856;
            long publicKey2 = 19316454;

            long loopSize = GetLoopSize(7, publicKey1);
            long encKey = CalculateEncryptionKey(publicKey2, loopSize);

            return encKey.ToString();
        }

        private static long GetLoopSize(long subjectNumber, long publicKey)
        {
            int loop = 0;
            long value = 1;

            do
            {
                value *= subjectNumber;
                value %= 20201227;
                loop++;
            } while (value != publicKey);

            return loop;
        }

        private static long CalculateEncryptionKey(long subjectNumber, long loopSize)
        {
            long value = 1;
            int loop = 0;

            while (true)
            {
                value *= subjectNumber;
                value %= 20201227;
                loop++;
                if (loop == loopSize)
                    return value;
            }
        }
    }
}
