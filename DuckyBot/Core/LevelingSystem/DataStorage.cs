using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using DuckyBot.Core.LevelingSystem.UserAccounts;

namespace DuckyBot.Core.LevelingSystem
{
    public static class DataStorage /* CODE PROVIDED BY PETER/SPELOS - https://youtu.be/GpHFj9_aey0 */
    {
        //save all user accounts
        public static void SaveUserAccounts(IEnumerable<UserAccount> accounts, string filePath) // parameters - collection of user account(s), where to save
        {
            var json = JsonConvert.SerializeObject(accounts, Formatting.Indented); // json indented by default - ensures it's being written/read properly.
            File.WriteAllText(filePath, json); // write all text into the json file
        }
        //get all user accounts
        public static IEnumerable<UserAccount> LoadUserAccounts(string filePath) // return collection of user accounts, pass in where to load from
        {
            if (!File.Exists(filePath)) return null; // if no file exists at the file path return null
            var json = File.ReadAllText(filePath); // read all text at the specified area to load from
            return JsonConvert.DeserializeObject<List<UserAccount>>(json); // return list of user account object
        }

        public static bool SaveExists(string filePath) // if save exists
        {
            return File.Exists(filePath); // return
        }
    }
}