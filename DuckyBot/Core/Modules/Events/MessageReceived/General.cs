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
            if (msg.Author.IsBot)
            {
                return; // make sure its not a bot account
            }
            var message = msg.ToString().ToLowerInvariant(); // turn users message input to lowercase - less stressful to check with the array
            if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://"))
            {
                return; // make sure its not a command, emote or url link
            }

            if (gachiArray.Any(message.Contains)) // if any of the gachi words/phrases are detected
            {
                var usermsg = msg as IUserMessage;
                var emote = Emote.Parse("<:gachiGASM:448977978236600320>"); // react with gachiGASM
                await Task.Delay(1500).ConfigureAwait(false);
                if (usermsg != null)
                {
                    await usermsg.AddReactionAsync(emote);
                }
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
        public static async Task JackFrags(SocketMessage msg)
        {
            if (msg.Author.IsBot)
            {
                return; // make sure its not a bot account
            }
            if (msg.Content.StartsWith("!") || msg.Content.StartsWith(":") || msg.Content.StartsWith("https://"))
            {
                return; // make sure its not a command, emote or url link
            }

            var messageCheck = msg.Content.ToLowerInvariant();
            if (messageCheck.Contains("jack frags"))
            {
                await Task.Delay(1500).ConfigureAwait(false);
                await msg.Channel.SendMessageAsync("Cd1 dde. FaZZXZÅ gjøre noe båttur og det var Nice i stuen og gå av når du spurte oss");
            }
        }
    }
}
