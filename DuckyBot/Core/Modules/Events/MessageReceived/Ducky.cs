using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DuckyBot.Core.Modules.Events.MessageReceived
{
    internal class Ducky : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    {
        public static async Task GiveKisses(SocketMessage arg)
        {
            if (arg.Author.Id == 98543769795723264) // if ducky types
            {
                var message = arg.ToString();
                if (message.StartsWith("!") || message.StartsWith(":") || message.StartsWith("https://"))
                {
                    return; // make sure its not a command, emote or url link
                }

                if (message.EndsWith(" xxx"))
                {
                    await Task.Delay(1500);
                    await arg.Channel.SendMessageAsync(arg.Author.Mention + " :kissing_closed_eyes: :kissing_closed_eyes: :kissing_closed_eyes: ");
                }
            }
        }
    }
}
