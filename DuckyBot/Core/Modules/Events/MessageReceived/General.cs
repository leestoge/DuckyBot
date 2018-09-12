using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DuckyBot.Core.Modules.Events.MessageReceived
{
    public static class General
    {
        public static async Task Gay(SocketMessage msg)
        {
            if (msg.Author.IsBot) return; // make sure its not a bot account
            var message = msg.ToString().ToLower();
            if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://")) return; // make sure its not a command, emote or url link
            //string nospaces = Regex.Replace(message, @" ", "");

            if (message.Contains("gay") || message.Contains("suction") || message.Contains("dungeon") && message.Contains("master") || message.Contains("my") && message.Contains("shoulder"))
            {
                var usermsg = msg as IUserMessage;
                Emote emote = Emote.Parse("<:gachiGASM:448977978236600320>");
                await Task.Delay(1500);
                await usermsg.AddReactionAsync(emote);
                return;
            }
        }
        public static async Task WhoDidThis(SocketMessage msg)
        {
            if (msg.Author.IsBot) return; // make sure its not a bot account
            var message = msg.ToString().ToLower();
            if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://")) return; // make sure its not a command, emote or url link
            //string nospaces = Regex.Replace(message, @" ", "");

            if (message.Contains("who") && message.Contains("did") && message.Contains("this"))
            {
                var usermsg = msg as IUserMessage;
                var emote = new Emoji("😂");
                await Task.Delay(1500);
                await usermsg.AddReactionAsync(emote);
                return;
            }
        }
        public static async Task GoodBot(SocketMessage msg)
        {
            if (msg.Author.IsBot) return; // make sure its not a bot account
            var message = msg.ToString().ToLower();
            if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://")) return; // make sure its not a command, emote or url link
            //string nospaces = Regex.Replace(message, @" ", "");

            if (message.Contains("good") && message.Contains("bot"))
            {
                await Task.Delay(1500);
                await msg.Channel.SendMessageAsync("Good human.");
                return;
            }
        }
        public static async Task BadBot(SocketMessage msg)
        {
            if (msg.Author.IsBot) return; // make sure its not a bot account
            var message = msg.ToString().ToLower();
            if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://")) return; // make sure its not a command, emote or url link
            //string nospaces = Regex.Replace(message, @" ", "");

            if (message.Contains("bad") && message.Contains("bot"))
            {
                await Task.Delay(1500);
                await msg.Channel.SendMessageAsync("Bad human.");
                return;
            }
        }
        public static async Task HempusJibberish(SocketMessage msg)
        {
            if (msg.Author.IsBot) return; // make sure its not a bot account
            if (msg.Content.StartsWith("!") || msg.Content.StartsWith(":") || msg.Content.StartsWith("https://")) return; // make sure its not a command, emote or url link
            //string nospaces = Regex.Replace(msg, @" ", "");

            if (msg.Content.Contains("Cd1 dde. FaZZXZÅ gjøre noe båttur og det var Nice i stuen og gå av når du spurte oss"))
            {
                await Task.Delay(1500);
                await msg.Channel.SendMessageAsync("Cd1 dde. FaZZXZÅ gjøre noe båttur og det var Nice i stuen og gå av når du spurte oss");
                return;
            }
        }
        public static async Task Cummo(SocketMessage msg)
        {
            if (msg.Author.IsBot) return; // make sure its not a bot account
            var message = msg.ToString().ToLower();
            if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://")) return; // make sure its not a command, emote or url link
            //string nospaces = Regex.Replace(message, @" ", "");

            if (message.Contains("cummo") || message.Contains("sticky") && message.Contains("uh") || message.Contains("stiffy") && message.Contains("uh"))
            {
                var usermsg = msg as IUserMessage;
                Emote emote = Emote.Parse("<:CUMMO:428285290193616896>");
                await Task.Delay(1500);
                await usermsg.AddReactionAsync(emote);
                return;
            }
        }
    }
}
