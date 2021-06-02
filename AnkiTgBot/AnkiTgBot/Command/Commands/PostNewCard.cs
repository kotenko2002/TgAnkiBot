using AnkiTgBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace AnkiTgBot.Command.Commands
{
    class PostNewCard : Command
    {
        public override string Name { get; set; } = "/add_card";
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
            await client.SendTextMessageAsync(message.Chat.Id, $"{await CommandHelper.Translate("Enter", user)} front_back_deckId {await CommandHelper.Translate("of card through a", user)}  '_'");
            Bot.OnMessage += GetString;
        }

        private async void GetString(object sender, MessageEventArgs e)
        {
            string Info = e.Message.Text;
            var splitedInfo = Info.Split("_");
            
            if (splitedInfo.Length != 3 )
                return;
            bool @bool = int.TryParse(splitedInfo[2], out int isd);
            if (!@bool)
                return;
            Card card = new Card(splitedInfo[0], splitedInfo[1], Convert.ToInt32(splitedInfo[2]));

            _ = CommandHelper.CreateCard(card, e.Message);
            var user = await CommandHelper.GetUserInfo(e.Message);
            await Bot.SendTextMessageAsync(e.Message.Chat.Id, $"{await CommandHelper.Translate("Card created", user)}");
            Bot.OnMessage -= GetString;
        }
    }
}
