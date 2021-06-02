using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnkiTgBot.Command.Commands
{
    class GetAllFavoriteCards : Command
    {
        public override string Name { get; set; } = "/view_all_fav";
        public override TelegramBotClient Bot { get; set; }

        public override async void Execute(Message message, TelegramBotClient client)
        {
            Bot = client;

            var user = await CommandHelper.GetUserInfo(message);
            var decks = await CommandHelper.GetDeskByUser(user.Id);

            await Bot.SendTextMessageAsync(message.Chat.Id, await CommandHelper.Translate("Favorite cards", user));
            string text = "";
            foreach (var desk in decks)
            {
                var cards = await CommandHelper.GetCardByDesk(desk.Id);
                foreach (var card in cards)
                {
                    
                    if(card.Favorite)
                        text += $"\n{card.Front} : {card.Back} : {card.Id} ";
                    
                }
            }
            if(text != "")
                await Bot.SendTextMessageAsync(message.Chat.Id, text);
        }
    }
}
