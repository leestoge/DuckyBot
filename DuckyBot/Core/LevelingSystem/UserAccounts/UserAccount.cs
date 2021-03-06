﻿using System;

namespace DuckyBot.Core.LevelingSystem.UserAccounts
{
    public class UserAccount /* CODE PROVIDED BY PETER/SPELOS - https://youtu.be/GpHFj9_aey0 */
    {
        public ulong ID { get; set; } // unsigned integer (doesnt go below 0) getter/setter for user id
        // user id is the same constant group of numbers no matter what server or channel the user is in.
        public uint XP { get; set; } // unsigned integer (doesnt go below 0) getter/setter for user XP
        // XP is the same throughout all servers the bot can collect it in. - user will gain XP in ANY server/channel the bot is also in.

        public uint LevelNumber // unsigned integer (doesn't go below 0)
            => (uint)Math.Sqrt((double)XP / 200);
    }
}