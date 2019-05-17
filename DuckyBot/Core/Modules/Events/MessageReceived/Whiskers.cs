using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using DuckyBot.Core.Utilities;

namespace DuckyBot.Core.Modules.Events.MessageReceived
{
    internal class Whiskers : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    {
        public static async Task GiveFeels(SocketMessage msg)
        {
            if (msg.Author.Id == UserIDs.Whiskers) // if whiskers types
            {
                var message = msg.ToString().ToLowerInvariant();

                if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://"))
                {
                    return; // make sure its not a command, emote or url link
                }

                if (message.Contains("girl") || message.Contains("girlfriend") || message.Contains("wife") || message.Contains("gf") || message.Contains("woman") || message.Contains("women") || message.Contains("married") || message.Contains("marry") || message.Contains("marrying"))
                {
                    var usermsg = msg as IUserMessage;
                    var emote = Emote.Parse("<:feels:346348418702245888>");
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
