using AnkiTgBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;


namespace AnkiTgBot.Command.Commands
{
    class Reiteration : Command
    {
        public override string Name { get; set; } = "/go";
        public override TelegramBotClient Bot { get; set; }


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



        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var cards = await CommandHelper.GetCardByDesk(Convert.ToInt32(e.Message.Text));

            var user = await CommandHelper.GetUserInfo(e.Message);
            await Bot.SendTextMessageAsync(e.Message.Chat.Id, await CommandHelper.Translate("Manage to remember what to be on the other side of the card before you see the answer", user));
            Thread.Sleep(5000);
            await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Ready");
            Thread.Sleep(500);
            await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Set");
            Thread.Sleep(500);
            await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Go");
            foreach (var card in cards)
            {
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, $"🃏{await CommandHelper.Translate("Front side", user)}: \"{card.Front}\"");
                Thread.Sleep(2000);
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "3️⃣");
                Thread.Sleep(1250);
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "2️⃣");
                Thread.Sleep(1250);
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "1️⃣");
                Thread.Sleep(1250);
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, $"🎴{await CommandHelper.Translate("Back side", user)}: \"{card.Back}\"");
                Thread.Sleep(2000);
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, $" 👇");
            }

            //var list = CommandHelper.SaveRest<Card>(cards);
            
            Bot.OnMessage -= Bot_OnMessage;
        }

        
    }
}
