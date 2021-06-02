using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnkiTgBot.Command
{
    public abstract class Command
    {
        public abstract string Name { get; set; }
        public abstract TelegramBotClient Bot { get; set; }
        public abstract void Execute(Message message, TelegramBotClient client);

        //public bool Contains(string message)
        //{
        //    foreach (var mess in Names)
        //    {
        //        if (message.Contains(mess))
        //            return true;
        //    }
        //    return false;
        //}
    }
}
