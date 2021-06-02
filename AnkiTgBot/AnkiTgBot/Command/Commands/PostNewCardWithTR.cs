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
    class PostNewCardWithTR : Command
    {
        public override string Name { get; set; } = "/add_cardTR";
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

            await client.SendTextMessageAsync(message.Chat.Id, $"{await CommandHelper.Translate("Enter", user)} front_deckId {await CommandHelper.Translate("of card through a", user)}  '_'");
            Bot.OnMessage += GetString;
        }

        private async void GetString(object sender, MessageEventArgs e)
        {
            string back;
            string Info = e.Message.Text;
            var splitedInfo = Info.Split("_");
            if (splitedInfo.Length != 2)
                return;
            bool @bool = int.TryParse(splitedInfo[1], out int isd);
            if (!@bool)
                return;
            var userr = await CommandHelper.GetUserInfo(e.Message);
            //string back = await CommandHelper.Translate(splitedInfo[0],userr);
            //var user = await CommandHelper.GetUserInfo(e.Message.Chat.Id);
            using (HttpResponseMessage response = await ApiSetting.ApiClient.GetAsync($"{ApiSetting.Base}api/translate?text={splitedInfo[0]}&toLang={userr.Lang}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    Translation result = JsonConvert.DeserializeObject<Translation>(content);

                    back = result.Data.Translation;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            Card card = new Card(splitedInfo[0], back, Convert.ToInt32(splitedInfo[1]));
            Console.WriteLine($"{card.Front} {card.Back} {card.DeskId}");

            _ = CommandHelper.CreateCard(card, e.Message);

            await Bot.SendTextMessageAsync(e.Message.Chat.Id, $"{await CommandHelper.Translate("Card created", userr)}");

            Bot.OnMessage -= GetString;

        }
    }
}
