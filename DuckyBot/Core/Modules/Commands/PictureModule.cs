using Discord;
using Discord.Commands;
using DuckyBot.Core.Utilities;
using ImageSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static DuckyBot.Core.Utilities.RandomGen;

namespace DuckyBot.Core.Modules.Commands
{
    public class Pictures : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    //SocketCommandContext allows access to the message, channel, server and user the command was invoked by, so works as the base as to how the bot knows where to reply/what user used which command, etc.
    {
        [Command("pic")] // Command declaration
        [Alias("img", "photo", "image", "picture")] // command aliases (also trigger task)
        [Summary("Posts a random picture :camera_with_flash:")] // command summary
        public async Task Picture() // command async task (method basically)
        {
            var dirInfo = Directory.EnumerateFiles("Pictures/").ToList();
            var randomIndex = StaticRandom.Instance.Next(0, dirInfo.Count); // get random number between 0 and list length
            var randomFilePathString = dirInfo[randomIndex];

            await Context.Channel.SendFileAsync(randomFilePathString); // send file at our string file path we randomly get

            if (randomFilePathString == "Pictures/hoora.jpg") // under certain conditions will post a message alongside a picture
            {
                await Context.Channel.SendMessageAsync("ALL HAIL COMMANDER DUCKY. THOSE WHO OPPOSE M'LORD SHALL PERISH! HOORA HOORA HOORA...");
            }
            if (randomFilePathString == "Pictures/dragon.jpg")
            {
                await Context.Channel.SendMessageAsync("Dracarys! 🔥🔥🔥🔥🔥🔥🔥🔥");
            }
            if (randomFilePathString == "Pictures/ult.png")
            {
                await Context.Channel.SendMessageAsync("ryū ga waga teki wo kurau!");
            }
            if (randomFilePathString == "Pictures/tomape.png")
            {
                await Context.Channel.SendMessageAsync("ook ook");
            }
        }
        [Command("corona")] // Command declaration
        [Alias("Corona")] // command aliases (also trigger task)
        [Summary("Find out what happens to Ducky after 6 bottles of Corona :beers:")] // command summary
        public async Task SixBottlesLater() // command async task (method basically)
        {
            await Context.Channel.SendMessageAsync("6 Bottles of Corona Later.."); // message
            await Context.Channel.SendFileAsync("Pictures/6bottles.gif"); // post gif
            await Context.Channel.SendMessageAsync("...Fuck me"); // message
            Console.WriteLine("!corona executed successfully in " + Context.Guild.Name);
        }
        [Command("coffee")] // Command declaration
        [Alias("coffeepepe", "pepe")] // command aliases (also trigger task)
        [Summary("D A I L Y G R I N D")] // command summary
        public async Task CoffeePepe() // command async task (method basically)
        {        
            var dirInfo = Directory.EnumerateFiles("Pictures/coffee/").ToList();
            int randomIndex = StaticRandom.Instance.Next(0, dirInfo.Count); // get random number between 0 and list length
            var randomFilePathString = dirInfo[randomIndex];

            await Context.Channel.SendFileAsync(randomFilePathString); // send file at our string file path we randomly get
        }
        [Command("rotate")] // Command declaration
        [Summary("Rotate your discord avatar at an input angle :arrows_counterclockwise:")] // command summary
        public async Task Rotate([Remainder] uint x) // command async task that takes in a parameter (remainder represents a space between the command and the parameter)
        // parameter is a uint, meaning that it can only be a positive number
        {
            if (x > 360 || x < 1) // if input parameter is greater than 360 (degrees) or less than 1
            {
                await Context.Channel.SendMessageAsync("Something has went wrong! Stoge has been notified."); // tell user an error occurred

                var application = await Context.Client.GetApplicationInfoAsync(); // gets channels from discord client
                var z = await application.Owner.GetOrCreateDMChannelAsync(); // find my dm channel in order to private message me
                await z.SendMessageAsync($"{DateTime.Now.ToString("t")} **{Context.User.Username}** tried to rotate their avatar by `" + x + "` degrees."); // private message me with error reason (with time error occurred)
            }
            else // if input parameter is < 360 degrees
            {
                var httpClient = new HttpClient(); // This acts like a web browser
                var response = await httpClient.GetAsync(Context.User.GetAvatarUrl());  // get response from users avatar URL
                var inputStream = await response.Content.ReadAsStreamAsync(); // input users avatar URL into ImageSharp to be parsed
                var image = ImageSharp.Image.Load(inputStream);
                image.Rotate(x); // rotate the image by the input parameter amount

                Stream outStream = new MemoryStream(); // store the rotated image as "outStream"
                image.SaveAsPng(outStream); // save the rotated image
                outStream.Position = 0;
                const string input = "abcdefghijklmnopqrstuvwxyz0123456789"; // random letters/numbers for naming convention for temporary saves
                var randomString = ""; // string "randomString" to save generated naming convention
                var rand = StaticRandom.Instance.Next(0, input.Length); // get random number between 0 and input string length
                for (var i = 0; i < 8; i++) // loop 8 times to generate an 8 character long random naming convention for temporary saves
                {
                    var ch = input[rand]; // new char variable
                    randomString += ch; // string "randomString" to be assigned a new char result 8 times, resulting in a randomly generated name consisting of 8 characters
                }
                var file = File.Create($"Pictures/{randomString}.png"); // create the file with the "randomString" naming convention
                await outStream.CopyToAsync(file); // copy the stored rotated image into the file
                file.Dispose(); // release stream
                await Context.Channel.SendFileAsync($"Pictures/{randomString}.png"); // post the image rotated by the set user amount
                File.Delete($"Pictures/{randomString}.png"); // delete the image
                Console.WriteLine($"{DateTime.Now.ToString("t")} !rotate executed successfully in " + Context.Guild.Name);
            }
        }
        [Command("cat")] // Command declaration
        [Alias("cate", "kitty", "kitten")] // command aliases (also trigger task)
        [Summary("Posts random cat pictures :cat:")] // command summary
        public async Task Cat()
        {
            bool post; // Post default to true
            do
            {
                const string weblink = "https://www.reddit.com/r/catpictures/random/.json"; // api link
                var httpClient = new HttpClient(); //This acts like a web browser
                var json = await httpClient.GetStringAsync(weblink);

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json); // de-serialise json
                string image = dataObject[0].data.children[0].data.url.ToString(); // pull image from api string

                if (image.EndsWith(".jpg") || image.EndsWith(".png") || image.EndsWith(".gif")) // if its an image
                {
                    post = true; //post it
                    var embed = new EmbedBuilder(); // Create new embedded message
                    embed.WithImageUrl(ApiHelper.GetRedirectUrl(image)); // embed the image within the message
                    embed.WithColor(new Color(255, 82, 41)); // set embedded message trim colour to orange
                    embed.WithFooter(footer => // embedded message footer builder
                    {
                        footer
                        .WithText($"Requested by {Context.User.Username} at {DateTime.Now.ToString("t")} | From: r/catpictures") // footer data, "Requested by [name] at [time] | from [place]
                        .WithIconUrl(Context.User.GetAvatarUrl(ImageFormat.Auto, 64)); // get users avatar for use in footer
                    });
                    var final = embed.Build(); // final = constructed embedded message
                    await Context.Channel.SendMessageAsync("", false, final); // post embedded message
                }
                else if (image.EndsWith(".gifv")) // .gifv causes Discord not to display the image... for some reason
                {
                    post = false; // do not post it
                }
                else // if its not an image (youtube video, etc)
                {
                    post = false; // do not post it
                }
            } while (post == false); // Loop until its an image
        }
        [Command("dog")] // Command declaration
        [Alias("doggo", "pupper", "yapper")] // command aliases (also trigger task)
        [Summary("Posts random dog pictures :dog:")] // command summary
        public async Task Dog() // command async task (method basically)
        {
            bool post; // Post default to true
            do
            {
                using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })) //This acts like a web browser
                { //https://dog.ceo/dog-api/
                    string websiteurl = "https://random.dog/woof.json"; // The API site
                    client.BaseAddress = new Uri(websiteurl); // Redirects our acting web browser to the API site
                    HttpResponseMessage response = client.GetAsync("").Result; // Verify connection to site
                    response.EnsureSuccessStatusCode(); // Verify connection to site
                    string result = await response.Content.ReadAsStringAsync(); // Gets full website information
                    var json = JObject.Parse(result); // Reads the json from the html

                    string DogImage = json["url"].ToString(); // Saves the detected url to DogImage string (with "url" identifier prefix)

                    if (DogImage.Contains("mp4") || DogImage.EndsWith(".gifv")) // check if our DogImage string contains an .mp4 or .gifv extension
                    {
                        post = false; // Do not post if this is the case (They don't display properly in Discord within the embed)
                    }
                    else // All other extensions
                    {
                        post = true; // Post them
                        var embed = new EmbedBuilder() // Create new embedded message
                        {
                            Color = new Color(255, 82, 41), // set embedded message trim colour to orange
                        };
                        embed.WithImageUrl(ApiHelper.GetRedirectUrl(DogImage)); // place picture inside embedded message (ensures the picture is posted with no link shown)
                        embed.WithFooter(footer =>
                        {
                            footer
                            .WithText($"Requested by {Context.User.Username} at {DateTime.Now.ToString("t")} | From: https://random.dog")
                            .WithIconUrl(Context.User.GetAvatarUrl(ImageFormat.Auto, 64));
                        });
                        var final = embed.Build();
                        await ReplyAsync("", false,final); // Sends the the embedded message (picture)
                    }
                }
            } while (post == false); // Loop until DogImage doesnt have an .mp4 extension
        }
        [Command("aesthetic")]
        [Alias("a", "outrun")]
        [Summary("Posts random ＡＥＳＴＨＥＴＩＣ　ワネヘ pictures")] // command summary
        public async Task Aesthetic()
        {
            bool post = true; // Post default to true
            do
            {
                string weblink = "https://www.reddit.com/r/VaporwaveAesthetics/random/.json"; // api link
                string json = ""; //default json to ""
                var httpClient = new HttpClient(); //This acts like a web browser
                json = await httpClient.GetStringAsync(weblink); // set json to api string

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json); // deserialize json
                string image = dataObject[0].data.children[0].data.url.ToString(); // pull image from api string

                if (image.EndsWith(".jpg") || image.EndsWith(".png") || image.EndsWith(".gif")) // if its an image
                {
                    post = true; //post it
                    var embed = new EmbedBuilder(); // Create new embedded message
                    embed.WithImageUrl(ApiHelper.GetRedirectUrl(image)); // embed the image within the message
                    embed.WithColor(new Color(255, 82, 41)); // set embedded message trim colour to orange
                    embed.WithFooter(footer => // embedded message footer builder
                    {
                        footer
                        .WithText($"Requested by {Context.User.Username} at {DateTime.Now.ToString("t")} | From: r/VaporwaveAesthetics") // footer data, "Requested by [name] at [time] | from [place]
                        .WithIconUrl(Context.User.GetAvatarUrl(ImageFormat.Auto, 64)); // get users avatar for use in footer
                    });
                    var final = embed.Build(); // final = constructed embedded message
                    await Context.Channel.SendMessageAsync("", false, final); // post embedded message
                }
                else if (image.EndsWith(".gifv")) // .gifv causes Discord not to display the image... for some reason
                {
                    post = false; // do not post it
                }
                else // if its not an image (youtube video, etc)
                {
                    post = false; // do not post it
                }
            } while (post == false); // Loop until its an image
        }
        [Command("gamer")] // We live in a society...
        [Alias("g", "gamersriseup", "veronica")]
        [Summary("We live in a society")] // command summary
        public async Task GamersRiseUp()
        {
            bool post = true; // Post default to true
            do
            {
                string weblink = "https://www.reddit.com/r/GamersRiseUp/random/.json"; // api link
                string json = ""; //default json to ""
                var httpClient = new HttpClient(); //This acts like a web browser
                json = await httpClient.GetStringAsync(weblink); // set json to api string

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json); // deserialize json
                string image = dataObject[0].data.children[0].data.url.ToString(); // pull image from api string

                if (image.EndsWith(".jpg") || image.EndsWith(".png") || image.EndsWith(".gif")) // if its an image
                {
                    post = true; //post it
                    var embed = new EmbedBuilder(); // Create new embedded message
                    embed.WithImageUrl(ApiHelper.GetRedirectUrl(image)); // embed the image within the message
                    embed.WithColor(new Color(255, 82, 41)); // set embedded message trim colour to orange
                    embed.WithFooter(footer => // embedded message footer builder
                    {
                        footer
                        .WithText($"Requested by {Context.User.Username} at {DateTime.Now.ToString("t")} | From: r/GamersRiseUp") // footer data, "Requested by [name] at [time] | from [place]
                        .WithIconUrl(Context.User.GetAvatarUrl(ImageFormat.Auto, 64)); // get users avatar for use in footer
                    });
                    var final = embed.Build(); // final = constructed embedded message
                    await Context.Channel.SendMessageAsync("", false, final); // post embedded message
                }
                else if (image.EndsWith(".gifv")) // .gifv causes Discord not to display the image... for some reason
                {
                    post = false; // do not post it
                }
                else // if its not an image (youtube video, etc)
                {
                    post = false; // do not post it
                }
            } while (post == false); // Loop until its an image
        }
        [Command("anime")]
        [Alias("hentai", "hempus")]
        [Summary("<:hempus:446374082481750017>")] // command summary
        public async Task Anime()
        {
            bool post = true; // Post default to true
            do
            {
                string weblink = "https://www.reddit.com/r/animegifs/random/.json"; // api link
                string json = ""; //default json to ""
                var httpClient = new HttpClient(); //This acts like a web browser
                json = await httpClient.GetStringAsync(weblink); // set json to api string

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json); // deserialize json
                string image = dataObject[0].data.children[0].data.url.ToString(); // pull image from api string

                if (image.EndsWith(".gif")) // if its a gif
                {
                    post = true; //post it
                    var embed = new EmbedBuilder(); // Create new embedded message
                    embed.WithImageUrl(image); // embed the image within the message
                    embed.WithColor(new Color(255, 82, 41)); // set embedded message trim colour to orange
                    embed.WithFooter(footer => // embedded message footer builder
                    {
                        footer
                        .WithText($"Requested by {Context.User.Username} at {DateTime.Now.ToString("t")} | From: r/animegifs") // footer data, "Requested by [name] at [time] | from [place]
                        .WithIconUrl(Context.User.GetAvatarUrl(ImageFormat.Auto, 64)); // get users avatar for use in footer
                    });
                    var final = embed.Build(); // final = constructed embedded message
                    await Context.Channel.SendMessageAsync("", false, final); // post embedded message
                }
                else if(image.EndsWith(".gifv")) // .gifv causes Discord not to display the image... for some reason
                {
                    post = false; // do not post it
                }
                else // if its not a gif
                {
                    post = false; // do not post it
                }
            } while (post == false); // Loop until its a gif
        }
        [Command("justfuckmyshitup")]
        [Alias("fuckmeup", "fam","fuckmeupfam", "justfuckmeupfam", "justfuckmeup")]
        [Summary("Post pictures of people who've just been fucked up fam :100:")] // command summary
        public async Task Fuckmeup()
        {
            bool post = true; // Post default to true
            do
            {
                string weblink = "https://www.reddit.com/r/Justfuckmyshitup/random/.json"; // api link
                string json = ""; //default json to ""
                var httpClient = new HttpClient(); //This acts like a web browser
                json = await httpClient.GetStringAsync(weblink); // set json to api string

                var dataObject = JsonConvert.DeserializeObject<dynamic>(json); // deserialize json
                string image = dataObject[0].data.children[0].data.url.ToString(); // pull image from api string
                string posttitle = dataObject[0].data.children[0].data.title.ToString();

                if (image.Contains(".jpg") || image.Contains(".png") || image.Contains(".gif"))
                {
                    post = true;
                    var embed = new EmbedBuilder();
                    embed.WithImageUrl(image);
                    embed.WithAuthor(author =>
                    {
                        author
                            .WithName($"{posttitle}\n")
                            .WithUrl("https://www.reddit.com/r/Justfuckmyshitup")
                            .WithIconUrl("http://cdn.edgecast.steamstatic.com/steamcommunity/public/images/avatars/ea/ea879dd914a94d7f719bb553306786fa5ae6acb0_full.jpg"); // duckybot logo, displayed beside author text
                    });
                    embed.WithColor(new Color(255, 82, 41));
                    embed.WithFooter(footer =>
                    {
                        footer
                        .WithText($"Requested by {Context.User.Username} at {DateTime.Now.ToString("t")} | From: r/Justfuckmyshitup")
                        .WithIconUrl(Context.User.GetAvatarUrl(ImageFormat.Auto, 64));
                    });
                    var final = embed.Build();
                    await Context.Channel.SendMessageAsync("", false, final);
                }
                else
                {
                    post = false;
                }
            } while (post == false); // Loop until DogImage doesnt have an .mp4 extension
        }
    }
}
