using System.Threading.Tasks;

namespace DuckyBot.Core.Main
{
    public static class Program 
    {
        static async Task Main(string[] args) 
            => await new DuckyBotClient().InitializeAsync();
    }
}