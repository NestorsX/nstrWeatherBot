using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace nstrWeatherBot
{
    public class Bot
    {
        private readonly TelegramBotClient _bot;
        private readonly Weather _weather;

        public Bot(string token)
        {
            if(token == null)
            {
                throw new ArgumentNullException("API token is null");
            }

            _bot = new TelegramBotClient(token);
            _weather = new Weather();
        }

        public void Start()
        {
            using var cts = new CancellationTokenSource();
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { }
            };

            _bot.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken: cts.Token);
            User me = _bot.GetMeAsync().Result;
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            cts.Cancel();
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message)
            {
                return;
            }
            if (update.Message!.Type != MessageType.Text)
            {
                return;
            }

            long chatId = update.Message.Chat.Id;
            string messageText = update.Message.Text;
            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");
            if (messageText != "/start")
            {
                string weatherInfo = _weather.GetWeatherInfo(messageText);
                await botClient.SendTextMessageAsync(chatId: chatId, text: weatherInfo, cancellationToken: cancellationToken);
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId: chatId, text: "Привет! Я погодный бот.\nОтправь мне название города, для которого хочешь узнать погоду.", cancellationToken: cancellationToken);
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
