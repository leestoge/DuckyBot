using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
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
                var randomDuckyIndex = Instance.Next(0, _duckyQuotes.Count); // get random number between 0 and list length
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
        [Summary("Add quotes to the !ducky command :heavy_plus_sign::speaking_head: (Only TDP Members can use this command)")] // Command summary     
        public async Task AddQuote([Remainder] string quote) // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        {
            const string path = @"F:\Visual Studio\Projects\DuckyBot\DuckyBot\Resources\DuckyQuotes.txt"; // would have to be manually set up as it currently is
            // should probably add something here to check for the above path, and create it if its not found.

            var application = await Context.Client.GetApplicationInfoAsync(); // gets client details from bot
            var Me = application.Owner.Id; // find my user id from bot client details

            if (((SocketGuildUser)Context.User).Roles.Any(r => r.Name == "TDP Member"))
            {
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
            else if (((SocketGuildUser)Context.User).Id == Me)
            {
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
            else
            {
                await ReplyAsync(Context.User.Mention + " you don't appear to be a TDP Member yet so you are unable to add quotes to the `!ducky` command.");
            }        
        }
    }
}