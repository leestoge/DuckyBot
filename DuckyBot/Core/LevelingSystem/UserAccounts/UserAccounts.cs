﻿using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;


namespace DuckyBot.Core.LevelingSystem.UserAccounts
{
    public static class UserAccounts /* CODE PROVIDED BY PETER/SPELOS - https://youtu.be/GpHFj9_aey0 */
    {
        private static List<UserAccount> accounts; //json is a list of account objects

        private static string accountsFile = "Resources/accounts.json"; // acounts json file directory

        static UserAccounts()
        {
            if (DataStorage.SaveExists(accountsFile)) // if accounts file already exists
            {
                accounts = DataStorage.LoadUserAccounts(accountsFile).ToList(); // load the accounts file
            }
            else // if it doesnt exist
            {
                accounts = new List<UserAccount>(); // create new empty list
                SaveAccounts(); // save account
            }
        }

        public static void SaveAccounts()
        {
            DataStorage.SaveUserAccounts(accounts, accountsFile); // save accounts to accounts json file
        }

        public static UserAccount GetAccount(SocketUser user) //returns user account with user parameter passed in
        {
            return GetOrCreateAccount(user.Id); // get user account from their id
        }

        private static UserAccount GetOrCreateAccount(ulong id) //returns user account with id parameter passed in
        {
            var result = from a in accounts // linq query? from each account in accounts
                         where a.ID == id // where id of account equal to parameter id
                         select a; // select account
            // should be unique as user ids are per user
            var account = result.FirstOrDefault(); // store account fromr result
            if (account == null) account = CreateUserAccount(id); //if account doesnt exist then create the account
            return account; // return the account
        }

        private static UserAccount CreateUserAccount(ulong id) //returns user account with id parameter passed in
        {
            var newAccount = new UserAccount() // create new account in the accounts list json file
            {
                ID = id, // set id to the passed in id
                XP = 0 // default value for XP is 0
            };
            accounts.Add(newAccount); // add newly made user to the accounts list json file
            SaveAccounts(); // save accounts list
            return newAccount; // return the new account
        }
    }
}