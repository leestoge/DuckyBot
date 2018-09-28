using Discord.WebSocket;
using DuckyBot.Core.Main;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace DuckyBot.Core.Utilities
{
    internal static class RepeatingTimer
    {
        private static Timer _loopingTimer;
        private static SocketTextChannel _channel;

        internal static Task StartTimer()
        {
            _channel = Global.Client.GetGuild(307712604904620034).GetTextChannel(475222498175352834);
            _loopingTimer = new Timer()
            {
                Interval = 1800000, // every half hour 1800000
                AutoReset = true,
                Enabled = true
            };

            _loopingTimer.Elapsed += OnTimerTicked;
            return Task.CompletedTask;
        }

        private static async void OnTimerTicked(object sender, ElapsedEventArgs e)
        {
            if (Global.Client == null)
            {
                Console.WriteLine("Timer ticked before the client was ready."); // error checking
            }
			// post hydration reminder
            // await channel.SendMessageAsync(":droplet: Time to drink water!");
        }
    }
}
