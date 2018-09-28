using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Collections.Generic;
using static DuckyBot.Core.Utilities.RandomGen;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;

namespace DuckyBot.Core.Modules.Commands
{
    public class Text : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    //SocketCommandContext allows access to the message, channel, server and user the command was invoked by, so works as the base as to how the bot knows where to reply/what user used which command, etc.
    {
        private readonly List<string>_tdpQuotes = File.ReadLines("Resources/TDPQuotes.txt").ToList();

        [Command("tdp"), Summary("Triggers a random quote a member of TDP has said! <:tdp:266702060454412288>")] // Command declaration and summary
        public async Task TdpQuote() // command async task (method basically)
        {
            do
            {
                var random = StaticRandom.Instance.Next(0, _tdpQuotes.Count); // get random number between 0 and list length
                var quoteToPost = _tdpQuotes[random]; // store string at the random number position in the list

                if (string.IsNullOrWhiteSpace(quoteToPost))
                {
                    return;
                }
                await Context.Channel.SendMessageAsync(quoteToPost); // reply with the stored string
            } while (false);
        }
        [Command("addtdp"), Summary("Add quotes to the !tdp command <:tdp:266702060454412288> (Only Moderators can use this command)")] // Command declaration and summary
        [RequireUserPermission(GuildPermission.Administrator)] // Needed User Permissions //
        public async Task AddQuote([Remainder] string quote) // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        {
            const string path = @"F:\Visual Studio\Projects\DuckyBot\DuckyBot\Resources\TDPQuotes.txt"; // would have to be manually set up as it currently is

            // should probably add something here to check for the above path, and create it if its not found.

            //TDPQuotes.Add(quote); // Add the parameter (quote) to the !ducky command list - implemented quotes currently lost upon bot process ending, must be added manually during downtime.
            using (var textEditor = File.AppendText("Resources/TDPQuotes.txt"))
            {
                textEditor.WriteLine(quote);
            }
            using (var textEditor = File.AppendText(path))
            {
                textEditor.WriteLine(quote);
            }
            await Context.Channel.SendMessageAsync("Successfully added '**" + quote + "**'  to the `!tdp` command.");  // Notify user their parameter has been successfully added.
        }
        [Command("8ball")] // Command declaration
        [Summary("Ask the great and wise 8 Ball to tell your fortune :four_leaf_clover:")] // command summary
        public async Task EightBall([Remainder] string input) // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        {
            var predictionsTexts = new[] // array of strings called "predictionsTexts"
            {
                "It is very unlikely",
                "It is very likely",
                "I don't think so",
                "Signs point to yes",
                "Ask again later",
                "My sources say no",
                "Concentrate and ask again",
                "It is certain",
                "Most likely", // self implemented strings
                "Not Likely",
                "Yes, in due time",
                "Definitely not",
                "I have my doubts",
                "Yes",
                "Without a doubt",
                "You may rely on it",
                "No",
                "Don't count on it",
                "Very doubtful",
                "Outlook is good",
                "Outlook not so good",
            };
            var rand = StaticRandom.Instance.Next(predictionsTexts.Length); // get random number between 0 and array length
            var text = predictionsTexts[rand]; // store string at the random number position in the array
            await ReplyAsync(Context.User.Mention + ", " + text); // reply with the string we got
        }
        [Command("owo")]
        [Summary("OwO what's this? <:hempus:446374082481750017>")] // command summary
        public async Task OwO([Remainder] string message)
        {
            var temp = message.ToLower();

            var replaceR = Regex.Replace(temp, @"r", "w");
            var replaceL = Regex.Replace(replaceR, @"l", "w");
            var replaceIs = Regex.Replace(replaceL, @"is", "ish");
            var replaceNe = Regex.Replace(replaceIs, @"ne", "nye");
            var replaceNu = Regex.Replace(replaceNe, @"nu", "nyu");
            var replaceNi = Regex.Replace(replaceNu, @"ni", "nyi");
            var replaceNa = Regex.Replace(replaceNi, @"na", "nya");
            var replaceNo = Regex.Replace(replaceNa, @"no", "nyo");

            var final = replaceNo.ToLower();
            await Context.Channel.SendMessageAsync(final + " " + "<:hempus:446374082481750017>");
        }
        [Command("vaporwave")]
        [Alias("v")] // command aliases (also trigger task)
        [Summary("vaporwave text")] // command summary
        public async Task VaporwaveText([Remainder] string message)
        {
            var temp = message.ToLower();
            //AESTHETIC TEXT
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            var replaceA = Regex.Replace(temp, @"a", "Ａ");
            var replaceB = Regex.Replace(replaceA, @"b", "Ｂ");
            var replaceC = Regex.Replace(replaceB, @"c", "Ｃ");
            var replaceD = Regex.Replace(replaceC, @"d", "Ｄ");
            var replaceE = Regex.Replace(replaceD, @"e", "Ｅ");
            var replaceF = Regex.Replace(replaceE, @"f", "Ｆ");
            var replaceG = Regex.Replace(replaceF, @"g", "Ｇ");
            var replaceH = Regex.Replace(replaceG, @"h", "Ｈ");
            var replaceI = Regex.Replace(replaceH, @"i", "Ｉ");
            var replaceJ = Regex.Replace(replaceI, @"j", "Ｊ");
            var replaceK = Regex.Replace(replaceJ, @"k", "Ｋ");
            var replaceL = Regex.Replace(replaceK, @"l", "Ｌ");
            var replaceM = Regex.Replace(replaceL, @"m", "Ｍ");
            var replaceN = Regex.Replace(replaceM, @"n", "Ｎ");
            var replaceO = Regex.Replace(replaceN, @"o", "Ｏ");
            var replaceP = Regex.Replace(replaceO, @"p", "Ｐ"); 
            var replaceQ = Regex.Replace(replaceP, @"q", "Ｑ");
            var replaceR = Regex.Replace(replaceQ, @"r", "Ｒ");
            var replaceS = Regex.Replace(replaceR, @"s", "Ｓ");
            var replaceT = Regex.Replace(replaceS, @"t", "Ｔ");
            var replaceU = Regex.Replace(replaceT, @"u", "Ｕ");
            var replaceV = Regex.Replace(replaceU, @"v", "Ｖ");
            var replaceW = Regex.Replace(replaceV, @"w", "Ｗ");
            var replaceX = Regex.Replace(replaceW, @"x", "Ｘ");
            var replaceY = Regex.Replace(replaceX, @"y", "Ｙ");
            var replaceZ = Regex.Replace(replaceY, @"z", "Ｚ");
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            string final = replaceZ.ToUpper();
            await Context.Channel.SendMessageAsync(final);
        }
        [Command("Trump")] // Command declaration
        [Alias("trump", "shadilay")] // command aliases (also trigger task)
        [Summary("Posts random Donald Trump quotes <:donaldpepe:455848800607797298>")] // command summary
        public async Task Maga() // command async task (method basically)
        {
            Console.WriteLine("!trump: making API call..."); // Console output for me
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })) //This acts like a web browser
            {
                var websiteurl = "https://api.whatdoestrumpthink.com/api/v1/quotes/random"; // The API site
                client.BaseAddress = new Uri(websiteurl); // Redirects our acting web browser to the API site
                var response = client.GetAsync("").Result; // Verify connection to site
                response.EnsureSuccessStatusCode(); // Verify connection to site
                var result = await response.Content.ReadAsStringAsync(); // Gets full website information
                var json = JObject.Parse(result); // Reads the json from the html

                var trumpQuote = json["message"].ToString(); // Saves the detected url to DogImage string (with "url" identifier prefix)

                await Context.Channel.SendMessageAsync(trumpQuote);
            }
        }
        [Command("joke")] // Command declaration
        [Alias("jokes", "j")] // command aliases (also trigger task)
        [Summary("Posts a random joke by itself or to a mentioned user :rofl:")] // command summary
        public async Task Joke([Remainder] string arg = "") // command async task (method basically)
        {
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault(); // store the mentioned user (if there is one) as "mentionedUser"

            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })) //This acts like a web browser
            {
                var websiteurl = "https://08ad1pao69.execute-api.us-east-1.amazonaws.com/dev/random_joke"; // The API site
                client.BaseAddress = new Uri(websiteurl); // Redirects our acting web browser to the API site
                var response = client.GetAsync("").Result; // Verify connection to site
                response.EnsureSuccessStatusCode(); // Verify connection to site
                var result = await response.Content.ReadAsStringAsync(); // Gets full website information
                var json = JObject.Parse(result); // Reads the json from the html

                var setup = json["setup"].ToString(); // Saves the detected url to DogImage string (with "url" identifier prefix)
                var punchline = json["punchline"].ToString(); // Saves the detected url to DogImage string (with "url" identifier prefix)

                if (mentionedUser == null) // if there is no mentioned user
                {
                    await Context.Channel.SendMessageAsync(setup);
                    await Task.Delay(3000);
                    await Context.Channel.SendMessageAsync(punchline);
                }
                else // if there is a mentioned user
                {
                    var target = mentionedUser; // target is the mentioned user
                    await Context.Channel.SendMessageAsync(target.Mention + " " + setup);
                    await Task.Delay(3000);
                    await Context.Channel.SendMessageAsync(target.Mention + " " + punchline);
                }
            }
        }
        [Command("copypasta")]
        [Alias("cp")]
        [Summary("copy and paste")] // command summary
        public async Task CopyPasta()
        {
            bool post; // Post default to true
            do
            {
                var weblink = "https://www.reddit.com/r/copypasta/random/.json"; // api link
                var httpClient = new HttpClient(); //This acts like a web browser
                var json = await httpClient.GetStringAsync(weblink); //default json to ""

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json); // deserialize json
                string copypasta = dataObject[0].data.children[0].data.selftext; // pull copypasta from api string

                if (copypasta.Length >= 2000 || string.IsNullOrEmpty(copypasta))
                {
                    post = false;
                }
                else
                {
                    post = true;

                    if (copypasta.Contains("&gt;"))
                    {
                        string final = Regex.Replace(copypasta, @"&gt;", "");
                        await Context.Channel.SendMessageAsync(final);
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync(copypasta); // post embedded message
                    }
                }
            } while (post == false);
        }
        [Command("emojipasta")]
        [Alias("ep")]
        [Summary("copy and paste (now with emojis!)")] // command summary
        public async Task EmojiCopyPasta()
        {
            bool post; // Post default to true
            do
            {               
                var weblink = "https://www.reddit.com/r/emojipasta/random/.json"; // api link
                var httpClient = new HttpClient(); //This acts like a web browser
                var json = await httpClient.GetStringAsync(weblink);

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json); // deserialize json
                string emojipasta = dataObject[0].data.children[0].data.selftext; // pull emojipasta from api string

                if (emojipasta.Length >= 2000 || string.IsNullOrEmpty(emojipasta))
                {
                    post = false;
                }
                else
                {
                    post = true;
                    await Context.Channel.SendMessageAsync(emojipasta); // post embedded message
                }
            } while (post == false);
        }
    }
}
