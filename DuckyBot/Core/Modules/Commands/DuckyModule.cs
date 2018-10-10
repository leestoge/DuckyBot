using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static DuckyBot.Core.Utilities.RandomGen;

namespace DuckyBot.Core.Modules.Commands
{
    public class Ducky : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    //SocketCommandContext allows access to the message, channel, server and user the command was invoked by, so works as the base as to how the bot knows where to reply/what user used which command, etc.
    {
        readonly List<string> _duckyQuotes = File.ReadLines("Resources/DuckyQuotes.txt").ToList();

        [Command("ducky")] // Command declaration
        [Alias("d")] // command aliases (also trigger task)
        [Summary("Triggers a random quote from the mind of Ducky. <:Duck1:267698075202486272>")] // Command summary
        public async Task DuckyQuote() // command async task (method basically)
        {
            do // **************** removed parts here, see archived backup if it posts empty string. ****************
            {
                var randomDuckyIndex = StaticRandom.Instance.Next(0, _duckyQuotes.Count); // get random number between 0 and list length
                var quoteToPost = _duckyQuotes[randomDuckyIndex]; // store string at the random number position in the list

                if (string.IsNullOrWhiteSpace(quoteToPost)) // filter out the empty line
                {
                    return;
                }
                await Context.Channel.SendMessageAsync(quoteToPost); // reply with the stored string
            } while (false); // loop while post is false
        }
        [Command("addquote")] // Command declaration
        [Alias("addducky", "ad")] // command aliases (also trigger task)
        [Summary("Add quotes to the !ducky command :heavy_plus_sign::speaking_head: (Only Moderators can use this command)")] // Command summary
        [RequireUserPermission(GuildPermission.Administrator)] // Needed User Permissions //
        public async Task AddQuote([Remainder] string quote) // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        {
            const string path = @"F:\Visual Studio\Projects\DuckyBot\DuckyBot\Resources\DuckyQuotes.txt"; // would have to be manually set up as it currently is

            // should probably add something here to check for the above path, and create it if its not found.

            //DuckyQuotes.Add(quote); // Add the parameter (quote) to the !ducky command list - self implemented strings currently lost upon bot process ending, must be added manually during downtime.
            using (var textEditor = File.AppendText("Resources/DuckyQuotes.txt"))
            {
                await textEditor.WriteLineAsync(quote).ConfigureAwait(false);
            }
            using (var textEditor = File.AppendText(path))
            {
                textEditor.WriteLine(quote);
            }
            Console.WriteLine($"{DateTime.Now:t}: Successfully added {quote} to !ducky"); // Notify me in console the time that this happened
            await Context.Channel.SendMessageAsync("Successfully added '**" + quote + "**'  to the `!ducky` command.");  // Notify user their parameter has been successfully added.
        }
    }
}