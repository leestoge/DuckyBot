﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace DuckyBot.Core.Modules.Commands
{
    public class Help : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    //SocketCommandContext allows access to the message, channel, server and user the command was invoked by, so works as the base as to how the bot knows where to reply/what user used which command, etc.
    {
        private CommandService _service; // declared command service - basically command handler

        public Help(CommandService service) // Constructor for the command service dependency
        {
            _service = service; // passed in command service (from command handler) is equal to the privately declared command service above
        }

        [Command("help")] // command declaration
        [Summary("Shows a list of all available commands per module, and a brief description of what they do. :nerd:")] // command summary
        public async Task HelpAsync() // command async task (method basically)
        {
            var dmChannel = await Context.User.GetOrCreateDMChannelAsync(); // A direct messages channel is created so that the !help command content will be privately sent to the user & not flood the chat
            var builder = new EmbedBuilder() // create new embed
            {
                Color = new Color(255, 82, 41), // embed colour (orange)
                Author = new EmbedAuthorBuilder() // create new author within embed (used as a title when displayed)
                {
                    Name = "The DuckyBot commands must be typed precisely as shown below:-", // author text
                    IconUrl = "http://cdn.edgecast.steamstatic.com/steamcommunity/public/images/avatars/ea/ea879dd914a94d7f719bb553306786fa5ae6acb0_full.jpg" // duckybot logo, displayed beside author text
                }
            };

            foreach (var module in _service.Modules) // loop through the modules taken from the command service initiated earlier!
            {
                string description = null; // description defaults to null to ensure no errors occur if the description building fails

                foreach (var cmd in module.Commands) // loop through all the commands per module aswell
                {
                    var result = await cmd.CheckPreconditionsAsync(Context); // gotta check if they pass

                    if (result.IsSuccess) // if they DO pass
                    {
                        description += $"`{"!"}{cmd.Aliases.First()}`" + $" - {cmd.Summary}\n"; // ADD that command's first alias (aka it's actual name) to the description tag of this embed, along with the set bot command prefix and a summary of the command
                    }
                }

                if (!string.IsNullOrWhiteSpace(description)) // if the module wasn't empty add a field where we drop all the command data into!
                {
                    builder.AddField(x => // add field with multiple properties defined
                    {
                        x.Name = module.Name; // module name
                        x.Value = description; // above description format for each command
                        x.IsInline = false; // not inline (so it appears like a list)
                    });
                }
            }
            await dmChannel.SendMessageAsync("", false, builder.Build()); // then send embed to the user in the direct messages channel
            await Context.Channel.SendMessageAsync(Context.User.Mention + " Check your direct messages for a list of my commands! <:duckybot:378960915116064768> "); // reply to user in the channel they used the !help command to notify them of the direct message
        }
        [Command("info")]
        [RequireUserPermission(GuildPermission.Administrator)] ///Needed User Permissions ///
        [Summary("Gives general bot info. (Only Moderators can use this command)")]
        public async Task BotInfo()
        {
            var application = await Context.Client.GetApplicationInfoAsync();
            await ReplyAsync(
                $"{Format.Bold("Info")}\n" +
                $"- Author: {application.Owner.Username} (ID {application.Owner.Id})\n" +
                $"- Library: Discord.Net ({DiscordConfig.Version})\n" +
                $"- Runtime: {RuntimeInformation.FrameworkDescription} {RuntimeInformation.OSArchitecture}\n" +
                $"- Uptime: {GetUptime()}\n\n" +

                $"{Format.Bold("Stats")}\n" +
                $"- Heap Size: {GetHeapSize()} MB\n" +
                $"- Guilds: {(Context.Client as DiscordSocketClient).Guilds.Count}\n" +
                $"- Channels: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Channels.Count)}" +
                $"- Users: {(Context.Client as DiscordSocketClient).Guilds.Sum(g => g.Users.Count)}"
            );
        }
        private static string GetUptime()
            => (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
        private static string GetHeapSize() => Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();

        [Command("ServerInfo")]
        [RequireUserPermission(GuildPermission.Administrator)] ///Needed User Permissions ///
        [Alias("sinfo", "servinfo")]
        [Remarks("Info about a server")]
        [Summary("Gives general server info. (Only Moderators can use this command)")]
        public async Task GuildInfo()
        {
            EmbedBuilder embed;
            embed = new EmbedBuilder();
            embed.WithColor(255, 82, 41); //embed trim colour

            var gld = Context.Guild as SocketGuild;
            var client = Context.Client as DiscordSocketClient;

            if (!string.IsNullOrWhiteSpace(gld.IconUrl))
                embed.ThumbnailUrl = gld.IconUrl;
            var O = gld.Owner.Username;

            var V = gld.VoiceRegionId;
            var C = gld.CreatedAt;
            var N = gld.DefaultMessageNotifications;
            var VL = gld.VerificationLevel;
            var XD = gld.Roles.Count;
            var X = gld.MemberCount;
            var Z = client.ConnectionState;

            embed.Title = $"{gld.Name} Server Information";
            embed.Description = $"Server Owner: **{O}\n**Voice Region: **{V}\n**Created At: **{C}\n**MsgNtfc: **{N}\n**Verification: **{VL}\n**Role Count: **{XD}\n **Members: **{X}\n **Conntection state: **{Z}\n\n**";
            var final = embed.Build();
            await ReplyAsync("", false, final);
        }
        [Command("ratelimits")]
        public async Task Rate()
        {
            string message = "```\nREST:\n        POST Message |  5/5s    | per-channel\n      DELETE Message |  5/1s    | per-channel\n PUT/DELETE Reaction |  1/0.25s | per-channel\n        PATCH Member |  10/10s  | per-guild\n   PATCH Member Nick |  1/1s    | per-guild\n      PATCH Username |  2/3600s | per-account\n      |All Requests| |  50/1s   | per-account\nWS:\n     Gateway Connect |   1/5s   | per-account\n     Presence Update |   5/60s  | per-session\n |All Sent Messages| | 120/60s  | per-session```";
            await Context.Channel.SendMessageAsync(message);

        }
    }
}