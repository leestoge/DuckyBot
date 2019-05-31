using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DuckyBot.Core.LevelingSystem // *** DOESN'T WORK  LOL***
{
    internal static class Leveling /* CODE PROVIDED BY PETER/SPELOS - https://youtu.be/GpHFj9_aey0 */
    {
        internal static async Task UserSentMessage(SocketGuildUser user, SocketTextChannel channel)
        {
            var userAccount = UserAccounts.UserAccounts.GetAccount(user); // get user that just typed
            uint oldLevel = userAccount.LevelNumber; // store their previous level

            userAccount.XP += 10; // add the xp
            UserAccounts.UserAccounts.SaveAccounts(); //save
            uint newLevel = userAccount.LevelNumber; // store their current level

            if (oldLevel != newLevel) // if old level not equal to new level they must have gained a level
            {
                var embed = new EmbedBuilder // create new embed
                {
                    Color = new Color(255, 82, 41), // embed colour (orange)
                    Author = new EmbedAuthorBuilder // create new author within embed (used as a title when displayed)
                    {
                        Name = user.Username + " just levelled up!", // author text
                        IconUrl =
                            "http://cdn.edgecast.steamstatic.com/steamcommunity/public/images/avatars/ea/ea879dd914a94d7f719bb553306786fa5ae6acb0_full.jpg" // duckybot logo, displayed beside author text
                    }
                };
                embed.AddField("LEVEL", newLevel, true); // users current level
                embed.AddField("Current XP", userAccount.XP, true); // users current xp
                await channel.SendMessageAsync(embed.ToString()); // post embed
            }
        }
    }
}