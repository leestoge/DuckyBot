using System;
using System.Threading.Tasks;
using System.Timers;
using DuckyBot.Core.Main;

namespace DuckyBot.Core.Utilities
{
    internal static class RepeatingTimer
    {
        internal static Task StartTimer()
        {
            var channel = Global.Client.GetGuild(307712604904620034).GetTextChannel(475222498175352834);
            var loopingTimer = new Timer
            {
                Interval = 1800000, // every half hour 1800000
                AutoReset = true,
                Enabled = true
            };

            loopingTimer.Elapsed += OnTimerTicked;
            return Task.CompletedTask;
        }

        private static void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            if (Global.Client == null)
            {
                Console.WriteLine("Timer ticked before the client was ready."); // error checking
            }
            // post hydration reminder
        }
    }
}
