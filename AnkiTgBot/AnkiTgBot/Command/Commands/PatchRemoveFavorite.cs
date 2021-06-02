using AnkiTgBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace AnkiTgBot.Command.Commands
{
    class PatchRemoveFavorite : Command
    {
        public override string Name { get; set; } = "/remove_from_fav";
        public override TelegramBotClient Bot { get; set; }

        public override async void Execute(Message message, TelegramBotClient client)
        {
            Bot = client;

            var user = await CommandHelper.GetUserInfo(message);
            var decks = await CommandHelper.GetDeskByUser(user.Id);


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
            await Bot.SendTextMessageAsync(message.Chat.Id, text);
            await Bot.SendTextMessageAsync(message.Chat.Id, await CommandHelper.Translate("Enter the number of the card you want to remove from favorites", user));
            Bot.OnMessage += GetString;
        }
        private async void GetString(object sender, MessageEventArgs e)
        {
            string id = e.Message.Text;
            var card = await CommandHelper.GetCardById(Convert.ToInt32(id));
            Console.WriteLine("Нашел");
            _ = CommandHelper.DeleteCard(Convert.ToInt32(id));
            Console.WriteLine("Удалил");
            Card card1 = new Card(card.Front, card.Front, card.DeskId);
            card1.Favorite = false;
            _ = CommandHelper.CreateCard(card1, e.Message);
            Console.WriteLine("создал");
            var user = await CommandHelper.GetUserInfo(e.Message);
            await Bot.SendTextMessageAsync(e.Message.Chat.Id, $"{await CommandHelper.Translate("Card removed from favorite", user)}");
            Bot.OnMessage -= GetString;

        }
    }
}
