using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnkiTgBot.Command.Commands
{
    class Start : Command
    {
        public override string Name { get; set; } = "/start";
        public override TelegramBotClient Bot { get; set; }

        public override async void Execute(Message message, TelegramBotClient client)
        {
            Bot = client;

            var result = await ApiSetting.ApiClient.GetAsync($"{ApiSetting.Base}api/users/{ message.From.Id}");

            if (!result.IsSuccessStatusCode)
            {
                string startText = "Choose your native language" +
                "\nIf Spanish enter: es" +
                "\nIf English enter: en" +
                "\nIf Russia enter: ru";
                await client.SendTextMessageAsync(message.From.Id, startText);

                Bot.OnMessage += GetString;
            }
            else
            {
                await client.SendTextMessageAsync(message.From.Id, "You have already chosen the language!");
            }
        }

        private async void GetString(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            //if (e.Message.Text == )
            AnkiTgBot.Models.User user = new AnkiTgBot.Models.User() { TgId = e.Message.From.Id, Lang = e.Message.Text };

            var JSON = JsonConvert.SerializeObject(user);
            var data = new StringContent(JSON, Encoding.UTF8, "application/json");

            await ApiSetting.ApiClient.PostAsync($"{ApiSetting.Base}api/users", data);
            await Bot.SendTextMessageAsync(e.Message.From.Id, await CommandHelper.Translate("Now use this command", user) + "/menu");
            Bot.OnMessage -= GetString;
        }
    }
}
