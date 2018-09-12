using Discord.WebSocket;

namespace DuckyBot.Core.Main
{
    internal static class Global
    {
        internal static DiscordSocketClient Client { get; set; }
        internal static ulong MessageIdToTrack { get; set; }
    }
}
