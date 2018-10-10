﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DuckyBot.Core.Modules.Events.MessageReceived
{
    internal class Whiskers : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    {
        public static async Task GiveFeels(SocketMessage arg)
        {
            if (arg.Author.Id == 193075985267163136) // if whiskers types
            {
                var message = arg.ToString().ToLowerInvariant();

                if (message.StartsWith("!") || message.StartsWith(":") && message.EndsWith(":") || message.StartsWith("https://"))
                {
                    return; // make sure its not a command, emote or url link
                }
                if (message.Contains("girl") || message.Contains("girlfriend") || message.Contains("wife") || message.Contains("gf") || message.Contains("woman") || message.Contains("women") || message.Contains("married") || message.Contains("marry") || message.Contains("marrying"))
                {
                    var usermsg = arg as IUserMessage;
                    var emote = Emote.Parse("<:feels:346348418702245888>");
                    await Task.Delay(1500).ConfigureAwait(false);
                    if (usermsg != null) await usermsg.AddReactionAsync(emote);
                }
            }
        }
    }
}
