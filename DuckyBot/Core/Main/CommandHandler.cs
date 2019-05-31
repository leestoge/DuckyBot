using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using DuckyBot.Core.LevelingSystem;

namespace DuckyBot.Core.Main
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;

        public CommandHandler(DiscordSocketClient client, CommandService commands, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
        }

        public async Task InitializeAsync()
        {
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            _commands.Log += LogAsync;
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage msg)
        {
            var argPos = 0;

            if (msg.Author.IsBot)
            {
                return;
            }

            var userMessage = msg as SocketUserMessage;

            if (userMessage is null)
            {
                return;
            }

            if(!userMessage.HasStringPrefix("!", ref argPos) || userMessage.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                return;
            }

            var context = new SocketCommandContext(_client, userMessage);
            var result = await _commands.ExecuteAsync(context, argPos, _services);

            if (!result.IsSuccess && result.Error != CommandError.UnknownCommand) // If not successful, reply with an error. - Filters out unknown command error as mis-typed commands happen frequently.
            {
                await context.Channel.SendMessageAsync(result.ErrorReason); // Tell user an error occured
                Console.WriteLine($"[{DateTime.UtcNow:t} [Commands] {context.Message.Author.Username}: {context.Message.Content} | Error: {result.ErrorReason}");
                await Task.Delay(1500).ConfigureAwait(false);

                var application = await context.Client.GetApplicationInfoAsync(); // gets channels from discord client
                var ownerDM = await application.Owner.GetOrCreateDMChannelAsync(); // find dm channel to private message me
                await ownerDM.SendMessageAsync($"[{DateTime.UtcNow:t} [Commands] {context.Message.Author.Username}: {context.Message.Content} | Error: {result.ErrorReason}"); // private message me with exact error reason
            }

            // Levelling up related
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // Execute the UserSentMessage task within the leveling class - passing in the user who typed the message and the channel they typed it in
            await Leveling.UserSentMessage((SocketGuildUser)context.User, (SocketTextChannel)context.Channel); /* CODE PROVIDED BY PETER/SPELOS - https://youtu.be/GpHFj9_aey0 */
            //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
            // Levelling up end
        }

        private Task LogAsync(Discord.LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}
