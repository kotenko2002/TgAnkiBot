using AnkiTgBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnkiTgBot.Command.Commands
{
    class GetAllCardsByDesk : Command
    {
        public override string Name { get; set; } = "/view_all_cards";
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
        }
    }
}
