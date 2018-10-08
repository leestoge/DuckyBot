using System.Linq;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DuckyBot.Core.Modules.Events.MessageReceived
{
    public static class General
    {
        public static async Task Gay(SocketMessage msg)
        {
            string[] gachiArray = { "deep dark fantasies", "come on college boy", "suction", "dungeon master", "my shoulder", "that turns me on", "boss of this gym", "slave get your ass back here", "get your ass back here", "boy next door", "take it boy" };
            if (msg.Author.IsBot) return; // make sure its not a bot account
            var message = msg.ToString().ToLower(); // turn users message input to lowercase - less stressful to check with the array
            if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://")) return; // make sure its not a command, emote or url link

            if (gachiArray.Any(message.Contains)) // if any of the gachi words/phrases are detected
            {
                var usermsg = msg as IUserMessage;
                var emote = Emote.Parse("<:gachiGASM:448977978236600320>"); // react with gachiGASM
                await Task.Delay(1500);
                if (usermsg != null) await usermsg.AddReactionAsync(emote);
            }
        }
        public static async Task WhoDidThis(SocketMessage msg)
        {
            if (msg.Author.IsBot) return; // make sure its not a bot account
            var message = msg.ToString().ToLower();
            if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://")) return; // make sure its not a command, emote or url link

            if (message.Contains("who did this")) // if it contains all 3, highly unlikely to trigger randomly
            {
                var usermsg = msg as IUserMessage;
                var emote = new Emoji("😂");
                await Task.Delay(1500);
                if (usermsg != null) await usermsg.AddReactionAsync(emote);
            }
        }
        public static async Task GoodBot(SocketMessage msg)
        {
            if (msg.Author.IsBot) return; // make sure its not a bot account
            var message = msg.ToString().ToLower();
            if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://")) return; // make sure its not a command, emote or url link

            if (message.StartsWith("good") && message.EndsWith("bot") || message.Contains("good bot"))
            {
                await Task.Delay(1500);
                await msg.Channel.SendMessageAsync("Good human.");
            }
        }
        public static async Task BadBot(SocketMessage msg)
        {
            if (msg.Author.IsBot) return; // make sure its not a bot account
            var message = msg.ToString().ToLower();
            if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://")) return; // make sure its not a command, emote or url link

            if (message.StartsWith("bad") && message.EndsWith("bot") || message.Contains("bad bot"))
            {
                await Task.Delay(1500);
                await msg.Channel.SendMessageAsync("Bad human.");
            }
        }
        public static async Task HempusJibberish(SocketMessage msg)
        {
            if (msg.Author.IsBot) return; // make sure its not a bot account
            if (msg.Content.StartsWith("!") || msg.Content.StartsWith(":") || msg.Content.StartsWith("https://")) return; // make sure its not a command, emote or url link

            if (msg.Content.Contains("Cd1 dde. FaZZXZÅ gjøre noe båttur og det var Nice i stuen og gå av når du spurte oss"))
            {
                await Task.Delay(1500);
                await msg.Channel.SendMessageAsync("Cd1 dde. FaZZXZÅ gjøre noe båttur og det var Nice i stuen og gå av når du spurte oss");
            }
        }
        public static async Task Cummo(SocketMessage msg)
        {
            string[] cummoArray = { "cummo", "sticky uh", "stiffy uh", "blicky uh", "iffy uh", "icky uh", "sticky uh", "gay gang", "blicky", "flicky", "flicky uh" };
            if (msg.Author.IsBot) return; // make sure its not a bot account
            var message = msg.ToString().ToLower();
            if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://")) return; // make sure its not a command, emote or url link

            if (cummoArray.Any(message.Contains)) // if any of the cummo words/phrases are detected
            {
                var usermsg = msg as IUserMessage;
                var emote = Emote.Parse("<:CUMMO:428285290193616896>");
                await Task.Delay(1500);
                if (usermsg != null) await usermsg.AddReactionAsync(emote);
            }
        }
    }
}
