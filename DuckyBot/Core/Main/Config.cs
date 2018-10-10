using System;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;

namespace DuckyBot.Core.Main
{
    internal static class Config /* CODE PROVIDED BY PETER/SPELOS - https://youtu.be/i4qkIkaF7Yk */
    {
        private const string ConfigFolder = "Resources"; // folder storing config file
        private const string ConfigFile = "config.json"; // config file

        public static readonly BotConfig Bot; // instance of the defined bot config class

        static Config()
        {
            if (!Directory.Exists(ConfigFolder))
            {
                Directory.CreateDirectory(ConfigFolder); // If the folder (Resources) doesn't exist then create it
            }

            if (!File.Exists(ConfigFolder + "/" + ConfigFile)) // if the config file doesnt exist
            {
                Bot = new BotConfig(); // create new instance of bot config
                var json = JsonConvert.SerializeObject(Bot, Formatting.Indented); //json indented by default - ensures it's being written/read properly.
                File.WriteAllText(ConfigFolder + "/" + ConfigFile, json); // write all text into the json file
            }
            else // if the config file does exist
            {
                var json = File.ReadAllText(ConfigFolder + "/" + ConfigFile); // read from json
                Bot = JsonConvert.DeserializeObject<BotConfig>(json);
            }
        }
    }

    public struct BotConfig : IEquatable<BotConfig>
    {
        public string Token; // default value for token - must be manually set within the json itself
        public string CmdPrefix; // default value for command prefix - must be manually set within the json itself

        public override bool Equals(object obj)
        {
            return obj is BotConfig config && Equals(config);
        }

        public bool Equals(BotConfig other)
        {
            return Token == other.Token &&
                   CmdPrefix == other.CmdPrefix;
        }

        public override int GetHashCode()
        {
            var hashCode = 54683585;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Token);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(CmdPrefix);
            return hashCode;
        }
    }
}