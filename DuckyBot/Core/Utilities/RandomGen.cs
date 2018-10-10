using System;
using System.Threading;

namespace DuckyBot.Core.Utilities
{
    internal static class RandomGen /* CODE TAKEN FROM STACKOVERFLOW, WRITTEN BY "Alessandro D'Andria" - https://stackoverflow.com/questions/19270507/correct-way-to-use-random-in-multithread-application */
    {
        private static int _seed;

        private static readonly ThreadLocal<Random> ThreadLocal =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));

        static RandomGen()
        {
            _seed = Environment.TickCount; // Seed generated from PC up-time
        }

        public static Random Instance => ThreadLocal.Value;
    }
}