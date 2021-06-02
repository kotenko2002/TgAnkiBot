using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AnkiTgBot.Command.Commands
{
    class StartMenu : Command
    {
        public override string Name { get; set; } = "/menu";
        public override TelegramBotClient Bot { get; set; }
        public override async void Execute(Message message, TelegramBotClient client)
        {
            var user = await CommandHelper.GetUserInfo(message);
            //string sss = "List of commands";
            //var ss = await CommandHelper.Translate(sss, user);
            //await client.SendTextMessageAsync(message.From.Id, ss);
            //var user = await CommandHelper.GetUserInfo(message);
            string startText = $"{await CommandHelper.Translate("List of commands", user)}" +
                $"\n✔️/add_card - {await CommandHelper.Translate("Add new card", user)}" +
                $"\n✔️/add_cardTR - {await CommandHelper.Translate("Add a card with auto translation", user)}" +
                $"\n✔️/add_deck - {await CommandHelper.Translate("Add new deck", user)}" +
                $"\n👀/view_all_cards - {await CommandHelper.Translate("See a list of all cards", user)}" +
                $"\n👀/view_all_fav - {await CommandHelper.Translate("See a list of favorite cards", user)}" +
                $"\n❤️/add_to_fav - {await CommandHelper.Translate("Add card to favorite", user)}" +
                $"\n💔/remove_from_fav - {await CommandHelper.Translate("Remove card from favorite", user)}" +
                $"\n❌/del_card - {await CommandHelper.Translate("Delete card ", user)}" +
                $"\n❌/del_deck - {await CommandHelper.Translate("Delete deck", user)}" +
                $"\n🏃/go - {await CommandHelper.Translate("Start reiteration", user)}";
            await client.SendTextMessageAsync(message.From.Id, startText);
        }
    }
}
