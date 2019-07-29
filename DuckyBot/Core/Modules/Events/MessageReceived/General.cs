using Discord;
using Discord.WebSocket;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static DuckyBot.Core.Utilities.RandomGen;

namespace DuckyBot.Core.Modules.Events.MessageReceived
{
    public static class General
    {
        public static async Task Gay(SocketMessage msg)
        {
            if (msg.Author.IsBot)
            {
                return; // make sure its not a bot account
            }

            var message = msg.ToString().ToLowerInvariant(); // turn users message input to lowercase - less stressful to check with the array

            if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://"))
            {
                return; // make sure its not a command, emote or url link
            }

            bool containsGachi = Regex.IsMatch(message, @"\b(deep dark fantasies|come on college boy|suction|dungeon master|my shoulder|that turns me on|boss of this gym|get your ass back here|boy next door|take it boy)\b");

            if (containsGachi) // if any of the gachi words/phrases are detected
            {
                var usermsg = msg as IUserMessage;
                var emote = Emote.Parse("<:gachiGASM:448977978236600320>"); // react with gachiBASS ?
                await Task.Delay(1500).ConfigureAwait(false);
                if (usermsg != null)
                {
                    await usermsg.AddReactionAsync(emote);
                }
            }
        }

        public static async Task JackFrags(SocketMessage msg)
        {
            if (msg.Author.IsBot)
            {
                return; // make sure its not a bot account
            }

            var message = msg.ToString().ToLowerInvariant(); // turn users message input to lowercase - less stressful to check with the array

            if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://"))
            {
                return; // make sure its not a command, emote or url link
            }

            bool containsJackFragsNonsense = Regex.IsMatch(message, @"\b(jack frags here|i'll see you guys in the next one|ill see you guys in the next one|best infantry game|dislike if you didnt|dislike if you disliked it|dislike the video if you didnt like it|dislike the video if you didn't like it|like if you liked it|like the video if you liked it|is it still good|back to battlefield|the thompson|is it any good|left side or right side|full left side|full right side|sometimes i go left side|sometimes i go right side)\b");

            var gameClickbait = new[] // array of strings
            {
                "Battlefield 4",
                "Battlefield 3",
                "Battlefield V",
                "Apex Legends",
                "PUBG",
                "Bad Company 2",
                "Battlefield One",
                "Battlefield 1",
                "Star Wars Battlefront 2",
            };

            var replies = new[] // array of strings
            {
                "Like if you liked it",
                "Dislike if you disliked it",
                "Jack Frags here",
                $"And today, we're playing {gameClickbait[Instance.Next(gameClickbait.Length)]}",
                "I'll see you guys in the next one",
                "Battlefield 4 Best Infantry Game",
                $"{gameClickbait[Instance.Next(gameClickbait.Length)]} Is it still good?",
                $"Back to {gameClickbait[Instance.Next(gameClickbait.Length)]}",
                "But is it any good?",
                "The Thompson",
                $"{gameClickbait[Instance.Next(gameClickbait.Length)]} has a problem",
                "Bad Company 3 Leak",
                "Hey, guys, Jack here!",
                "DICE please",
                $"How wrong was I about {gameClickbait[Instance.Next(gameClickbait.Length)]}",
                $"{gameClickbait[Instance.Next(gameClickbait.Length)]} - Is it any good?",
                $"{gameClickbait[Instance.Next(gameClickbait.Length)]} is back on track",
                $"{gameClickbait[Instance.Next(gameClickbait.Length)]} Pro Tips to make you a better player",
                $"{gameClickbait[Instance.Next(gameClickbait.Length)]} Pro Tips and Secrets",
                $"{gameClickbait[Instance.Next(gameClickbait.Length)]} Shotgun Buff",
                $"When {gameClickbait[Instance.Next(gameClickbait.Length)]} players Rage",
                "Sometimes I go left side, but then other times I go right side",
                "I go full left side on this one",
                "I go full right side on this one",
            };

            var rand = Instance.Next(replies.Length); // get random number between 0 and array length
            var text = replies[rand]; // store string at the random number position in the array

            if (containsJackFragsNonsense) // if any of the gachi words/phrases are detected
            {
                await Task.Delay(1500).ConfigureAwait(false);
                await msg.Channel.SendMessageAsync(text);
            }
        }

        public static async Task HempusJibberish(SocketMessage msg)
        {
            if (msg.Author.IsBot)
            {
                return; // make sure its not a bot account
            }

            if (msg.Content.StartsWith("!") || msg.Content.StartsWith(":") || msg.Content.StartsWith("https://"))
            {
                return; // make sure its not a command, emote or url link
            }

            if (msg.Content.Contains("Cd1 dde. FaZZXZÅ gjøre noe båttur og det var Nice i stuen og gå av når du spurte oss"))
            {
                await Task.Delay(1500).ConfigureAwait(false);
                await msg.Channel.SendMessageAsync("Cd1 dde. FaZZXZÅ gjøre noe båttur og det var Nice i stuen og gå av når du spurte oss");
            }
        }
    }
}
