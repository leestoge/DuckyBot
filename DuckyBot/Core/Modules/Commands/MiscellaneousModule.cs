using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using System.Linq;
using static DuckyBot.Core.Utilities.RandomGen;
using DuckyBot.Core.LevelingSystem.UserAccounts;

namespace DuckyBot.Core.Modules.Commands
{
    public class Miscellaneous : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    //SocketCommandContext allows access to the message, channel, server and user the command was invoked by, so works as the base as to how the bot knows where to reply/what user used which command, etc.
    {
        [Command("Stats")] // Command declaration
        [Summary("Allows users to see their current XP! :sparkles:")] // command summary
        public async Task Stats([Remainder] string arg = "") // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        // the arg parameter allows mentioning of another user in order to see their gained XP, not mentioning a user just provides your own XP.
        {
            SocketUser target; // default the target user whos XP we are returning to null
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault(); // store the mentioned user (if there is one) as "mentionedUser"

            target = mentionedUser ?? Context.User;
            var account = UserAccounts.GetAccount(target); // get the users account info by passing in the obtained user ID
            await Context.Channel.SendMessageAsync($"{target.Mention} has {account.XP} XP!"); // reply mentioning the user and stating their current XP
            Console.WriteLine($"{DateTime.Now.ToString("t")} !stats executed successfully in " + Context.Guild.Name);
        }
        [Command("SetGame")] // Command declaration
        [Summary("Sets a 'Game' for the bot :video_game: (Only Moderators can use this command)")] // command summary
        [RequireUserPermission(GuildPermission.Administrator)] ///Needed User Permissions ///
        public async Task setgame([Remainder] string game) // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        {
            await (Context.Client as DiscordSocketClient).SetGameAsync(game); // change bots playing status to the provided string parameter
            await Context.Channel.SendMessageAsync($"Successfully set my playing status to '**{game}**'"); // notify user in the text channel the command was used in
            Console.WriteLine($"{DateTime.Now}: Game was changed to {game}"); // notify me in console log
            Console.WriteLine($"{DateTime.Now.ToString("t")} !setgame executed successfully in " + Context.Guild.Name);
        }
        [Command("idiot")] // Command declaration
        [Summary("@idiot idiot :drooling_face:")] // command summary
        public async Task IdiotRole() // command async task (method basically)
        {
            var user = Context.User; // store the user that used the command as var user
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "Idiot"); // store the server role called "Idiot" as var role

            await (user as IGuildUser).AddRoleAsync(role); // modify var users role
            await Context.Channel.SendMessageAsync(Context.User.Mention + " idiot"); // notify user in the text channel the command was used in
            Console.WriteLine($"{DateTime.Now.ToString("t")} !idiot executed successfully in " + Context.Guild.Name);

            if (role.Name != "Idiot")
            {
                await Context.Channel.SendMessageAsync("oh fuck"); // notify user in the text channel the command was used in
                return;
            }
        }
        [Command("dm")] // Command declaration
        [Summary("Directly message the bot owner. :writing_hand:")] // command summary
        public async Task sendmsgtoowner([Remainder] string text) // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        {
            await Context.Channel.SendMessageAsync(Context.User.Mention + " your message has been sent!"); // notify user in the text channel the command was used in

            var embed = new EmbedBuilder() // create new embedded message
            {
                Color = new Color(255, 82, 41) // set colour of embedded message trim (to orange)
            };
            var application = await Context.Client.GetApplicationInfoAsync(); // gets channels from discord client
            var z = await application.Owner.GetOrCreateDMChannelAsync(); // find my dm channel in order to private message me
            //embed.Description = $"`{Context.User.Username}` **from** `{Context.Guild.Name}` **sent you a message!**\n\n{text}"; // set embedded message description as user sent a message (With their input message)
            //await z.SendMessageAsync("", false, embed); // private message me with the users message
            await z.SendMessageAsync($"`{Context.User.Username}` **from** `{Context.Guild.Name}` **sent you a message!**\n\n{text}");
            Console.WriteLine($"{DateTime.Now.ToString("t")} !dm executed successfully in " + Context.Guild.Name);
        }
        [Command("vid")] // Command declaration
        [Alias("Vid", "video", "vids")] // command aliases (also trigger task)
        [Summary("Posts a random Ducky related video :popcorn:")] // command summary 
        public async Task DuckyVideo() // command async task (method basically)
        {
            string[] vids = new string[] // array of strings
            {
                "https://youtu.be/w1hZajJykW4",
                "https://youtu.be/JxVunljhq7s",
                "https://youtu.be/gR2UfAqezQk",
                "https://youtu.be/SM3wUdV16KU", // self implemented strings
                "https://youtu.be/h5_YKvkrQuo",
                "https://youtu.be/j2aPWI9ChtM",
                "https://youtu.be/y_lActFE-sw",
                "https://youtu.be/iKR7anoXsqg",
                "https://clips.twitch.tv/ShyGleamingWormUnSane",
                "https://clips.twitch.tv/KitschyElegantLocustBIRB",
            }; // array of strings called "vids" that holds the video links
            int rand = StaticRandom.Instance.Next(vids.Length); // get random number between 0 and array length
            string vidtopost = vids[rand]; // store string at the random number position in the array
            await ReplyAsync(vidtopost); // send file at our string file path we randomly get
            Console.WriteLine($"{DateTime.Now.ToString("t")} !vid executed successfully in " + Context.Guild.Name);
        }
        [Command("flip")]
        [Summary("Flips a coin :moneybag:")]
        [Alias("coin", "coinflip", "headsortails")]
        public async Task FlipAsync()
        {
            string flipToSend;
            int flip = StaticRandom.Instance.Next(1, 3);
            if ((flip == 1) && !(flip == 2))
            {
                flipToSend = "Heads";
            }
            else if ((flip == 2) && !(flip == 1))
            {
                flipToSend = "Tails";
            }
            else
            {
                await Context.Channel.SendMessageAsync("Something has went wrong! Stoge has been notified."); // tell user an error occured

                var application = await Context.Client.GetApplicationInfoAsync(); // gets channels from discord client
                var z = await application.Owner.GetOrCreateDMChannelAsync(); // find my dm channel in order to private message me
                await z.SendMessageAsync($"[{DateTime.Now.ToString("t")}] `!flip` error: error establishing random number"); // private message me with error reason (with time error occured)

                Console.WriteLine("!flip error: error establishing random number");
                return;
            }
            await Context.Channel.SendMessageAsync(flipToSend);
            Console.WriteLine($"{DateTime.Now.ToString("t")} !flip executed successfully in " + Context.Guild.Name);
        }
    }
}
