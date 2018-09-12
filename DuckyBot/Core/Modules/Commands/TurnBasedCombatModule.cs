using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using static DuckyBot.Core.Utilities.RandomGen;
using System;

namespace DuckyBot.Core.Modules.Commands
{
    public class TurnBasedCombat : ModuleBase<SocketCommandContext> // Define module and direct to command handler
    //SocketCommandContext allows access to the message, channel, server and user the command was invoked by, so works as the base as to how the bot knows where to reply/what user used which command, etc.
    {
        static string fight_Status = "nofight"; // default to no ongoing fight

        static string player1; // player 1 identifier
        static string player2; // player 2 identifier

        static string whosTurn; // current turn
        static string whoWaits; // current waiting
        static string placeHolder; // placeholder to switch between whosTurn and whoWaits

        static int player1HP = 100; //player1 hp
        static int player2HP = 100; //player2 hp
        static int player1AP = 20; //player1 ap
        static int player2AP = 20; //player2 ap
        static int player1Hpot = 2; //player1 hp potion
        static int player1Apot = 2; //player1 ap potion
        static int player2Hpot = 2; //player2 hp potion
        static int player2Apot = 2; //player2 ap potion

        // string arrays with strings relating to the type of command used - all self implemented
        string[] punchTexts = new string[]
        {
            "slapped",
            "hit",
            "punched",
            "uppercut",
            "chopped",
            "smashed",
            "bonked",
            "smacked",
            "wallopped",
            "karate chopped",
            "jabbed",
            "pummeled",
            "biffed",
            "whacked",
            "battered",
        };
        string[] kickTexts = new string[]
        {
            "knocked",
            "hit",
            "kicked",
            "smashed",
            "bonked",
            "smacked",
            "wallopped",
            "roundhouse kicked",
            "belted",
            "bashed",
            "pummeled",
            "booted",
            "whacked",
            "bumped",
            "thumped",
            "thrashed",
        };
        string[] critTexts = new string[]
        {
            ", a serious wound is inflicted.",
            ", the blow knocks them to the ground.",
            ", leaving a big bruise.",
            ", inflicting an extra helping of damage.",
            ", unfortunately, their spine is now clearly visible from the front.",
            ", knocking them to the ground like a bowling pin in a league game.",
            ", inflicting some extra pain!",
            ", the pain is too much for them, and they fall to the ground.",
            ", ouch! That had to hurt!",
            ", and without protection, they fall over, groaning in agony.",
            ", the pain is too much for them and they collapse like a rag.",
            ", striking a vital spot.",
            ", gallons of viscera spurt to the ground.",
            ", sending them flying to the floor.",
            ", sending them flying on their back.",
            ", sending them flying across the room.",
            ", their skull makes several unhealthy sounds.",
            ", even Michael 'Ducky' Martin would be impressed by such a blow!",
            ", it's as if the power of Lord Ducky flowed through that vicious blow!",
            ", an absolutely wappy attack!",
            ", a devastating, wappy attack!",
            ", it's the most tremendous, wappiest, 6th gearest attack you have ever witnessed!",
            ", it's the most tremendous, wappiest, 6th gearest attack you have ever seen!",
            ", they must have trained with Michael 'Ducky' Martin to inflict such a blow!",
            ", it's the wappiest attack you've ever seen!",
            ", the vicious blow sends them across the room.",
            ", a truly wappy attack!",
            ", Michael's spirit looks down and applauds at that tremendous attack.",
            ", Michael's spirit looks down and applauds at that wappy attack.",
            ", holy shit.",
            ", smacking them into full fucking 6th gear!",
            ", knocking them into full fucking 6th gear!",
            ", Meanwhile Tom is absolutely devouring a donut.",
            ", while this has went on, Tom has ate fifty donuts!",
            ", the devastating attack sends shockwaves through the room!",
            ", the devastating attack sends shockwaves through the room, causing all the crumbs to escape from Tom's beard!",
            ", you've seen this raw strength only once before... <:Duck2:315947942903283714>",
        };
        string[] tauntTexts = new string[]
        {
            "performs the jig of the tanians!",
            "does the long john dance of the long john people!",
            "calls you a jipped cunt!",
            "calls you an empty john!",
            "tells you that your beliefs on being a long john are false, and that you are infact a short john!",
            "shakes their fist at you while mumbling about long johns!",
            "tells you that they are gonna thrust their foot upside your arsehole!",
            "insults your mother!",
            "lets out a  loud screech at you, it is believed this high pitched screech is the battlecry of the tanians!",
            "calls you a small fry!",
            "calls you an absolute cpu idiot!",
            "calls you a brisky idiot!",
            "says you couldn't even flingmeoff mingoff!",
        };
        [Command("fight")] // command declaration
        [Alias("Fight", "battle")] // command aliases (also trigger task)
        [Summary("starts a fight with the @Mention user :boom:")] // command summary
        [RequireBotPermission(GuildPermission.ManageChannels)] // Needed Bot Permissions 
        public async Task Fight(IUser user) // command async task that takes in a parameter of a user (mentioned)
        {
            if (user.Id != 315499394810249216)
            {
                await Context.Channel.SendMessageAsync("A Fight has started between " + Context.User.Mention + " and " + user.Mention + "!");

                if (Context.User.Mention != user.Mention && fight_Status == "nofight") // if user entering command is not equal to the user mentioned in the command parameter and fight status is set to no ongoing fight
                {
                    fight_Status = "fight_proceed"; // set status to fight ongoing
                    player1 = Context.User.Mention; // player 1 - differentiated by context - meaning the user who initially entered the command
                    player2 = user.Mention; // player 2 - differentiated by being the one mentioned as the command parameter

                    var worldstar = await Context.Guild.CreateTextChannelAsync("WORLDSTAR"); // create the "WORLDSTAR" text channel

                    string[] whoStarts = new string[] // string array with the name "whoStarts" provides choice of player 1 or player 2
                    {
                    Context.User.Mention, // player 1
                    user.Mention // player 2
                    };
                    int rando = StaticRandom.Instance.Next(whoStarts.Length); // get random number between 0 and array length
                    string text = whoStarts[rando]; // store string at the random number position in the array
                    whosTurn = text; // save the stored string as string "text"
                    if (text == Context.User.Mention) // if player 1
                    {
                        whoWaits = user.Mention; // player 2 waits
                    }
                    else // if player 2
                    {
                        whoWaits = Context.User.Mention; // player 1 waits
                    }
                    // Fight commands, info and which players turn it currently is.
                    await worldstar.SendMessageAsync("Fight Commands!\n\n" + "`!punch`/`!hit` - Punch your opponent :punch:, hard to miss, inflicts low damage. `(Costs 2 Action Points)`\n" + "`!kick`/`!boot` - Kick your opponent :boot:, easy to miss, but inflicts more damage. `(Costs 4 Action Points)`\n" + "`!chop`/`!crit` - Devastating precision attack, very easy to miss, but inflicts very high damage :boom: `(Costs 6 Action Points)`\n" + "`!taunt` - Taunt your opponent :middle_finger:, possible to fail, but will lower your opponents AP if successful. `(Costs 2 Action Points)`\n" + "`!ap`/`!action` - Drink an Action Potion to restore some action points! :zap: `(Restores between 10-20 Action Points)`\n" + "`!hp`/`!health` - Drink a Health potion to restore some health points! :heart: `(Restores between 10-40 Action Points)`\n" + "`!run`/`!giveup` - Forfeits the fight. :runner: `(Costs 0 Action Points and can be performed at any time)`\n\n"); // fight commands and rules
                    await worldstar.SendMessageAsync("A Fight has started between " + Context.User.Mention + " and " + user.Mention + "!\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + "  action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + text + " your turn!"); //whos turn it currently is and current hp, ap, etc of players
                }
                else // if user mentions self, or tries to fight when a fight is ongoing
                {
                    await ReplyAsync(Context.User.Mention + " sorry but there is a fight going on right now or you just tried to fight yourself.");
                }
            }
            else
            {
                    await Context.Channel.SendMessageAsync(Context.User.Mention + " you can't fight me!"); // notify user
                    var application = await Context.Client.GetApplicationInfoAsync(); // gets channels from discord client
                    var z = await application.Owner.GetOrCreateDMChannelAsync(); // find my dm channel in order to private message me
                    await z.SendMessageAsync($"[{DateTime.Now.ToString("t")}] **{Context.User.Username}** tried to fight DuckyBot."); // private message me with error reason (with time error occured)
            }
        }
        [Command("run")] // command declaration
        [Alias("GiveUp", "Giveup", "giveUp", "Run", "RUN")] // command aliases (also trigger task)
        [Summary("Forfeits the fight. :runner:")] // command summary
        public async Task GiveUp() // command async task (method basically)
        {
            if (Context.Channel.Name == "worldstar") // check if bot is posting/focused on the right channel (worldstar)
            {
                if (fight_Status == "fight_proceed") //if fight ongoing
                {
                    await ReplyAsync("The fight has stopped."); // notify user that fight has stopped
                    fight_Status = "nofight"; // reset status to no fight ongoing.
                    player1HP = 100;
                    player2HP = 100;
                    player1AP = 20;
                    player2AP = 20; // reset hp, ap, potions to default values
                    player1Hpot = 2;
                    player1Apot = 2;
                    player2Hpot = 2;
                    player2Apot = 2;

                    if (Context.Channel.Name == "worldstar") // secondary check for channel as we are about to PERMANENTLY delete a text channel
                    {
                        await Context.Channel.SendMessageAsync("`#worldstar channel will be deleted in 10 seconds.`"); // notify user of deletion timer - gives users time to see ending results, etc.
                        await Task.Delay(10000); // delay deletion task by 10 seconds
                        await (Context.Channel as IGuildChannel).DeleteAsync(); // delete WORLDSTAR text channel
                    }
                }
                else //if no fight ongoing
                {
                    await ReplyAsync("There is no fight to stop."); // notify user there is no ongoing fight
                }
            }
            else // if fight commands not posted in "WORLDSTAR" channel
            {
                await Context.Channel.SendMessageAsync("Please only type fight commands in the `#worldstar` channel"); // notify user to only type commands in the "WORLDSTAR" channel
            }
        }
        [Command("punch")] // command declaration
        [Alias("Punch", "hit")] // command aliases (also trigger task)
        [Summary("Punch your opponent :punch:, hard to miss, low damage. (Costs 2 Action Points)")] // command summary
        public async Task Punch() // command async task (method basically)
        {
            if (Context.Channel.Name == "worldstar") // check if bot is posting/focused on the right channel (worldstar)
            {
                int randomText = StaticRandom.Instance.Next(punchTexts.Length); // get random number between 0 and array length
                string text = punchTexts[randomText]; // store string at the random number position in the array as string "text" - results in random phrases to go along with command if it doesnt miss

                if (fight_Status == "fight_proceed") // if fight ongoing
                {
                    if (whosTurn == Context.User.Mention) // last mentioned players turn - (as in, the bottom text saying @user your turn!) - see screen layouts under "Design" heading for an example
                    {
                        int chanceToHit = StaticRandom.Instance.Next(1, 10); // get random number between 1 to 10 and store as integer "chanceToHit"
                        if (chanceToHit != 1) // if "chanceToHit" is not equal to 1 then proceed
                        {
                            int randomMultiplier = StaticRandom.Instance.Next(1, 4); // get random number between 1 to 4 and store as integer "randomMultiplier"
                            int damageValue = randomMultiplier * 2; // integer "damageValue" equals "randomMultiplier" times 2 - results in a range of 2 - 8 damage

                            if (Context.User.Mention != player1) // if player 2's turn
                            {
                                if (player1HP > 0 && player2AP >= 2) // and player 2 has at least 2 AP and player 1 has more than 0 hp
                                {
                                    player1HP = player1HP - damageValue; // player 1 loses "damageValue" amount of health
                                    player2AP = player2AP - 2; // player 2 loses 2 action points

                                    placeHolder = whosTurn;
                                    whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                                    whoWaits = placeHolder;

                                    // post result in chat v - (for example:- "user 1 [text]'d user 2 and did [damageValue] damage")
                                    await ReplyAsync(Context.User.Mention + " " + text + " " + player1 + " and did `" + damageValue + " damage!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");

                                    if (player1HP <= 0) // if player 1 has 0 or less hp (is dead)
                                    {
                                        await ReplyAsync(Context.User.Mention + " has defeated " + player1 + "!\n\n" + player2 + " wins!"); // post death message - (for example:- "user1 defeated user2, user2 wins!")
                                        fight_Status = "nofight"; // reset status to no fight ongoing.
                                        player1HP = 100;
                                        player2HP = 100;
                                        player1AP = 20;
                                        player2AP = 20; // reset hp,ap, potions
                                        player1Hpot = 2;
                                        player1Apot = 2;
                                        player2Hpot = 2;
                                        player2Apot = 2;
                                        if (Context.Channel.Name == "worldstar") // secondary check for channel as we are about to PERMANENTLY delete a text channel
                                        {
                                            await Context.Channel.SendMessageAsync("`#worldstar channel will be deleted in 10 seconds.`"); // notify user of deletion timer - gives users time to see ending results, etc.
                                            await Task.Delay(10000); // delay deletion task by 10 seconds
                                            await (Context.Channel as IGuildChannel).DeleteAsync(); // delete WORLDSTAR text channel
                                        }
                                    }
                                }
                                else if (player2AP < 2) // if player 2 has less than 2 action points
                                {
                                    await ReplyAsync(Context.User.Mention + " You do not have enough Action Points for this attack!\n\n" + whosTurn + " your turn!"); // notify them they must have at least 2 action points
                                }
                                else if (player2AP <= 0) // if player 2 has 0 or less AP
                                {
                                    await ReplyAsync(Context.User.Mention + " You have no Action Points left! Use an Action potion!\n\n" + whosTurn + " your turn!"); // notify them they should drink an action point potion
                                }
                            }
                            else if (Context.User.Mention == player1) // Player 1's turn
                            {
                                if (player2HP > 0 && player1AP >= 2) // and player 1 has at least 2 AP and player 2 has more than 0 hp
                                {
                                    player2HP = player2HP - damageValue; // player 2 loses "damageValue" amount of health
                                    player1AP = player1AP - 2; // player 1 loses 2 action points

                                    placeHolder = whosTurn;
                                    whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                                    whoWaits = placeHolder;

                                    // post result in chat v - (for example:- "user 1 [text]'d user 2 and did [damageValue] damage")
                                    await ReplyAsync(Context.User.Mention + " " + text + " " + player2 + " and did `" + damageValue + " damage!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");

                                    if (player2HP <= 0) // if player 2 has 0 or less hp (is dead)
                                    {
                                        await ReplyAsync(Context.User.Mention + " has defeated " + player2 + "!\n\n" + player1 + " wins!"); // post death message - (for example:- "user1 defeated user2, user2 wins!")
                                        fight_Status = "nofight"; // reset status to no fight ongoing.
                                        player1HP = 100;
                                        player2HP = 100;
                                        player1AP = 20;
                                        player2AP = 20; // reset hp,ap, potions
                                        player1Hpot = 2;
                                        player1Apot = 2;
                                        player2Hpot = 2;
                                        player2Apot = 2;
                                        if (Context.Channel.Name == "worldstar") // secondary check for channel as we are about to PERMANENTLY delete a text channel
                                        {
                                            await Context.Channel.SendMessageAsync("`#worldstar channel will be deleted in 10 seconds.`"); // notify user of deletion timer - gives users time to see ending results, etc.
                                            await Task.Delay(10000); // delay deletion task by 10 seconds
                                            await (Context.Channel as IGuildChannel).DeleteAsync(); // delete WORLDSTAR text channel
                                        }
                                    }
                                }
                                else if (player1AP < 2) // if player 1 has less than 2 action points
                                {
                                    await ReplyAsync(Context.User.Mention + " You do not have enough Action Points for this attack!\n\n" + whosTurn + " your turn!"); // notify them they must have at least 2 action points
                                }
                                else if (player1AP <= 0) // if player 1 has 0 or less AP
                                {
                                    await ReplyAsync(Context.User.Mention + " You have no Action Points left! Use an Action potion!\n\n" + whosTurn + " your turn!"); // notify them they should drink an action point potion
                                }
                            }
                            else // if it's somehow neither players turn
                            {
                                await ReplyAsync("Sorry it seems like something went wrong. Please type `!run`"); // notify user that an error has occured and suggest ceasing the fight
                            }
                        }
                        else // if "chanceToHit" is equal to 1
                        { // pass the turn of the player who missed
                            placeHolder = whosTurn;
                            whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                            whoWaits = placeHolder;

                            await ReplyAsync(Context.User.Mention + " you missed!\n" + whosTurn + " your turn!"); // notify them that they missed their action
                        }
                    }
                    else // if user tries to use this command when it isnt their turn
                    {
                        await ReplyAsync(Context.User.Mention + " it is not your turn."); // notify them that it is not their turn
                    }
                }
                else // if user tries to use this command when fight status is set to "nofight"
                {
                    await ReplyAsync("There is no fight at the moment."); // notify user there is no ongoing fight
                }
            }
            else // if fight commands not posted in "WORLDSTAR" channel
            {
                await Context.Channel.SendMessageAsync("Please only type fight commands in the `#worldstar` channel"); // notify user to only type commands in the "WORLDSTAR" channel
            }
        }
        [Command("kick")] // command declaration
        [Alias("Kick", "boot")] // command aliases (also trigger task)
        [Summary("Kick your opponent :boot:, easy to miss, but inflicts more damage. (Costs 4Action Points)")] // command summary
        public async Task Kick() // command async task (method basically)
        {
            if (Context.Channel.Name == "worldstar") // check if bot is posting/focused on the right channel (worldstar)
            {
                int randomText = StaticRandom.Instance.Next(kickTexts.Length); // get random number between 0 and array length
                string text = kickTexts[randomText]; // store string at the random number position in the array as string "text" - results in random phrases to go along with command if it doesnt miss

                if (fight_Status == "fight_proceed") // if fight ongoing
                {
                    if (whosTurn == Context.User.Mention) // last mentioned players turn - (as in, the bottom text saying @user your turn!) - see screen layouts under "Design" heading for an example
                    {
                        int chanceToHit = StaticRandom.Instance.Next(1, 6); // get random number between 1 to 6 and store as integer "chanceToHit"
                        if (chanceToHit != 1) // if "chanceToHit" is not equal to 1 then proceed
                        {
                            int randomMultiplier = StaticRandom.Instance.Next(2, 7); // get random number between 2 to 7 and store as integer "randomMultiplier"
                            int damageValue = randomMultiplier * 2; // integer "damageValue" equals "randomMultiplier" times 2 - results in a range of 4 - 14 damage

                            if (Context.User.Mention != player1) // if player 2's turn
                            {
                                if (player1HP > 0 && player2AP >= 4) // and player 2 has at least 2 AP and player 1 has more than 0 hp
                                {
                                    player1HP = player1HP - damageValue; // player 1 loses "damageValue" amount of health
                                    player2AP = player2AP - 4; // player 2 loses 4 action points

                                    placeHolder = whosTurn;
                                    whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                                    whoWaits = placeHolder;

                                    // post result in chat v - (for example:- "user 1 [text]'d user 2 and did [damageValue] damage")
                                    await ReplyAsync(Context.User.Mention + " " + text + " " + player1 + " and did `" + damageValue + " damage!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");

                                    if (player1HP <= 0) // if player 1 has 0 or less hp (is dead)
                                    {
                                        await ReplyAsync(Context.User.Mention + " has defeated " + player1 + "!\n\n" + player2 + " wins!"); // post death message - (for example:- "user1 defeated user2, user2 wins!")
                                        fight_Status = "nofight"; // reset status to no fight ongoing.
                                        player1HP = 100;
                                        player2HP = 100;
                                        player1AP = 20;
                                        player2AP = 20; // reset hp,ap, potions
                                        player1Hpot = 2;
                                        player1Apot = 2;
                                        player2Hpot = 2;
                                        player2Apot = 2;
                                        if (Context.Channel.Name == "worldstar") // secondary check for channel as we are about to PERMANENTLY delete a text channel
                                        {
                                            await Context.Channel.SendMessageAsync("`#worldstar channel will be deleted in 10 seconds.`"); // notify user of deletion timer - gives users time to see ending results, etc.
                                            await Task.Delay(10000);  // delay deletion task by 10 seconds
                                            await (Context.Channel as IGuildChannel).DeleteAsync(); // delete WORLDSTAR text channel
                                        }
                                    }
                                }
                                else if (player2AP <= 0) // if player 2 has 0 or less AP
                                {
                                    await ReplyAsync(Context.User.Mention + " You have no Action Points left! Use an Action potion!\n\n" + whosTurn + " your turn!"); // notify them they should drink an action point potion
                                }
                                else if (player2AP < 4) // if player 2 has less than 4 action points
                                {
                                    await ReplyAsync(Context.User.Mention + " You do not have enough Action Points for this attack!\n\n" + whosTurn + " your turn!"); // notify them they must have at least 4 action points
                                }
                            }
                            else if (Context.User.Mention == player1) // Player 1's turn
                            {
                                if (player2HP > 0 && player1AP >= 4) // and player 1 has at least 4 AP and player 2 has more than 0 hp
                                {
                                    player2HP = player2HP - damageValue; // player 2 loses "damageValue" amount of health
                                    player1AP = player1AP - 4; // player 1 loses 4 action points

                                    placeHolder = whosTurn;
                                    whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                                    whoWaits = placeHolder;

                                    // post result in chat v - (for example:- "user 1 [text]'d user 2 and did [damageValue] damage")
                                    await ReplyAsync(Context.User.Mention + " " + text + " " + player2 + " and did `" + damageValue + " damage!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");

                                    if (player2HP <= 0) // if player 2 has 0 or less hp (is dead)
                                    {
                                        await ReplyAsync(Context.User.Mention + " has defeated " + player2 + "!\n\n" + player1 + " wins!"); // post death message - (for example:- "user1 defeated user2, user2 wins!")
                                        fight_Status = "nofight"; // reset status to no fight ongoing.
                                        player1HP = 100;
                                        player2HP = 100;
                                        player1AP = 20;
                                        player2AP = 20; // reset hp,ap, potions
                                        player1Hpot = 2;
                                        player1Apot = 2;
                                        player2Hpot = 2;
                                        player2Apot = 2;
                                        if (Context.Channel.Name == "worldstar") // secondary check for channel as we are about to PERMANENTLY delete a text channel
                                        {
                                            await Context.Channel.SendMessageAsync("`#worldstar channel will be deleted in 10 seconds.`"); // notify user of deletion timer - gives users time to see ending results, etc.
                                            await Task.Delay(10000); // delay deletion task by 10 seconds
                                            await (Context.Channel as IGuildChannel).DeleteAsync(); // delete WORLDSTAR text channel
                                        }
                                    }
                                }
                                else if (player1AP <= 0) // if player 1 has 0 or less AP
                                {
                                    await ReplyAsync(Context.User.Mention + " You have no Action Points left! Use an Action potion!\n\n" + whosTurn + " your turn!"); // notify them they should drink an action point potion
                                }
                                else if (player1AP < 4) // if player 1 has less than 4 action points
                                {
                                    await ReplyAsync(Context.User.Mention + " You do not have enough Action Points for this attack!\n\n" + whosTurn + " your turn!"); // notify them they must have at least 4 action points
                                }
                            }
                            else // if it's somehow neither players turn
                            {
                                await ReplyAsync("Sorry it seems like something went wrong. Please type `!run`"); // notify user that an error has occured and suggest ceasing the fight
                            }
                        }
                        else // if "chanceToHit" is equal to 1
                        { // pass the turn of the player who missed
                            placeHolder = whosTurn;
                            whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                            whoWaits = placeHolder;

                            await ReplyAsync(Context.User.Mention + " you missed!\n" + whosTurn + " your turn!"); // notify them that they missed their action
                        }
                    }
                    else // if user tries to use this command when it isnt their turn
                    {
                        await ReplyAsync(Context.User.Mention + " it is not your turn."); // notify them that it is not their turn
                    }
                }
                else // if user tries to use this command when fight status is set to "nofight"
                {
                    await ReplyAsync("There is no fight at the moment."); // notify user there is no ongoing fight
                }
            }
            else // if fight commands not posted in "WORLDSTAR" channel
            {
                await Context.Channel.SendMessageAsync("Please only type fight commands in the `#worldstar` channel"); // notify user to only type commands in the "WORLDSTAR" channel
            }
        }
        [Command("taunt")] // command declaration
        [Alias("Taunt")] // command aliases (also trigger task)
        [Summary("Taunt your opponent :middle_finger:, possible to fail, but will lower your opponents AP if successful. (Costs 2 Action Points)")] // command summary
        public async Task Taunt() // command async task (method basically)
        {
            if (Context.Channel.Name == "worldstar") // check if bot is posting/focused on the right channel (worldstar)
            {
                int randomText = StaticRandom.Instance.Next(tauntTexts.Length); // get random number between 0 and array length
                string text = tauntTexts[randomText]; // store string at the random number position in the array as string "text" - results in random phrases to go along with command if it doesnt miss

                if (fight_Status == "fight_proceed") // if fight ongoing
                {
                    if (whosTurn == Context.User.Mention) // last mentioned players turn - (as in, the bottom text saying @user your turn!) - see screen layouts under "Design" heading for an example
                    {
                        int chanceToHit = StaticRandom.Instance.Next(1, 5); // get random number between 1 to 5 and store as integer "chanceToHit"
                        if (chanceToHit != 1) // if "chanceToHit" is not equal to 1 then proceed
                        {
                            int randomMultiplier = StaticRandom.Instance.Next(1, 5); // get random number between 1 to 5 and store as integer "randomMultiplier"
                            int damageValue = randomMultiplier * 2; // integer "damageValue" equals "randomMultiplier" times 2 - results in a range of 2 - 10 damage

                            if (Context.User.Mention != player1) // if player 2's turn
                            {
                                if (player2AP >= 2) // and player 2 has at least 2 AP
                                {
                                    player1AP = player1AP - damageValue; // player 1 loses "damageValue" amount of action points
                                    player2AP = player2AP - 2; // player 2 loses 2 action points

                                    placeHolder = whosTurn;
                                    whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                                    whoWaits = placeHolder;

                                    // post result in chat v - (for example:- "user 1 [text], your action points have lowered by [damageValue]!")
                                    await ReplyAsync(Context.User.Mention + " " + text + " `Your action points have lowered by " + damageValue + "!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");
                                }
                                else if (player2AP < 2) // if player 2 has less than 2 action points
                                {
                                    await ReplyAsync(Context.User.Mention + " You do not have enough Action Points for this action!\n\n" + whosTurn + " your turn!"); // notify them they must have at least 4 action points
                                }
                                else if (player2AP <= 0) // if player 2 has 0 or less AP
                                {
                                    await ReplyAsync(Context.User.Mention + " You have no Action Points left! Use an Action potion!\n\n" + whosTurn + " your turn!"); // notify them they should drink an action point potion
                                }
                            }
                            else if (Context.User.Mention == player1) // Player 1's turn
                            {
                                if (player1AP >= 2) // and player 1 has at least 2 AP
                                {
                                    player2AP = player2AP - damageValue; // player 2 loses "damageValue" amount of action points
                                    player1AP = player1AP - 2; // player 1 loses 2 action points

                                    placeHolder = whosTurn;
                                    whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                                    whoWaits = placeHolder;

                                    // post result in chat v - (for example:- "user 1 [text], your action points have lowered by [damageValue]!")
                                    await ReplyAsync(Context.User.Mention + " " + text + " `Your action points have lowered by " + damageValue + "!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");
                                }
                                else if (player1AP < 2) // if player 2 has less than 2 action points
                                {
                                    await ReplyAsync(Context.User.Mention + " You do not have enough Action Points for this action!\n\n" + whosTurn + " your turn!"); // notify them they must have at least 4 action points
                                }
                                else if (player1AP <= 0) // if player 2 has 0 or less AP
                                {
                                    await ReplyAsync(Context.User.Mention + " You have no Action Points left! Use an Action potion!\n\n" + whosTurn + " your turn!"); // notify them they should drink an action point potion
                                }
                            }
                            else // if it's somehow neither players turn
                            {
                                await ReplyAsync("Sorry it seems like something went wrong. Please type `!run`"); // notify user that an error has occured and suggest ceasing the fight
                            }
                        }
                        else // if "chanceToHit" is equal to 1
                        { // pass the turn of the player who missed
                            placeHolder = whosTurn;
                            whosTurn = whoWaits;
                            whoWaits = placeHolder;

                            await ReplyAsync(Context.User.Mention + " " + text + " Unfortunately their attempts to taunt you are futile.\n" + whosTurn + " your turn!"); // notify them that they missed their action
                        }
                    }
                    else // if user tries to use this command when it isnt their turn
                    {
                        await ReplyAsync(Context.User.Mention + " it is not your turn."); // notify them that it is not their turn
                    }
                }
                else // if user tries to use this command when fight status is set to "nofight"
                {
                    await ReplyAsync("There is no fight at the moment."); // notify user there is no ongoing fight
                }
            }
            else // if fight commands not posted in "WORLDSTAR" channel
            {
                await Context.Channel.SendMessageAsync("Please only type fight commands in the `#worldstar` channel"); // notify user to only type commands in the "WORLDSTAR" channel
            }
        }
        [Command("chop")] // command declaration
        [Alias("Chop", "crit")] // command aliases (also trigger task)
        [Summary("Devastating precision attack :martial_arts_uniform:, very easy to miss, but inflicts very high damage (Costs 6 Action Points)")] // command summary
        public async Task Chop() // command async task (method basically)
        {
            if (Context.Channel.Name == "worldstar") // check if bot is posting/focused on the right channel (worldstar)
            {
                int randomText = StaticRandom.Instance.Next(critTexts.Length); // get random number between 0 and array length
                string text = critTexts[randomText]; // store string at the random number position in the array as string "text" - results in random phrases to go along with command if it doesnt miss

                if (fight_Status == "fight_proceed") // if fight ongoing
                {
                    if (whosTurn == Context.User.Mention) // last mentioned players turn - (as in, the bottom text saying @user your turn!) - see screen layouts under "Design" heading for an example
                    {
                        int chanceToHit = StaticRandom.Instance.Next(1, 3); // get random number between 1 to 3 and store as integer "chanceToHit"
                        if (chanceToHit != 1) // if "chanceToHit" is not equal to 1 then proceed
                        {
                            int randomMultiplier = StaticRandom.Instance.Next(10, 30); // get random number between 10 to 30 and store as integer "randomMultiplier"
                            int damageValue = randomMultiplier * 2; // integer "damageValue" equals "randomMultiplier" times 2 - results in a range of 20 - 60 damage

                            if (Context.User.Mention != player1) // if player 2's turn
                            {
                                if (player1HP > 0 && player2AP >= 6) // and player 2 has at least 6 AP and player 1 has more than 0 hp
                                {
                                    player1HP = player1HP - damageValue; // player 1 loses "damageValue" amount of health
                                    player2AP = player2AP - 6; // player 2 loses 6 action points

                                    placeHolder = whosTurn;
                                    whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                                    whoWaits = placeHolder;

                                    // post result in chat v - (for example:- "user 1 [text]'d user 2 and did [damageValue] damage")
                                    await ReplyAsync(Context.User.Mention + " critically hit " + player1 + " and did `" + damageValue + " damage`" + text + "\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");

                                    if (player1HP <= 0) // if player 1 has 0 or less hp (is dead)
                                    {
                                        await ReplyAsync(Context.User.Mention + " has defeated " + player1 + "!\n\n" + player2 + " wins!"); // post death message - (for example:- "user1 defeated user2, user2 wins!")
                                        fight_Status = "nofight"; // reset status to no fight ongoing.
                                        player1HP = 100;
                                        player2HP = 100;
                                        player1AP = 20;
                                        player2AP = 20; // reset hp,ap, potions
                                        player1Hpot = 2;
                                        player1Apot = 2;
                                        player2Hpot = 2;
                                        player2Apot = 2;
                                        if (Context.Channel.Name == "worldstar") // secondary check for channel as we are about to PERMANENTLY delete a text channel
                                        {
                                            await Context.Channel.SendMessageAsync("`#worldstar channel will be deleted in 10 seconds.`"); // notify user of deletion timer - gives users time to see ending results, etc.
                                            await Task.Delay(10000); // delay deletion task by 10 seconds
                                            await (Context.Channel as IGuildChannel).DeleteAsync(); // delete WORLDSTAR text channel
                                        }
                                    }
                                }
                                else if (player2AP < 6) // if player 2 has less than 6 action points
                                {
                                    await ReplyAsync(Context.User.Mention + " You do not have enough Action Points for this action!\n\n" + whosTurn + " your turn!"); // notify them they must have at least 6 action points
                                }
                                else if (player2AP <= 0) // if player 2 has 0 or less AP
                                {
                                    await ReplyAsync(Context.User.Mention + " You have no Action Points left! Use an Action potion!\n\n" + whosTurn + " your turn!"); // notify them they should drink an action point potion
                                }
                            }
                            else if (Context.User.Mention == player1) // Player 1's turn
                            {
                                if (player2HP > 0 && player1AP >= 6) // and player 1 has at least 6 AP and player 2 has more than 0 hp
                                {
                                    player2HP = player2HP - damageValue; // player 2 loses "damageValue" amount of health
                                    player1AP = player1AP - 6; // player 1 loses 6 action points

                                    placeHolder = whosTurn;
                                    whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                                    whoWaits = placeHolder;

                                    // post result in chat v - (for example:- "user 1 [text]'d user 2 and did [damageValue] damage")
                                    await ReplyAsync(Context.User.Mention + " critically hit " + player2 + " and did `" + damageValue + " damage`" + text + "\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");

                                    if (player2HP <= 0) // if player 2 has 0 or less hp (is dead)
                                    {
                                        await ReplyAsync(Context.User.Mention + " has defeated " + player2 + "!\n\n" + player1 + " wins!"); // post death message - (for example:- "user1 defeated user2, user2 wins!")
                                        fight_Status = "nofight"; // reset status to no fight ongoing.
                                        player1HP = 100;
                                        player2HP = 100;
                                        player1AP = 20;
                                        player2AP = 20;
                                        player1Hpot = 2; // reset hp,ap, potions
                                        player1Apot = 2;
                                        player2Hpot = 2;
                                        player2Apot = 2;
                                        if (Context.Channel.Name == "worldstar") // secondary check for channel as we are about to PERMANENTLY delete a text channel
                                        {
                                            await Context.Channel.SendMessageAsync("`#worldstar channel will be deleted in 10 seconds.`"); // notify user of deletion timer - gives users time to see ending results, etc.
                                            await Task.Delay(10000); // delay deletion task by 10 seconds
                                            await (Context.Channel as IGuildChannel).DeleteAsync(); // delete WORLDSTAR text channel
                                        }
                                    }
                                }
                                else if (player1AP < 6) // if player 1 has less than 6 action points
                                {
                                    await ReplyAsync(Context.User.Mention + " You do not have enough Action Points for this action!\n\n" + whosTurn + " your turn!"); // notify them they must have at least 6 action points
                                }
                                else if (player1AP <= 0) // if player 1 has 0 or less AP
                                {
                                    await ReplyAsync(Context.User.Mention + " You have no Action Points left! Use an Action potion!\n\n" + whosTurn + " your turn!"); // notify them they should drink an action point potion
                                }
                            }
                            else // if it's somehow neither players turn
                            {
                                await ReplyAsync("Sorry it seems like something went wrong. Please type `!run`"); // notify user that an error has occured and suggest ceasing the fight
                            }
                        }
                        else // if "chanceToHit" is equal to 1
                        { // pass the turn of the player who missed
                            placeHolder = whosTurn;
                            whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                            whoWaits = placeHolder;

                            await ReplyAsync(Context.User.Mention + " you missed!\n" + whosTurn + " your turn!"); // notify them that they missed their action
                        }
                    }
                    else // if user tries to use this command when it isnt their turn
                    {
                        await ReplyAsync(Context.User.Mention + " it is not your turn."); // notify them that it is not their turn
                    }
                }
                else // if user tries to use this command when fight status is set to "nofight"
                {
                    await ReplyAsync("There is no fight at the moment."); // notify user there is no ongoing fight
                }
            }
            else // if fight commands not posted in "WORLDSTAR" channel
            {
                await Context.Channel.SendMessageAsync("Please only type fight commands in the `#worldstar` channel"); // notify user to only type commands in the "WORLDSTAR" channel
            }
        }
        [Command("ap")] // command declaration
        [Alias("AP", "action", "Action")] // command aliases (also trigger task)
        [Summary("Drink an Action Potion :zap:")] // command summary
        public async Task ActionRefill() // command async task (method basically)
        {
            if (Context.Channel.Name == "worldstar") // check if bot is posting/focused on the right channel (worldstar)
            {
                if (fight_Status == "fight_proceed") // if fight ongoing
                {
                    if (whosTurn == Context.User.Mention) // last mentioned players turn - (as in, the bottom text saying @user your turn!) - see screen layouts under "Design" heading for an example
                    {
                        int randomMultiplier = StaticRandom.Instance.Next(5, 10); // get random number between 5 to 10 and store as integer "randomMultiplier"
                        int restoreValue = randomMultiplier * 2; // integer "restoreValue" equals "randomMultiplier" times 2 - results in a range of 10 - 20 potential restoration

                        if (Context.User.Mention != player1) // if player 2's turn
                        {
                            player2Apot = player2Apot - 1; // take away 1 of player2's action potions
                            player2AP = player2AP + restoreValue; // player2 gains "restoreValue" amount of action points

                            placeHolder = whosTurn;
                            whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                            whoWaits = placeHolder;

                            if (player2Apot == 0) // if player2 action potion count equals 0
                            {// notify them they used their last action potion
                                await ReplyAsync(Context.User.Mention + " Just used their last Action Potion and gained `" + restoreValue + " Action Points!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");
                            }
                            else // if player2 action potion count is 1 or more
                            {// notify them they have drank an action potion
                                await ReplyAsync(Context.User.Mention + " Drank an Action potion and gained `" + restoreValue + " Action Points!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");
                            }
                            if (player2Apot < 0) // if player2 action potion count is less than 0 (they have none left)
                            {// notify them they have no action potions left
                                await ReplyAsync(Context.User.Mention + " You have no Action potions left!\n\n" + whosTurn + " your turn!");
                            }
                        }
                        else if (Context.User.Mention == player1) // if player 1's turn
                        {
                            player1Apot = player1Apot - 1; // take away 1 of player1's action potions
                            player1AP = player1AP + restoreValue; // player1 gains "restoreValue" amount of action points

                            placeHolder = whosTurn;
                            whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                            whoWaits = placeHolder;

                            if (player1Apot == 0) // if player1 action potion count equals 0
                            {// notify them they used their last action potion
                                await ReplyAsync(Context.User.Mention + " Just used their last Action Potion and gained `" + restoreValue + " Action Points!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");
                            }
                            else // if player1 action potion count is 1 or more
                            {// notify them they have drank an action potion
                                await ReplyAsync(Context.User.Mention + " Drank an Action potion and gained `" + restoreValue + " Action Points!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");
                            }
                            if (player1Apot < 0) // if player1 action potion count is less than 0 (they have none left)
                            {// notify them they have no action potions left
                                await ReplyAsync(Context.User.Mention + " You have no Action potions left!\n\n" + whosTurn + " your turn!");
                            }
                        }
                        else // if it's somehow neither players turn
                        {
                            await ReplyAsync("Sorry it seems like something went wrong. Please type `!run`"); // notify user that an error has occured and suggest ceasing the fight
                        }
                    }
                    else // if user tries to use this command when it isnt their turn
                    {
                        await ReplyAsync(Context.User.Mention + " it is not your turn."); // notify them that it is not their turn
                    }
                }
                else // if user tries to use this command when fight status is set to "nofight"
                {
                    await ReplyAsync("There is no fight at the moment."); // notify user there is no ongoing fight
                }
            }
            else // if fight commands not posted in "WORLDSTAR" channel
            {
                await Context.Channel.SendMessageAsync("Please only type fight commands in the `#worldstar` channel"); // notify user to only type commands in the "WORLDSTAR" channel
            }
        }
        [Command("hp")] // command declaration
        [Alias("HP", "health", "Health")] // command aliases (also trigger task)
        [Summary("Drink a Health potion :heart:")] // command summary
        public async Task HealthRefill() // command async task (method basically)
        {
            if (Context.Channel.Name == "worldstar") // check if bot is posting/focused on the right channel (worldstar)
            {
                if (fight_Status == "fight_proceed") // if fight ongoing
                {
                    if (whosTurn == Context.User.Mention) // last mentioned players turn - (as in, the bottom text saying @user your turn!) - see screen layouts under "Design" heading for an example
                    {
                        int randomMultiplier = StaticRandom.Instance.Next(5, 20); // get random number between 5 to 20 and store as integer "randomMultiplier"
                        int restoreValue = randomMultiplier * 2; // integer "restoreValue" equals "randomMultiplier" times 2 - results in a range of 10 - 40 potential restoration

                        if (Context.User.Mention != player1) // if player 2's turn
                        {
                            player2Hpot = player2Hpot - 1; // take away 1 of player2's health potions
                            player2HP = player2HP + restoreValue; // player2 gains "restoreValue" amount of health points

                            placeHolder = whosTurn;
                            whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                            whoWaits = placeHolder;

                            if (player2Hpot == 0) // if player2 action potion count equals 0
                            {// notify them they used their last action potion
                                await ReplyAsync(Context.User.Mention + " Just used their last Health Potion and restored `" + restoreValue + " Health Points!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");
                            }
                            else // if player2 action potion count is 1 or more
                            {// notify them they have drank an action potion
                                await ReplyAsync(Context.User.Mention + " Drank a Health potion and restored `" + restoreValue + " Health Points!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");
                            }
                            if (player2Hpot < 0) // if player2 action potion count is less than 0 (they have none left)
                            {// notify them they have no action potions left
                                await ReplyAsync(Context.User.Mention + " You have no Health potions left!\n\n" + whosTurn + " your turn!");
                            }
                        }
                        else if (Context.User.Mention == player1) // if player 1's turn
                        {
                            player1Hpot = player1Hpot - 1; // take away 1 of player1's action potions
                            player1HP = player1HP + restoreValue; // player1 gains "restoreValue" amount of action points

                            placeHolder = whosTurn;
                            whosTurn = whoWaits; // workout whos turn it is, setting the previous to placeholder - essentially swapping them around
                            whoWaits = placeHolder;
                            if (player1Hpot == 0) // if player1 action potion count equals 0
                            {// notify them they used their last action potion
                                await ReplyAsync(Context.User.Mention + " Just used their last Health Potion and restored `" + restoreValue + " Health Points!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");
                            }
                            else // if player1 action potion count is 1 or more
                            {
                                await ReplyAsync(Context.User.Mention + " Drank a Health potion and restored `" + restoreValue + " Health Points!`\n\n" + player1 + " has " + player1HP + " health, " + player1AP + " action points, " + player1Hpot + " Health potion and " + player1Apot + " Action potion!\n" + player2 + " has " + player2HP + " health, " + player2AP + " action points, " + player2Hpot + " Health potion and " + player2Apot + " Action potion!\n\n" + whosTurn + " your turn!");
                            }
                            if (player1Hpot < 0) // if player1 action potion count is less than 0 (they have none left)
                            {// notify them they have no action potions left
                                await ReplyAsync(Context.User.Mention + " You have no Health potions left!\n\n" + whosTurn + " your turn!");
                            }
                        }
                        else // if it's somehow neither players turn
                        {
                            await ReplyAsync("Sorry it seems like something went wrong. Please type `!run`"); // notify user that an error has occured and suggest ceasing the fight
                        }
                    }
                    else // if user tries to use this command when it isnt their turn
                    {
                        await ReplyAsync(Context.User.Mention + " it is not your turn."); // notify them that it is not their turn
                    }
                }
                else // if user tries to use this command when fight status is set to "nofight"
                {
                    await ReplyAsync("There is no fight at the moment."); // notify user there is no ongoing fight
                }
            }
            else // if fight commands not posted in "WORLDSTAR" channel
            {
                await Context.Channel.SendMessageAsync("Please only type fight commands in the `#worldstar` channel"); // notify user to only type commands in the "WORLDSTAR" channel
            }
        }
    }
}
