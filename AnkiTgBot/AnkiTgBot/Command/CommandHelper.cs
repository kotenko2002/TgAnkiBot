using AnkiTgBot.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AnkiTgBot.Models.User;

namespace AnkiTgBot.Command
{
    public static class CommandHelper
    {
        public static TelegramBotClient Bot { get; set; }

        public static async Task<AnkiTgBot.Models.User> GetUserInfo(Message message)
        {
            var result = await ApiSetting.ApiClient.GetAsync($"{ApiSetting.Base}api/users/{message.From.Id}");

            var contentL = result.Content.ReadAsStringAsync().Result;
            var userInfo = JsonConvert.DeserializeObject<AnkiTgBot.Models.User>(contentL);

            return userInfo;
        }

        public static async Task<IEnumerable<Desk>> GetDeskByUser(int id)
        {
            var result = await ApiSetting.ApiClient.GetAsync($"{ApiSetting.Base}api/users/desks/{id}");

            var content = result.Content.ReadAsStringAsync().Result;
            var userInfo = JsonConvert.DeserializeObject<IEnumerable<Desk>>(content);

            return userInfo;
        }

        public static async Task<IEnumerable<Card>> GetCardByDesk(int id)
        {
            var result = await ApiSetting.ApiClient.GetAsync($"{ApiSetting.Base}api/desks/{id}");

            var content = result.Content.ReadAsStringAsync().Result;
            var userInfo = JsonConvert.DeserializeObject<IEnumerable<Card>>(content);

            return userInfo;
        }

        public static async Task CreateCard(Card card, Message message)
        {

            var JSON = JsonConvert.SerializeObject(card);
            var data = new StringContent(JSON, Encoding.UTF8, "application/json");

            var response = await ApiSetting.ApiClient.PostAsync($"{ApiSetting.Base}api/cards", data);

            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Succesfully saved");
                //await Bot.SendTextMessageAsync(message.Chat.Id, $"Card created");
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }

        public static async Task<Card> GetCardById(int id)
        {
            var result = await ApiSetting.ApiClient.GetAsync($"{ApiSetting.Base}api/cards/{id}");

            var content = result.Content.ReadAsStringAsync().Result;
            var userInfo = JsonConvert.DeserializeObject<Card>(content);

            return userInfo;
        }
        public static async Task DeleteCard(int id)
        {
            var result = await ApiSetting.ApiClient.DeleteAsync($"{ApiSetting.Base}api/cards/{id}");

            //var content = result.Content.ReadAsStringAsync().Result;
            //var userInfo = JsonConvert.DeserializeObject<Card>(content);

            //return userInfo;
        }
        public static async Task DeleteDesk(int id)
        {
            var result = await ApiSetting.ApiClient.DeleteAsync($"{ApiSetting.Base}api/desks/{id}"); 
        }

        public static async Task<string> Translate(string text, User user)
        {
            using (HttpResponseMessage response = await ApiSetting.ApiClient.GetAsync($"{ApiSetting.Base}api/part/translate?text={text}&toLang={user.Lang}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content.ReadAsStringAsync().Result;
                    Translation result = JsonConvert.DeserializeObject<Translation>(content);

                    var translate = result.Data.Translation;
                    return translate;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
        public static List<Card> SaveRest<T>(this IEnumerable<Card> e)
        {
            var list = new List<Card>();
            foreach (var item in e)
            {
                list.Add(item);
            }
            return list;
        }
    }
}
