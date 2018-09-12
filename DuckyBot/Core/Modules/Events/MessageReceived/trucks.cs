using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DuckyBot.Core.Modules.Events.MessageReceived
{
    class trucks : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    {
        public static async Task GiveDonut(SocketMessage arg)
        {
            if (arg.Author.Id == 98511845438009344) // if whiskers types
            {
                var message = arg.ToString().ToLower();
                if (message.StartsWith("!") || message.StartsWith(":") && message.EndsWith(":") || message.StartsWith("https://")) return; // make sure its not a command, emote or url link
                //string nospaces = Regex.Replace(message, @" ", "");

                if (message.Contains("eat") || message.Contains("eating") || message.Contains("food") || message.Contains("donut") || message.Contains("doughnut"))
                {
                    var usermsg = arg as IUserMessage;
                    var emote = new Emoji("🍩");
                    await Task.Delay(1500);
                    await usermsg.AddReactionAsync(emote);
                    return;
                }
            }
            else return;
        }
    }
}
