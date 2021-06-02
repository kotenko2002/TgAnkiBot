using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnkiTgBot.Command.Commands
{
    class DeleteCard : Command
    {
        public override string Name { get; set; } = "/del_card";
        public override TelegramBotClient Bot { get; set; }

        public override async void Execute(Message message, TelegramBotClient client)
        {
            Bot = client;

            var user = await CommandHelper.GetUserInfo(message);
            var decks = await CommandHelper.GetDeskByUser(user.Id);

            foreach (var desk in decks)
            {
                string text = "";
                await Bot.SendTextMessageAsync(message.Chat.Id, $"{await CommandHelper.Translate("Deck name", user)}: {desk.Name}");


                var cards = await CommandHelper.GetCardByDesk(desk.Id);
                foreach (var card in cards)
                {
                    text += $"\n{card.Front} : {card.Back} : {card.Id} : {card.Favorite}";
                }
                if (text == "")
                    continue;
                await Bot.SendTextMessageAsync(message.Chat.Id, text);
            }
            //await Bot.SendTextMessageAsync(message.Chat.Id, await CommandHelper.Translate("Enter the deck id", user));
            Bot.OnMessage += GetString;
        }

        private async void GetString(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var id = e.Message.Text;
            await CommandHelper.DeleteCard(Convert.ToInt32(id));
            var user = await CommandHelper.GetUserInfo(e.Message);
            await Bot.SendTextMessageAsync(e.Message.Chat.Id, await CommandHelper.Translate("Card destroyed", user));
            Bot.OnMessage -= GetString;
        }

    }
}
