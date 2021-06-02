using AnkiTgBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnkiTgBot.Command.Commands
{
    class ReiterationFavorite : Command
    {
        public override string Name { get; set; } = "/go_fav";
        public override TelegramBotClient Bot { get; set; }

        public override async void Execute(Message message, TelegramBotClient client)
        {
            Bot = client;

            var user = await CommandHelper.GetUserInfo(message);
            var decks = await CommandHelper.GetDeskByUser(user.Id);

            List<Card> list = new List<Card>();
            //var list = CommandHelper.SaveRest<Card>(cards);
            foreach (var desk in decks)
            {
                var cards = await CommandHelper.GetCardByDesk(desk.Id);
                foreach (var card in cards)
                {

                    if (card.Favorite)
                        list.Add(card);
                }
            }

            await Bot.SendTextMessageAsync(message.From.Id, await CommandHelper.Translate("Manage to remember what to be on the other side of the card before you see the answer", user));
            Thread.Sleep(5000);
            await Bot.SendTextMessageAsync(message.From.Id, "Ready");
            Thread.Sleep(500);
            await Bot.SendTextMessageAsync(message.From.Id, "Set");
            Thread.Sleep(500);
            await Bot.SendTextMessageAsync(message.From.Id, "Go");
            foreach (var card in list)
            {
                await Bot.SendTextMessageAsync(message.From.Id, $"🃏{await CommandHelper.Translate("Front side", user)}: \"{card.Front}\"");
                Thread.Sleep(2000);
                await Bot.SendTextMessageAsync(message.From.Id, "3️⃣");
                Thread.Sleep(1250);
                await Bot.SendTextMessageAsync(message.From.Id, "2️⃣");
                Thread.Sleep(1250);
                await Bot.SendTextMessageAsync(message.From.Id, "1️⃣");
                Thread.Sleep(1250);
                await Bot.SendTextMessageAsync(message.From.Id, $"🎴{await CommandHelper.Translate("Back side", user)}: \"{card.Back}\"");
                Thread.Sleep(2000);
                
            }
            //await Bot.SendTextMessageAsync(message.Chat.Id, $"Введи ID колоды, которую хочешь повторить");
        }
    }
}
