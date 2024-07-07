using System;

namespace RecordMyStats.xTests
{
    internal class TestUtils
    {
        private static Random rnd = new Random();

        internal static string UniqueValue()
        {
            return rnd.Next(10000).ToString();
        }
        internal static int UniqueValueSmall()
        {
            return rnd.Next(30);
        }
    }
}
