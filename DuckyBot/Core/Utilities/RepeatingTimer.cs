using Discord.WebSocket;
using DuckyBot.Core.Main;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace DuckyBot.Core.Utilities
{
    internal static class RepeatingTimer
    {
        private static Timer loopingTimer;
        private static SocketTextChannel channel;

        internal static Task StartTimer()
        {
            channel = Global.Client.GetGuild(307712604904620034).GetTextChannel(475222498175352834);
            loopingTimer = new Timer()
            {
                Interval = 1800000, // every half hour 1800000
                AutoReset = true,
                Enabled = true
            };

            loopingTimer.Elapsed += OnTimerTicked;
            return Task.CompletedTask;
        }

        private static async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            if (Global.Client == null)
            {
                Console.WriteLine("Timer ticked before the client was ready."); // error checking
                return;
            }
			// post hydration reminder
            // await channel.SendMessageAsync(":droplet: Time to drink water!");
        }
    }
}
