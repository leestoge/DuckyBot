using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DuckyBot.Core.Modules.Events.MessageReceived
{
    internal class Trucks : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    {
        public static async Task GiveDonut(SocketMessage arg)
        {
            if (arg.Author.Id == 98511845438009344) // if trucks types
            {
                var message = arg.ToString().ToLowerInvariant();

                if (message.StartsWith("!") || message.StartsWith(":") && message.EndsWith(":") || message.StartsWith("https://"))
                {
                    return; // make sure its not a command, emote or url link
                }
                if (message.Contains("eat") || message.Contains("eating") || message.Contains("food") || message.Contains("donut") || message.Contains("doughnut")) // and it contains these phrases/words
                {
                    var usermsg = arg as IUserMessage;
                    var emote = new Emoji("🍩"); // give him a donut reaction
                    await Task.Delay(1500).ConfigureAwait(false);
                    if (usermsg != null) await usermsg.AddReactionAsync(emote);
                }
            }
        }
    }
}
