using AnkiTgBot.Command.Commands;
using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace AnkiTgBot
{
    class Program
    {
        private static TelegramBotClient client;
        private static List<Command.Command> commands;
        static void Main(string[] args)
        {
            ApiSetting.InitializeClient();

            client = new TelegramBotClient(Config.Token);

            commands = new List<Command.Command>();
            commands.Add(new Start());
            commands.Add(new StartMenu());
            commands.Add(new PostNewDesk());
            commands.Add(new PostNewCard());
            commands.Add(new PostNewCardWithTR()); 
            commands.Add(new GetAllCardsByDesk());
            commands.Add(new GetAllFavoriteCards());
            commands.Add(new PatchAddFavorite());
            commands.Add(new PatchRemoveFavorite());
            commands.Add(new DeleteDesk());
            commands.Add(new DeleteCard());
            commands.Add(new Reiteration());
            commands.Add(new ReiterationFavorite());
            commands.Add(new Game());

            var me = client.GetMeAsync().Result;
            Console.WriteLine(me.FirstName);

            client.OnMessage += Client_OnMessagReceivede;
            client.StartReceiving();
            Console.WriteLine("Bot started");
            Console.ReadLine();
            client.StopReceiving();
        }

        private static async void Client_OnMessagReceivede(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if(message.Text != null)
            {
                Console.WriteLine($"Пользователь {message.From.Id} отправил сообщение: \"{message.Text}\"");

                foreach (var command in commands)
                {
                    if (message.Text == command.Name)
                    {
                        command.Execute(message, client);
                    }
                }
            }
                
        }
    }
}
