using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using DuckyBot.Core.Utilities;

namespace DuckyBot.Core.Modules.Events.MessageReceived
{
    internal class Boon : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    {
        public static async Task GivePepega(SocketMessage msg)
        {
            if (msg.Author.Id == UserIDs.Boon) // if boon types
            {
                var usermsg = msg as IUserMessage;
                var Pepega1 = Emote.Parse("<:Pepega:504690810143375372>"); // give him pepega reaction
                var Pepega2 = Emote.Parse("<:Pepega:537678417638719488>"); 
                var Pepega3 = Emote.Parse("<:Pepega:504696606487216128>"); 
                await Task.Delay(2000).ConfigureAwait(false);
                if (usermsg != null)
                {
                    await Task.Delay(1500).ConfigureAwait(false);
                    await usermsg.AddReactionAsync(Pepega1);
                    await Task.Delay(1500).ConfigureAwait(false);
                    await usermsg.AddReactionAsync(Pepega2);
                    await Task.Delay(1500).ConfigureAwait(false);
                    await usermsg.AddReactionAsync(Pepega3);
                }
            }
        }
    }
}
