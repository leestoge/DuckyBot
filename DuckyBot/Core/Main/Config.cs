using Newtonsoft.Json;
using System.IO;

namespace DuckyBot.Core.Main
{
    class Config /* CODE PROVIDED BY PETER/SPELOS - https://youtu.be/i4qkIkaF7Yk */
    {
        private const string configFolder = "Resources"; // folder storing config file
        private const string configFile = "config.json"; // config file

        public static BotConfig bot; // instance of the defined bot config class

        static Config()
        {
            if (!Directory.Exists(configFolder))
                Directory.CreateDirectory(configFolder); //If the folder (Resources) doesn't exist then create it

            if (!File.Exists(configFolder + "/" + configFile)) // if the config file doesnt exist
            {
                bot = new BotConfig(); // create new instance of bot config
                string json = JsonConvert.SerializeObject(bot, Formatting.Indented); //json indented by default - ensures it's being written/read properly.
                File.WriteAllText(configFolder + "/" + configFile, json); // write all text into the json file
            }
            else // if the config file does exist
            {
                string json = File.ReadAllText(configFolder + "/" + configFile); // read from json
                bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
        }
    }
    public struct BotConfig
    {
        public string token; // default value for token - must be manually set within the json itself
        public string cmdPrefix; // default value for command prefix - must be manually set within the json itself
    }
}