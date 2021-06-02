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
    class PostNewDesk : Command
    {
        public override string Name { get; set; } = "/add_deck";
        public override TelegramBotClient Bot { get; set; }

        public override async void Execute(Message message, TelegramBotClient client)
        {
            //проверка
            Bot = client;
            var user = await CommandHelper.GetUserInfo(message);
            await client.SendTextMessageAsync(message.Chat.Id, await CommandHelper.Translate("Now use this command", user));
            Bot.OnMessage += GetString;
        }

        private async void GetString(object sender, MessageEventArgs e)
        {
            var userInfo = await CommandHelper.GetUserInfo(e.Message);
            Desk desk = new Desk() { UserId = userInfo.Id, Name = e.Message.Text };
            _ = CreateDesk(desk, e.Message);
            Bot.OnMessage -= GetString;
        }
        private async Task CreateDesk(Desk desk, Message message)
        {
            var JSON = JsonConvert.SerializeObject(desk);
            var data = new StringContent(JSON, Encoding.UTF8, "application/json");

            var response = await ApiSetting.ApiClient.PostAsync($"{ApiSetting.Base}api/desks", data);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Succesfully saved");
                var user = await CommandHelper.GetUserInfo(message);
                await Bot.SendTextMessageAsync(message.Chat.Id, await CommandHelper.Translate("Deck created", user));
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }
}
