using System;

namespace DutchCallouts
{
    internal static class Common
    {
        public static Random rand;
        static Common()
        {
            Common.rand = new Random();
        }
    }
}