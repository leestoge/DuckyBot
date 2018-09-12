using System;
using System.Threading;

namespace DuckyBot.Core.Utilities
{
    class RandomGen
    {
        public static class StaticRandom /* CODE TAKEN FROM STACKOVERFLOW, WRITTEN BY "Alessandro D'Andria" - https://stackoverflow.com/questions/19270507/correct-way-to-use-random-in-multithread-application */
        {
            private static int seed;

            private static ThreadLocal<Random> threadLocal = new ThreadLocal<Random>
                (() => new Random(Interlocked.Increment(ref seed)));

            static StaticRandom()
            {
                seed = Environment.TickCount; // Seed generated from PC uptime
            }

            public static Random Instance { get { return threadLocal.Value; } }
        }
    }
}