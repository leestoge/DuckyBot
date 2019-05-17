using System.Text.RegularExpressions;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using DuckyBot.Core.Utilities;

namespace DuckyBot.Core.Modules.Events.MessageReceived
{
    internal class Trucks : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    {
        public static async Task GiveDonut(SocketMessage msg)
        {       
            if (msg.Author.Id == UserIDs.trucks) // if trucks types
            {
                var message = msg.ToString().ToLowerInvariant();

                if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://"))
                {
                    return; // make sure its not a command, emote or url link
                }

                bool contains = Regex.IsMatch(message, @"\b(eat|eating|food|donut|doughnut|having a break|dinner|having break|have a break)\b");

                if (contains) // and it contains these phrases/words
                {
                    var usermsg = msg as IUserMessage;
                    var emote = new Emoji("🍩"); // give him a donut reaction
                    await Task.Delay(1500).ConfigureAwait(false);
                    if (usermsg != null)
                    {
                        await usermsg.AddReactionAsync(emote);
                    }
                }
            }
        }
    }
}
