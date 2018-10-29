using System;
using System.Threading.Tasks;
using System.Timers;
using DuckyBot.Core.Main;
using static DuckyBot.Core.Utilities.RandomGen;

namespace DuckyBot.Core.Utilities
{
    internal static class RepeatingTimer
    {
        internal static Task StartTimer()
        {
            var loopingTimer = new Timer
            {
                Interval = 1800000, // every half hour
                AutoReset = true,
                Enabled = true
            };

            loopingTimer.Elapsed += OnTimerTickedAsync;
            return Task.CompletedTask;
        }

        private static async void OnTimerTickedAsync(object sender, ElapsedEventArgs e)
        {
            var predictionsTexts = new[] // array of strings called "predictionsTexts"
            {
                "flingmeoff mingoff",
                "with CPU cooling liquid",
                "DuckyBot | !help",
                "with a long john",
                "with a fast pounding long john",
                "with eggs",
                "with a long john teleporter",
            };
            var rand = Instance.Next(predictionsTexts.Length); // get random number between 0 and array length
            var text = predictionsTexts[rand]; // store string at the random number position in the array
            if (Global.Client == null)
            {
                Console.WriteLine("Timer ticked before the client was ready."); // error checking
            }

            if (Global.Client != null)
            {
                await Global.Client.SetGameAsync(text);
            }
        }
    }
}
