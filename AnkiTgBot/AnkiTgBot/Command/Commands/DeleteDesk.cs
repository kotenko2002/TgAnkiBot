using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnkiTgBot.Command.Commands
{
    class DeleteDesk : Command
    {
        public override string Name { get; set; } = "/del_deck";
        public override TelegramBotClient Bot { get; set; }

        public override async void Execute(Message message, TelegramBotClient client)
        {
            Bot = client;

            var user = await CommandHelper.GetUserInfo(message);
            var decks = await CommandHelper.GetDeskByUser(user.Id);

            string text = "";
            foreach (var desk in decks)
            {
                text += $"\n{desk.Name} - ID : {desk.Id} ";
            }
            await Bot.SendTextMessageAsync(message.Chat.Id, text);
            await Bot.SendTextMessageAsync(message.Chat.Id, await CommandHelper.Translate("Enter the deck id", user));
            Bot.OnMessage += GetString;
        }

        private async void GetString(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var id = e.Message.Text;
            await CommandHelper.DeleteDesk(Convert.ToInt32(id));
            var user = await CommandHelper.GetUserInfo(e.Message);
            await Bot.SendTextMessageAsync(e.Message.Chat.Id, await CommandHelper.Translate("Deck destroyed", user));
            Bot.OnMessage -= GetString;
        }
    }
}
