using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bots.Types;
using UtilityBot.Controllers;

class Bot : BackgroundService
{
    ITelegramBotClient _telegramBotClient;
    DefaultMessageController _defaultMessageController;
    InlineKeyboardController _inlineKeyboardController;
    TextMessageController _textMessageController;
    List<string> _messages = new List<string>();
    public Bot(
        ITelegramBotClient telegramBotClient,
        DefaultMessageController defaultMessageController,
        InlineKeyboardController inlineKeyboard,
        TextMessageController textMessageController
        )
    {
        _telegramBotClient = telegramBotClient;
        _defaultMessageController = defaultMessageController;
        _inlineKeyboardController = inlineKeyboard;
        _textMessageController = textMessageController;
    }
    

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _telegramBotClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            new Telegram.Bot.Polling.ReceiverOptions() { AllowedUpdates = { } },
            cancellationToken: stoppingToken);

        Console.WriteLine("Бот запущен");
    }
    async Task HandleUpdateAsync(ITelegramBotClient botClient, Telegram.Bot.Types.Update update, CancellationToken
        cancellationToken)
    {

        if(update.Type == Telegram.Bot.Types.Enums.UpdateType.CallbackQuery)
        {
            await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
        }
        if(update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            switch(update.Message.Type)
            {
                case MessageType.Text:
                        await _textMessageController.Handle(update, cancellationToken);
                    return;
                default:
                    await _defaultMessageController.Handle(update.Message, cancellationToken);
                    return;
            }
        }

    }

    Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, 
        CancellationToken cancellationToken)
    {
        var errorMessage = exception switch
        {
            ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}\n{apiRequestException.StackTrace}",
            _ => exception.ToString()
        };

        Console.WriteLine(errorMessage);
        Console.WriteLine("Ожидайте 10 секунд");
        Thread.Sleep(10000);
        return Task.CompletedTask;
    }
}