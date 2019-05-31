using System.Threading.Tasks;

namespace DuckyBot.Core.Main
{
    public class Program 
    {
        static async Task Main(string[] args) 
            => await new DuckyBotClient().InitializeAsync();
    }
}