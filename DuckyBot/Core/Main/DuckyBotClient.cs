using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DuckyBot.Core.Modules.Events.MessageReceived;
using DuckyBot.Core.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Victoria;

namespace DuckyBot.Core.Main
{
    class DuckyBotClient
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        public DuckyBotClient(DiscordSocketClient client = null, CommandService commands = null)
        {
            _client = client ?? new DiscordSocketClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                LogLevel = LogSeverity.Verbose,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                MessageCacheSize = 10 // cached messages stored {per channel} - allows me to see deleted messages etc
            });

            _commands = commands ?? new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = false, // CaSe SeNsItIvE
                DefaultRunMode = RunMode.Async, // makes all commands asynchronous
                LogLevel = LogSeverity.Verbose
            });
        }

        public async Task InitializeAsync()
        {
            if (string.IsNullOrEmpty(Config.Bot.Token))
            {
                Console.WriteLine("Bot token not found.");
                Console.ReadKey();
                return; // if bot token in config json file is an empty string or can't be found - cease task
            }           

            await _client.LoginAsync(TokenType.Bot, Config.Bot.Token); // logs the bot in with a bot token, and utilises the config json file to obtain the token
            await _client.StartAsync(); // start up bot
            _client.Log += LogAsync; // lets bot know where to post log entries

            _services = SetupServices();
            var cmdHandler = new CommandHandler(_client, _commands, _services);
            await cmdHandler.InitializeAsync();


            _client.Ready += Client_ready; // reaction handling and bot status handling
            _client.MessageDeleted += MessageDeleted; // showing deleted messages in secret :thinking:

            await _services.GetRequiredService<MusicService>().InitializeAsync();

            Global.Client = _client;
            await Task.Delay(-1).ConfigureAwait(false); // wait until the operation ends (never unless closed)
        }

        private async Task Client_ready()
        {
            await RepeatingTimer.StartTimer(); // start timer to change bot status message

            await _client.SetGameAsync("DuckyBot | !help"); // set the bots "Playing" status
            await _client.SetStatusAsync(UserStatus.Online); // set the bots online status

            // EVENT HANDLERS
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            _client.MessageReceived += General.Gay;
            _client.MessageReceived += General.JackFrags;
            _client.MessageReceived += General.HempusJibberish;
            _client.MessageReceived += Whiskers.GiveFeels;
            _client.MessageReceived += Ducky.FacebookOUT;
            _client.MessageReceived += Trucks.GiveDonut;
            _client.MessageReceived += Boon.GivePepega;
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            await ConsoleInput().ConfigureAwait(false);
        }

        private async Task ConsoleInput()
        {
            var input = string.Empty;
            while (input != null && input.Trim().ToLowerInvariant() != "!block")
            {
                input = Console.ReadLine();
                if (input != null && input.Trim().ToLowerInvariant() == "!message")
                {
                    await ConsoleSendMessage().ConfigureAwait(false);
                }
            }
        }

        private async Task ConsoleSendMessage()
        {
            Console.WriteLine("Select the guild:");
            var guild = GetSelectedGuild(_client.Guilds);
            var textChannel = GetSelectedTextChannel(guild.TextChannels);
            var msg = string.Empty;
            while (msg != null && msg.Trim() == string.Empty)
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
            if (msg.Author.IsBot)
            {
                return;
            }

            if (msg.EditedTimestamp == DateTime.Now.AddMinutes(-5))
            {
                return;
            }

            await Task.Delay(3000).ConfigureAwait(false);
            await channel.SendMessageAsync($"**[{DateTime.Now}]** **[DELETED MESSAGE]** {msg.Author.Username}: {msg.Content}");
        }

        private Task LogAsync(LogMessage msg)
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

            return Task.CompletedTask;
        }

        private IServiceProvider SetupServices()
            => new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton<LavaRestClient>()
                .AddSingleton<LavaSocketClient>()
                .AddSingleton<MusicService>()
                .BuildServiceProvider();
    }
}
