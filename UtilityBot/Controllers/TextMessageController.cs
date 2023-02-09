using Polly;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using UtilityBot.Models;
using UtilityBot.Services;
using UtilityBot.Utilities;

namespace UtilityBot.Controllers
{
    internal class TextMessageController
    {
        readonly ITelegramBotClient _telegramClient;
        readonly IStorage _memoryStorage;

        public TextMessageController(
            ITelegramBotClient telegramBotClient,
            IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage;
        }
        public async Task Handle(Update update, CancellationToken ct)
        {
            switch(update.Message!.Text) 
            {
                case "/start":
                    // при удалении всего чата, в SetCommand оставалось значение, поэтому добавлена эта строка,
                    // чтоб его аннулировать; и вообще если пользователь решил сменить команду - стоит отменить
                    // предыдущую
                    _memoryStorage.GetSession(update.Message.Chat.Id).SetCommand = string.Empty;
                    var buttons = new List<InlineKeyboardButton[]>
                    {
                        new[]
                        {
                            InlineKeyboardButton.WithCallbackData($"длина текста", $"/length"),
                            InlineKeyboardButton.WithCallbackData($"сумма чисел", $"/sum")
                        }

                    };
                     await _telegramClient.SendTextMessageAsync(update.Message.Chat.Id,
                        $"<b>Наш бот считает количество символов в сообщении.</b> {Environment.NewLine}"
                        + $"{Environment.NewLine}Еще он умеет складывать числа " +
                        $"{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html,
                        replyMarkup: new InlineKeyboardMarkup(buttons));
                    break;

                case "/sum":
                    _memoryStorage.GetSession(update.Message.Chat.Id).SetCommand = "/sum";
                    await _telegramClient.SendTextMessageAsync(update.Message.Chat.Id, $"Окей! Выбрана опция <b>подсчёта суммы введённых чисел.</b>",
                        parseMode: ParseMode.Html,cancellationToken: ct);
                    break;

                case "/length":
                    _memoryStorage.GetSession(update.Message.Chat.Id).SetCommand = "/length";
                    await _telegramClient.SendTextMessageAsync(update.Message.Chat.Id, $"Окей! Выбрана опция <b>подсчёта символов в сообщении.</b>", 
                        parseMode: ParseMode.Html, cancellationToken: ct);
                    break;

                default:;
                    await _telegramClient.SendTextMessageAsync(update.Message.Chat.Id, 
                        Processor.TextProcessing(_memoryStorage.GetSession(update.Message.Chat.Id).SetCommand, update.Message.Text!),
                        parseMode: ParseMode.Html, cancellationToken: ct);
                    break;
            }
        }
    }
}