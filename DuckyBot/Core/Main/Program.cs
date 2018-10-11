using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using DuckyBot.Core.Modules.Events.MessageReceived;
using Discord.Commands;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using DuckyBot.Core.LevelingSystem;
using DuckyBot.Core.Utilities;

namespace DuckyBot.Core.Main
{
    public class Program /* CODE PROVIDED BY PETER/SPELOS (Did make edits of my own though) - https://youtu.be/i4qkIkaF7Yk */
    {
        private DiscordSocketClient _client; // discord client
        private CommandService _commands;

        private static void Main()
        => new Program().StartAsync().GetAwaiter().GetResult(); /* CODE PROVIDED BY PETER/SPELOS - https://youtu.be/i4qkIkaF7Yk */

        public async Task StartAsync() // Works as main as Discord operates asynchronously
        {
            if (string.IsNullOrEmpty(Config.Bot.Token))
            {
                Console.WriteLine("Bot token not found.");
                Console.ReadKey();
                return; // if bot token in config json file is an empty string or can't be found - cease task
            }
         
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose, // Verbose = Get as much information as possible in console log
                DefaultRetryMode = RetryMode.AlwaysRetry, // not really sure?
                MessageCacheSize = 10 // cached messages stored {per channel} - allows me to see deleted messages etc
            });

            _commands = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false, // CaSe SeNsItIvE
                DefaultRunMode = RunMode.Async, // run-mode asynchronous
                LogLevel = LogSeverity.Debug               
            });

            _client.MessageReceived += HandleCommand; // Command handler

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
         
            _client.Ready += Client_ready;

            _client.MessageUpdated += MessageEdited;

            _client.MessageDeleted += MessageDeleted;

            _client.Log += Log; // lets bot know where to post log entries

            await _client.LoginAsync(TokenType.Bot, Config.Bot.Token); // logs the bot in with a bot token, and utilises the config json file to obtain the token
            await _client.StartAsync(); // start up bot

            Global.Client = _client;
            await Task.Delay(-1).ConfigureAwait(false); // wait until the operation ends (never unless closed)
        }

        private async Task HandleCommand(SocketMessage messageParameter)
        {
            var message = messageParameter as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);

            if (context.Message == null || context.Message.Content == "")
            {
                return;
            }

            if (context.User.IsBot)
            {
                return;
            }

            var argPos = 0;

            if (!(message.HasStringPrefix("!", ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
            {
                return;
            }

            var result = await _commands.ExecuteAsync(context, argPos);

            if (!result.IsSuccess && result.Error != CommandError.UnknownCommand) // If not successful, reply with an error. - Filters out unknown command error as mis-typed commands happen frequently.
            {
                //await context.Channel.SendMessageAsync("Something has went wrong! Stoge has been notified."); // Tell user an error occured
                Console.WriteLine($"[{DateTime.UtcNow:t} [Commands] {context.Message.Author.Username}: {context.Message.Content} | Error: {result.ErrorReason}");
                var application = await context.Client.GetApplicationInfoAsync(); // gets channels from discord client
                var z = await application.Owner.GetOrCreateDMChannelAsync(); // find dm channel to private message me
                await z.SendMessageAsync($"[{DateTime.UtcNow:t} [Commands] {context.Message.Author.Username}: {context.Message.Content} | Error: {result.ErrorReason}"); // private message me with exact error reason
            }

            // Leveling up related
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            await Leveling.UserSentMessage((SocketGuildUser)context.User, (SocketTextChannel)context.Channel); /* CODE PROVIDED BY PETER/SPELOS - https://youtu.be/GpHFj9_aey0 */
            // Execute the UserSentMessage task within the leveling class - passing in the user who typed the message and the channel they typed it in

            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // Leveling up end
        }

        private async Task Client_ready()
        {
            await RepeatingTimer.StartTimer(); // hydrate reminder every 30min

            await _client.SetGameAsync("DuckyBot | !help"); // set the bots "Playing" status
            await _client.SetStatusAsync(UserStatus.Online); // set the bots online status

            //CUSTOM EVENT HANDLERS
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            _client.MessageReceived += General.Gay;
            _client.MessageReceived += General.Cummo;
            _client.MessageReceived += General.WhoDidThis;
            _client.MessageReceived += General.HempusJibberish;
            _client.MessageReceived += General.GoodBot;
            _client.MessageReceived += General.BadBot;
            _client.MessageReceived += Whiskers.GiveFeels;
            _client.MessageReceived += Ducky.GiveKisses;
            _client.MessageReceived += Trucks.GiveDonut;
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            ConsoleInput();
        }

        private void ConsoleInput()
        {
            var input = string.Empty;
            while (input.Trim().ToLowerInvariant() != "!block")
            {
                input = Console.ReadLine();
                if (input.Trim().ToLowerInvariant() == "!message")
                {
                    ConsoleSendMessage();
                }
            }
        }

        private async void ConsoleSendMessage()
        {
            Console.WriteLine("Select the guild:");
            var guild = GetSelectedGuild(_client.Guilds);
            var textChannel = GetSelectedTextChannel(guild.TextChannels);
            var msg = string.Empty;
            while (msg.Trim() == string.Empty)
            {
                Console.WriteLine("Your message:");
                msg = Console.ReadLine();
            }
            await textChannel.SendMessageAsync(msg);
        }

        private static SocketTextChannel GetSelectedTextChannel(IEnumerable<SocketTextChannel> channels)
        {
            var textChannels = channels.ToList();
            var maxIndex = textChannels.Count - 1;
            for (var i = 0; i <= maxIndex; i++)
            {
                Console.WriteLine($"{i} - {textChannels[i].Name}");
            }

            var selectedIndex = -1;
            while (selectedIndex < 0 || selectedIndex > maxIndex)
            {
                var success = int.TryParse(Console.ReadLine().Trim(), out selectedIndex);
                if (!success)
                {
                    Console.WriteLine("That was an invalid index, try again.");
                    selectedIndex = -1;
                }
            }
            return textChannels[selectedIndex];
        }

        private static SocketGuild GetSelectedGuild(IEnumerable<SocketGuild> guilds)
        {
            var socketGuilds = guilds.ToList();
            var maxIndex = socketGuilds.Count - 1;
            for (var i = 0; i <= maxIndex; i++)
            {
                Console.WriteLine($"{i} - {socketGuilds[i].Name}");
            }

            var selectedIndex = -1;
            while (selectedIndex < 0 || selectedIndex > maxIndex)
            {
                var success = int.TryParse(Console.ReadLine().Trim(), out selectedIndex);
                if (!success)
                {
                    Console.WriteLine("That was an invalid index, try again.");
                    selectedIndex = -1;
                }
            }

            return socketGuilds[selectedIndex];
        }

        private static async Task MessageDeleted(Cacheable<IMessage, ulong> msgCache, ISocketMessageChannel ContextChannel)
        {
            var msg = await msgCache.GetOrDownloadAsync();
            var channel = Global.Client.GetGuild(307712604904620034).GetTextChannel(475222498175352834);
            if (msg.Author.Username == "DuckyBot")
            {
                return;
            }

            if (msg.EditedTimestamp == DateTime.Now.AddMinutes(-5))
            {
                return;
            }

            await channel.SendMessageAsync($"**[{DateTime.Now}]** **[DELETED MESSAGE]** {msg.Author.Username}: {msg.Content}");
        }

        private static async Task MessageEdited(Cacheable<IMessage, ulong> cachedMsgBeforeUpdate, SocketMessage editedMsg, ISocketMessageChannel contextChannel)
        {
            var msgBeforeUpdate = await cachedMsgBeforeUpdate.GetOrDownloadAsync();
            var channel = Global.Client.GetGuild(307712604904620034).GetTextChannel(475222498175352834);
            if (msgBeforeUpdate.Author.Username == "DuckyBot")
            {
                return;
            }

            if (msgBeforeUpdate.Content == "https://" || editedMsg.Content == "https://")
            {
                return;
            }
            await channel.SendMessageAsync($"**[{DateTime.Now}]** **[EDITED MESSAGE]** {msgBeforeUpdate.Author.Username}: {msgBeforeUpdate.Content} **->** {msgBeforeUpdate.Author.Username}: {editedMsg.Content}");
        }       

        private static async Task Log(LogMessage msg) //log message argument
        {
            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow; // console customization depending on message type, actual errors are red. etc.
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Console.Title = "DuckyBot Log";
            Console.WriteLine($"{DateTime.Now:t} [{msg.Severity}] {msg.Source}: {msg.Message}"); // time, color, author, message
        }
    }
}