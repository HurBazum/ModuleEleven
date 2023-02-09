using Telegram.Bot;
using Telegram.Bot.Types;
using UtilityBot.Services;

namespace UtilityBot.Controllers
{
    public class InlineKeyboardController
    {
        readonly ITelegramBotClient _telegramClient;
        readonly IStorage _memoryStorage;

        public InlineKeyboardController(
            ITelegramBotClient telegramBotClient,
            IStorage storage
            )
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = storage;
        }
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

                _memoryStorage.GetSession(callbackQuery.From.Id).SetCommand = callbackQuery.Data;

            string commandText = callbackQuery.Data switch
            {
                "/sum" => "Сумма чисел",
                "/length" => "Длина сообщения",
                _ => string.Empty
            };

            await _telegramClient.SendTextMessageAsync(callbackQuery.From.Id, $"<b>Выбрана " +
                $"опция - {commandText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню(\"/start\")." +
                $"{Environment.NewLine}Или введя название команды в чат.",
                cancellationToken: ct, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
        }
    }
}