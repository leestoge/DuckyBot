using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DuckyBot.Core.Modules.Commands
{
    public class BattlefieldModule : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    {
        [Command("BFV")]
        [Summary("Show an overview of an input users Battlefield V stats")]
        public async Task BFVStats([Remainder] string originID)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })) //This acts like a web browser
            {
                var websiteurl = $"https://api.battlefieldtracker.com/api/v1/bfv/profile/origin/{originID}"; // The API site
                client.BaseAddress = new Uri(websiteurl); // Redirects our acting web browser to the API site
                var response = client.GetAsync("").Result; // Verify connection to site
                response.EnsureSuccessStatusCode(); // Verify connection to site
                var result = await response.Content.ReadAsStringAsync(); // Gets full website information
                var dataObject = JsonConvert.DeserializeObject<dynamic>(result); // de-serialise json

                string STATUS = dataObject["status"];

                if (STATUS == "Success")
                {
                    string name = dataObject["platformUserHandle"];
                    string AVATAR = dataObject["avatarUrl"];

                    string playTime = dataObject["data"].stats.timePlayed.displayValue.ToString();

                    string SPM = dataObject["data"].stats.scorePerMinute.displayValue.ToString();
                    string SPM_Per = dataObject["data"].stats.scorePerMinute.displayPercentile.ToString();

                    string KD = dataObject["data"].stats.kdRatio.displayValue.ToString();
                    string KD_Per = dataObject["data"].stats.kdRatio.displayPercentile.ToString();

                    string Accuracy = dataObject["data"].stats.shotsAccuracy.displayValue.ToString();
                    string Accuracy_Per = dataObject["data"].stats.shotsAccuracy.displayPercentile.ToString();

                    string KPM = dataObject["data"].stats.killsPerMinute.displayValue.ToString();
                    string KPM_Per = dataObject["data"].stats.killsPerMinute.displayPercentile.ToString();

                    string WinPercentage = dataObject["data"].stats.wlPercentage.displayValue.ToString();
                    string WinPercentage_Per = dataObject["data"].stats.wlPercentage.displayPercentile.ToString();

                    string Damage = dataObject["data"].stats.damage.displayValue.ToString();
                    string Damage_Per = dataObject["data"].stats.damage.displayPercentile.ToString();

                    string Heals = dataObject["data"].stats.heals.displayValue.ToString();
                    string Heals_Per = dataObject["data"].stats.heals.displayPercentile.ToString();

                    string Revives = dataObject["data"].stats.revives.displayValue.ToString();
                    string Revives_Per = dataObject["data"].stats.revives.displayPercentile.ToString();

                    string Resupplies = dataObject["data"].stats.resupplies.displayValue.ToString();
                    string Resupplies_Per = dataObject["data"].stats.resupplies.displayPercentile.ToString();

                    string Repairs = dataObject["data"].stats.repairs.displayValue.ToString();
                    string Repairs_Per = dataObject["data"].stats.repairs.displayPercentile.ToString();

                    string Killstreak = dataObject["data"].stats.killStreak.displayValue.ToString();
                    string Killstreak_Per = dataObject["data"].stats.killStreak.displayPercentile.ToString();

                    string Tagstaken = dataObject["data"].stats.dogtagsTaken.displayValue.ToString();
                    string Tagstaken_Per = dataObject["data"].stats.dogtagsTaken.displayPercentile.ToString();

                    var embed = new EmbedBuilder(); // Create new embedded message
                    embed.ThumbnailUrl = AVATAR;
                    embed.Description = $"Time played: `{playTime}`";
                    embed.WithColor(new Color(255, 82, 41)); // set embedded message trim colour to orange
                    embed.AddField("Score per minute", $"`{SPM}` ▸ __**{SPM_Per}**__", true);
                    embed.AddField("K/D ratio", $"`{KD}` ▸ __**{KD_Per}**__", true);
                    embed.AddField("Shot accuracy", $"`{Accuracy}` ▸ __**{Accuracy_Per}**__", true);
                    embed.AddField("Kills per minute", $"`{KPM}` ▸ __**{KPM_Per}**__", true);
                    embed.AddField("Win %", $"`{WinPercentage}` ▸ __**{WinPercentage_Per}**__", true);                  
                    embed.AddField("Damage", $"`{Damage}` ▸ __**{Damage_Per}**__", true);
                    embed.AddField("Heals", $"`{Heals}` ▸ __**{Heals_Per}**__", true);
                    embed.AddField("Revives", $"`{Revives}` ▸ __**{Revives_Per}**__", true);
                    embed.AddField("Resupplies", $"`{Resupplies}` ▸ __**{Resupplies_Per}**__", true);
                    embed.AddField("Repairs", $"`{Repairs}` ▸ __**{Repairs_Per}**__", true);
                    embed.AddField("Highest killstreak", $"`{Killstreak}` ▸ __**{Killstreak_Per}**__", true);
                    embed.AddField("Dogtags taken", $"`{Tagstaken}` ▸ __**{Tagstaken_Per}**__", true);

                    embed.WithAuthor(author =>
                    {
                        author.Name = name + "'s Battlefield V stats overview";
                        author.IconUrl = "http://cdn.edgecast.steamstatic.com/steamcommunity/public/images/avatars/ea/ea879dd914a94d7f719bb553306786fa5ae6acb0_full.jpg";
                    });

                    embed.WithFooter(footer => // embedded message footer builder
                    {
                        footer.WithText($"Requested by {Context.User.Username} at {DateTime.Now:t}") // footer data, "Requested by [name] at [time] | from [place]
                              .WithIconUrl(Context.User.GetAvatarUrl(ImageFormat.Auto, 64)); // get users avatar for use in footer
                    });

                    var final = embed.Build(); // final = constructed embedded message
                    await ReplyAsync("", false, final); // post embedded message
                }
                else
                {
                    await ReplyAsync($"Couldn't find stats for `{originID}`.");
                }
            }
        }

        [Command("willy")] // Command declaration
        [Alias("ds", "dick", "dicksize")] // command aliases (also trigger task)
        [Summary("Utilising a series of complex algorithms, work out the approximate dick size of a gamer from his battlefield V stats.")]
        // command summary
        public async Task BigDick([Remainder] string originID) // command async task (method basically)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })) //This acts like a web browser
            {
                var websiteurl = $"https://api.battlefieldtracker.com/api/v1/bfv/profile/origin/{originID}"; // The API site
                client.BaseAddress = new Uri(websiteurl); // Redirects our acting web browser to the API site
                var response = client.GetAsync("").Result; // Verify connection to site
                response.EnsureSuccessStatusCode(); // Verify connection to site
                var result = await response.Content.ReadAsStringAsync(); // Gets full website information
                var dataObject = JsonConvert.DeserializeObject<dynamic>(result); // de-serialise json

                string STATUS = dataObject["status"];

                if (STATUS == "Success")
                {
                    string name = dataObject["platformUserHandle"];
                    double KD = dataObject["data"].stats.kdRatio.displayValue;
                    double KPM = dataObject["data"].stats.killsPerMinute.displayValue;
                    double Damage = dataObject["data"].stats.damage.displayValue;
                    double Revives = dataObject["data"].stats.revives.displayValue;
                    double TagsTaken = dataObject["data"].stats.dogtagsTaken.displayValue;
                    double Deaths = dataObject["data"].stats.deaths.displayValue;
                    double Assists = dataObject["data"].stats.assists.displayValue;
                    double Wins = dataObject["data"].stats.wins.displayValue;
                    double Losses = dataObject["data"].stats.losses.displayValue;
                    double SPM = dataObject["data"].stats.scorePerMinute.displayValue;
                    double suppressionAssists = dataObject["data"].stats.suppressionAssists.displayValue;
                    double Headshots = dataObject["data"].stats.headshots.displayValue;
                    double RevivesReceived = dataObject["data"].stats.revivesRecieved.displayValue;
                    double longestHeadshot = dataObject["data"].stats.longestHeadshot.displayValue;

                    var val = Damage / Deaths / Assists * Revives + Wins * TagsTaken - (Wins - Losses) + SPM  - Math.Sqrt(suppressionAssists) - Math.PI - Math.Sqrt(RevivesReceived / 2);
                    var sqrtVal = Math.Sqrt(val / 777) + KPM + KD + Math.Sqrt(Headshots / 22);
                    var temp = Math.Sqrt(sqrtVal) + Math.Sqrt(Revives) / 96 + Math.Sqrt(SPM) / 6 - Math.Sqrt(longestHeadshot) / 8;
                    var final = Math.Round(temp, 2);                

                    if (final - Math.Round(final) != 0)
                    {
                        await ReplyAsync($"{name}'s dick is **{final:0.0} inches** long!");
                    }
                    else
                    {
                        await ReplyAsync($"{name}'s dick is **{Math.Round(final, 1)} inches** long!");
                    }
                }
                else
                {
                    await ReplyAsync($"Couldn't find stats for `{originID}`.");
                }
            }
        }

        [Command("saltlevel")] // Command declaration
        [Alias("sl", "salt", "kid")] // command aliases (also trigger task)
        [Summary("Utilising a series of complex algorithms, work out the approximate salt level of a gamer from his battlefield V stats.")]
        // command summary
        public async Task SaltyBoye([Remainder] string originID) // command async task (method basically)
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate })) //This acts like a web browser
            {
                var websiteurl = $"https://api.battlefieldtracker.com/api/v1/bfv/profile/origin/{originID}"; // The API site
                client.BaseAddress = new Uri(websiteurl); // Redirects our acting web browser to the API site
                var response = client.GetAsync("").Result; // Verify connection to site
                response.EnsureSuccessStatusCode(); // Verify connection to site
                var result = await response.Content.ReadAsStringAsync(); // Gets full website information
                var dataObject = JsonConvert.DeserializeObject<dynamic>(result); // de-serialise json

                string STATUS = dataObject["status"];

                if (STATUS == "Success")
                {
                    string name = dataObject["platformUserHandle"];

                    float Rounds = dataObject["data"].stats.rounds.displayValue;
                    float RoundsPlayed = dataObject["data"].stats.roundsPlayed.displayValue;

                    var saltLevel = Rounds - RoundsPlayed;

                    if (saltLevel != 0)
                    {
                        await ReplyAsync($"{name}'s salt level is `{saltLevel}`.");
                    }
                    else
                    {
                        await ReplyAsync($"{name} is at a salt level of `0`, impressive.");
                    }
                }
                else
                {
                    await ReplyAsync($"Couldn't find stats for `{originID}`.");
                }
            }
        }
    }
}
