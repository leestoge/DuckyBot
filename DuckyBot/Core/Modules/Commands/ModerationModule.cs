using System;
using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using System.Linq;
using Discord.WebSocket;

namespace DuckyBot.Core.Modules.Commands
{
    public class Moderation : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    //SocketCommandContext allows access to the message, channel, server and user the command was invoked by, so works as the base as to how the bot knows where to reply/what user used which command, etc.
    {
        [Command("Delete")] // Command declaration
        [Alias("Cleanup")] // command aliases (also trigger task)
        [Summary("Deletes DuckyBot's most recent messages to prevent chat flood. Can only delete between 1-100 Messages at a time. :shower: (Only Moderators can use this command)")] // command summary
        [RequireUserPermission(GuildPermission.Administrator)] ///Needed User Permissions ///
        [RequireBotPermission(GuildPermission.ManageMessages)] //Needed Bot Permissions - must be set by the server administrator
        public async Task CleanupMessages([Remainder] int y) // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        {
            if (y < 1 || y > 5) // if users input parameter is less than 1 or greater than 5 (doesnt obey restrictions/cant remove a negative amount of messages)
            {
                await Context.Channel.SendMessageAsync($"You can only delete messages in a 1 - 5 range"); // notify user of message deletion rules
                return;
            }
            var messages = (await Context.Channel.GetMessagesAsync(y + 1).Flatten()).Where(x => x.Author.Id == Context.Guild.CurrentUser.Id);
            // get the amount of messages specified in parameter from the user ID of the bot - ensures only bot messages are deleted.
            await Context.Channel.DeleteMessagesAsync(messages); // delete the messages
            await Context.Channel.SendMessageAsync($"Successfully deleted `{y}` DuckyBot messages."); // notify user of message deletion success
        }
        [Command("Ban")] // Command declaration
        [Summary("Ban @Username :octagonal_sign: (Only Moderators can use this command)")] // command summary
        [RequireUserPermission(GuildPermission.Administrator)] ///Needed User Permissions ///
        [RequireBotPermission(GuildPermission.BanMembers)] //Needed Bot Permissions - must be set by the server administrator
        public async Task BanAsync(SocketGuildUser user = null, [Remainder] string reason = null) // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        // parameters take in a mentioned user (which defaults to null), then a reason for the banning process
        {
            if (user == null) throw new ArgumentException("You must mention a user!"); // if no user mentioned, notify command user that they must mention a user to ban
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("You must provide a reason!"); // if no reason stated, notify command user that they must provide a ban reason

            var gld = Context.Guild as SocketGuild; // store server in context (aka guild) as var gld
            var embed = new EmbedBuilder(); // create new embeded message //
            embed.WithColor(new Color(255, 82, 41)); // set embedded message colour to orange
            embed.ThumbnailUrl = user.GetAvatarUrl(); // set embedded message thumbnail to the mentioned users avatar
            embed.Title = $"**{user.Username}** was banned from {user.Guild.Name}!"; // set embedded message title to who was banned
            embed.Description = $"**Username: **{user.Username}\n**Banned by: **{Context.User.Mention}!\n**Reason: **{reason}"; // set embedded description to who the mentioned user was banned by

            await gld.AddBanAsync(user); // adds mentioned user to the var gld banned list (Server in context) - different ban lists per server
            var final = embed.Build();
            await Context.Channel.SendMessageAsync("", false, final); // send the embedded message back to the command using user
        }
        [Command("Remove")] // Command declaration
        [Summary("Remove @Username from the server :door: :point_left: (Only Moderators can use this command)")] // command summary
        [RequireBotPermission(GuildPermission.KickMembers)] //Needed Bot Permissions - must be set by the server administrator
        [RequireUserPermission(GuildPermission.Administrator)] ///Needed User Permissions ///
        public async Task KickAsync(SocketGuildUser user, [Remainder] string reason) // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        // parameters take in a mentioned user (which defaults to null), then a reason for the removing process
        {
            if (user == null) throw new ArgumentException("You must mention a user!"); // if no user mentioned, notify command user that they must mention a user to ban
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("You must provide a reason!"); // if no reason stated, notify command user that they must provide a remove reason

            var gld = Context.Guild as SocketGuild; // store server in context (aka guild) as var gld
            var embed = new EmbedBuilder(); // create new embeded message //
            embed.WithColor(new Color(255, 82, 41)); // set embedded message colour to orange
            embed.ThumbnailUrl = user.GetAvatarUrl(); // set embedded message thumbnail to the mentioned users avatar
            embed.Title = $" {user.Username} has been removed from {user.Guild.Name}!"; // set embedded message title to who was removed
            embed.Description = $"**Username: **`{user.Username}`\n**Removed by: **{Context.User.Mention}!\n**Reason: **`{reason}`"; // set embedded description to who the mentioned user was removed by

            await user.KickAsync(); // removes mentioned user
            var final = embed.Build();
            await Context.Channel.SendMessageAsync("", false, final); // send the embedded message back to the command using user
        }
    }
}