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
        List<string>TDPQuotes = System.IO.File.ReadLines("Resources/TDPQuotes.txt").ToList();

        [Command("tdp"), Summary("Triggers a random quote a member of TDP has said! <:tdp:266702060454412288>")] // Command declaration and summary
        public async Task TDPQuote() // command async task (method basically)
        {
            bool post = true; // Post default to true
            do
            {
                int random = StaticRandom.Instance.Next(0, TDPQuotes.Count); // get random number between 0 and list length
                string quoteToPost = TDPQuotes[random]; // store string at the random number position in the list

                if (String.IsNullOrWhiteSpace(quoteToPost))
                {
                    post = false;
                    return;
                }
                await Context.Channel.SendMessageAsync(quoteToPost); // reply with the stored string
            } while (post == false);
        }
        [Command("addtdp"), Summary("Add quotes to the !tdp command <:tdp:266702060454412288> (Only Moderators can use this command)")] // Command declaration and summary
        [RequireUserPermission(GuildPermission.Administrator)] ///Needed User Permissions ///
        public async Task addQuote([Remainder] string quote) // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        {
            //TDPQuotes.Add(quote); // Add the parameter (quote) to the !ducky command list - implemented quotes currently lost upon bot process ending, must be added manually during downtime.
            using (StreamWriter TextEditor = File.AppendText("Resources/TDPQuotes.txt"))
            {
                TextEditor.WriteLine(quote);
            }
            await Context.Channel.SendMessageAsync("Sucessfully added '**" + quote + "**'  to the `!tdp` command.");  // Notify user their parameter has been sucessfully added.
        }
        [Command("8ball")] // Command declaration
        [Summary("Ask the great and wise 8 Ball to tell your fortune :four_leaf_clover:")] // command summary
        public async Task EightBall([Remainder] string input) // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        {
            string[] predictionsTexts = new string[] // array of strings called "predictionsTexts"
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
            int rand = StaticRandom.Instance.Next(predictionsTexts.Length); // get random number between 0 and array length
            string text = predictionsTexts[rand]; // store string at the random number position in the array
            await ReplyAsync(Context.User.Mention + ", " + text); // reply with the string we got
        }
        [Command("owo")]
        [Summary("OwO what's this? <:hempus:446374082481750017>")] // command summary
        public async Task OwO([Remainder] string message)
        {
            string temp;
            temp = message.ToLower();

            string replaceR = Regex.Replace(temp, @"r", "w");
            string replaceV = Regex.Replace(replaceR, @"v", "v"); // dont replace v
            string replaceL = Regex.Replace(replaceV, @"l", "w");
            string replaceIS = Regex.Replace(replaceL, @"is", "ish");
            string replaceNE = Regex.Replace(replaceIS, @"ne", "nye");
            string replaceNU = Regex.Replace(replaceNE, @"nu", "nyu");
            string replaceNI = Regex.Replace(replaceNU, @"ni", "nyi");
            string replaceNA = Regex.Replace(replaceNI, @"na", "nya");
            string replaceNO = Regex.Replace(replaceNA, @"no", "nyo");

            string final = replaceNO.ToLower();
            await Context.Channel.SendMessageAsync(final + " " + "<:hempus:446374082481750017>");
        }
        [Command("vaporwave")]
        [Alias("v")] // command aliases (also trigger task)
        [Summary("vaporwave text")] // command summary
        public async Task VaporwaveText([Remainder] string message)
        {
            string temp;
            temp = message.ToLower();
            //AESTHETIC TEXT
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            string replaceA = Regex.Replace(temp, @"a", "Ａ");
            string replaceB = Regex.Replace(replaceA, @"b", "Ｂ");
            string replaceC = Regex.Replace(replaceB, @"c", "Ｃ");
            string replaceD = Regex.Replace(replaceC, @"d", "Ｄ");
            string replaceE = Regex.Replace(replaceD, @"e", "Ｅ");
            string replaceF = Regex.Replace(replaceE, @"f", "Ｆ");
            string replaceG = Regex.Replace(replaceF, @"g", "Ｇ");
            string replaceH = Regex.Replace(replaceG, @"h", "Ｈ");
            string replaceI = Regex.Replace(replaceH, @"i", "Ｉ");
            string replaceJ = Regex.Replace(replaceI, @"j", "Ｊ");
            string replaceK = Regex.Replace(replaceJ, @"k", "Ｋ");
            string replaceL = Regex.Replace(replaceK, @"l", "Ｌ");
            string replaceM = Regex.Replace(replaceL, @"m", "Ｍ");
            string replaceN = Regex.Replace(replaceM, @"n", "Ｎ");
            string replaceO = Regex.Replace(replaceN, @"o", "Ｏ");
            string replaceP = Regex.Replace(replaceO, @"p", "Ｐ"); 
            string replaceQ = Regex.Replace(replaceP, @"q", "Ｑ");
            string replaceR = Regex.Replace(replaceQ, @"r", "Ｒ");
            string replaceS = Regex.Replace(replaceR, @"s", "Ｓ");
            string replaceT = Regex.Replace(replaceS, @"t", "Ｔ");
            string replaceU = Regex.Replace(replaceT, @"u", "Ｕ");
            string replaceV = Regex.Replace(replaceU, @"v", "Ｖ");
            string replaceW = Regex.Replace(replaceV, @"w", "Ｗ");
            string replaceX = Regex.Replace(replaceW, @"x", "Ｘ");
            string replaceY = Regex.Replace(replaceX, @"y", "Ｙ");
            string replaceZ = Regex.Replace(replaceY, @"z", "Ｚ");
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            string final = replaceZ.ToUpper();
            await Context.Channel.SendMessageAsync(final);
        }
        [Command("Trump")] // Command declaration
        [Alias("trump", "shadilay")] // command aliases (also trigger task)
        [Summary("Posts random Donald Trump quotes <:donaldpepe:455848800607797298>")] // command summary
        public async Task MAGA() // command async task (method basically)
        {
            Console.WriteLine("!trump: making API call..."); // Console output for me
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })) //This acts like a web browser
            {
                string websiteurl = "https://api.whatdoestrumpthink.com/api/v1/quotes/random"; // The API site
                client.BaseAddress = new Uri(websiteurl); // Redirects our acting web browser to the API site
                HttpResponseMessage response = client.GetAsync("").Result; // Verify connection to site
                response.EnsureSuccessStatusCode(); // Verify connection to site
                string result = await response.Content.ReadAsStringAsync(); // Gets full website information
                var json = JObject.Parse(result); // Reads the json from the html

                string TrumpQuote = json["message"].ToString(); // Saves the detected url to DogImage string (with "url" identifier prefix)

                await Context.Channel.SendMessageAsync(TrumpQuote);
            }
        }
        [Command("joke")] // Command declaration
        [Alias("jokes", "j")] // command aliases (also trigger task)
        [Summary("Posts a random joke by itself or to a mentioned user :rofl:")] // command summary
        public async Task Joke([Remainder] string arg = "") // command async task (method basically)
        {
            SocketUser target = null; // default the target user whos XP we are returning to null
            var mentionedUser = Context.Message.MentionedUsers.FirstOrDefault(); // store the mentioned user (if there is one) as "mentionedUser"

            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })) //This acts like a web browser
            {
                string websiteurl = "https://08ad1pao69.execute-api.us-east-1.amazonaws.com/dev/random_joke"; // The API site
                client.BaseAddress = new Uri(websiteurl); // Redirects our acting web browser to the API site
                HttpResponseMessage response = client.GetAsync("").Result; // Verify connection to site
                response.EnsureSuccessStatusCode(); // Verify connection to site
                string result = await response.Content.ReadAsStringAsync(); // Gets full website information
                var json = JObject.Parse(result); // Reads the json from the html

                string Setup = json["setup"].ToString(); // Saves the detected url to DogImage string (with "url" identifier prefix)
                string Punchline = json["punchline"].ToString(); // Saves the detected url to DogImage string (with "url" identifier prefix)

                if (mentionedUser == null) // if there is no mentioned user
                {
                    await Context.Channel.SendMessageAsync(Setup);
                    await Task.Delay(3000);
                    await Context.Channel.SendMessageAsync(Punchline);
                }
                else // if there is a mentioned user
                {
                    target = mentionedUser; // target is set to the mentioned user
                    await Context.Channel.SendMessageAsync(target.Mention + " " + Setup);
                    await Task.Delay(3000);
                    await Context.Channel.SendMessageAsync(target.Mention + " " + Punchline);
                }
            }
        }
        [Command("copypasta")]
        [Alias("cp")]
        [Summary("copy and paste")] // command summary
        public async Task CopyPasta()
        {
            bool post = true; // Post default to true
            do
            {
                string weblink = "https://www.reddit.com/r/copypasta/random/.json"; // api link
                string json = ""; //default json to ""
                var httpClient = new HttpClient(); //This acts like a web browser
                json = await httpClient.GetStringAsync(weblink); // set json to api string

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
            bool post = true; // Post default to true
            do
            {               
                string weblink = "https://www.reddit.com/r/emojipasta/random/.json"; // api link
                string json = ""; //default json to ""
                var httpClient = new HttpClient(); //This acts like a web browser
                json = await httpClient.GetStringAsync(weblink); // set json to api string

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
