using AnkiTgBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using System.Threading;

namespace AnkiTgBot.Command.Commands
{
    class Game : Command
    {
        public override string Name { get; set; } = "/play";
        public override TelegramBotClient Bot { get; set; }
        //IEnumerable<Card> cards{ get; set; }

    public override async void Execute(Message message, TelegramBotClient client)
        {
            Bot = client;
            Bot.OnMessage += Bot_OnMessage;
            var user = await CommandHelper.GetUserInfo(message);
            var decks = await CommandHelper.GetDeskByUser(user.Id);

            string text = "";
            foreach (var desk in decks)
            {
                text += $"\n{desk.Name} - ID : {desk.Id} ";
            }
            await Bot.SendTextMessageAsync(message.Chat.Id, text);

            await Bot.SendTextMessageAsync(message.Chat.Id, $"Введи ID колоды, которую хочешь повторить");
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var cards = await CommandHelper.GetCardByDesk(Convert.ToInt32(e.Message.Text));
            bool @bool = true;
            foreach (var card in cards)
            {
                if (@bool)
                {
                    await Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Игрок 1s");
                    @bool = false;
                }
                else
                {
                    await Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Игрок 2");
                    @bool = true;
                }

                await Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Передняя сторона: \"{card.Front}\"");
                Thread.Sleep(2000);
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "3");
                Thread.Sleep(1000);
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "2");
                Thread.Sleep(1000);
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "1");
                Thread.Sleep(1000);
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, $"Задняя сторона: \"{card.Back}\"");
                Thread.Sleep(2000);
            }

            //var list = CommandHelper.SaveRest<Card>(cards);

            Bot.OnMessage -= Bot_OnMessage;
        }
    }
}
