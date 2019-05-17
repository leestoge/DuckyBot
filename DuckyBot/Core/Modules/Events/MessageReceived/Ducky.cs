using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using DuckyBot.Core.Utilities;

namespace DuckyBot.Core.Modules.Events.MessageReceived
{
    internal class Ducky : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    {
        public static async Task GiveKisses(SocketMessage arg)
        {
            if (arg.Author.Id == UserIDs.Ducky) // if ducky types
            {
                var message = arg.ToString().ToLowerInvariant();

                if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://"))
                {
                    return; // make sure its not a command, emote or url link
                }

                if (message.Contains("facebook"))
                {
                    await Task.Delay(1500).ConfigureAwait(false);
                    await arg.Channel.SendMessageAsync(arg.Author.Mention + " :point_right: :door:  ");
                }
            }
        }
    }
}
