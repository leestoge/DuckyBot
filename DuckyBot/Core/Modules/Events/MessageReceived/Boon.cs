using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DuckyBot.Core.Modules.Events.MessageReceived
{
    internal class Boon : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    {
        public static async Task GivePepega(SocketMessage msg)
        {
            if (msg.Author.Id == 200026014032592897) // if boon types
            {
                var usermsg = msg as IUserMessage;
                var Pepega1 = Emote.Parse("<:Pepega:504690810143375372>"); // give him pepega reaction
                var Pepega2 = Emote.Parse("<:Pepega:507969601556971540>"); // give him pepega reaction
                await Task.Delay(2000).ConfigureAwait(false);
                if (usermsg != null)
                {
                    await usermsg.AddReactionAsync(Pepega1);
                    await Task.Delay(1500).ConfigureAwait(false);
                    await usermsg.AddReactionAsync(Pepega2);
                }
            }
        }
    }
}
