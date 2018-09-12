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
        List<string> DuckyQuotes = System.IO.File.ReadLines("Resources/DuckyQuotes.txt").ToList();

        [Command("ducky")] // Command declaration
        [Alias("d")] // command aliases (also trigger task)
        [Summary("Triggers a random quote from the mind of Ducky. <:Duck1:267698075202486272>")] // Command summary
        public async Task DuckyQuote() // command async task (method basically)
        {
            bool post = true; // Post default to true
            do
            {
                int randomDuckyIndex = StaticRandom.Instance.Next(0, DuckyQuotes.Count); // get random number between 0 and list length
                string quoteToPost = DuckyQuotes[randomDuckyIndex]; // store string at the random number position in the list

                if (String.IsNullOrWhiteSpace(quoteToPost))
                {
                    post = false;
                    return;
                }
                await Context.Channel.SendMessageAsync(quoteToPost); // reply with the stored string
            } while (post == false);
        }
        [Command("addquote")] // Command declaration
        [Alias("addducky", "ad")] // command aliases (also trigger task)
        [Summary("Add quotes to the !ducky command :heavy_plus_sign::speaking_head: (Only Moderators can use this command)")] // Command summary
        [RequireUserPermission(GuildPermission.Administrator)] ///Needed User Permissions ///
        public async Task addQuote([Remainder] string quote) // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        {
            //DuckyQuotes.Add(quote); // Add the parameter (quote) to the !ducky command list - self implemented strings currently lost upon bot process ending, must be added manually during downtime.
            using (StreamWriter TextEditor = File.AppendText("Resources/DuckyQuotes.txt"))
            {
                await TextEditor.WriteLineAsync(quote);
            }
            Console.WriteLine($"{DateTime.Now.ToString("t")}: Sucessfully added {quote} to !ducky"); // Notify me in console the time that this happened
            await Context.Channel.SendMessageAsync("Sucessfully added '**" + quote + "**'  to the `!ducky` command.");  // Notify user their parameter has been sucessfully added.
        }
    }
}