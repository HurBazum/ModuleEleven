using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.ExceptionServices;
using System.Text;
using Telegram.Bot;
using Telegram.Bots.Requests;
using UtilityBot.Configuration;
using UtilityBot.Controllers;
using UtilityBot.Services;

class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;

        var host = new HostBuilder()
            .ConfigureServices((hostContext, services) => ConfigureServices(services))
            .UseConsoleLifetime()
            .Build();

        testApiAsync();
        await Console.Out.WriteLineAsync("Сервис запущен");
        await host.RunAsync();
        // ctrl + c(?)
        await Console.Out.WriteLineAsync("Сервис остановлен");
    }
    static void ConfigureServices(IServiceCollection services)
    {
        AppSettings appSettings = BuildAppSettings();

        services.AddSingleton<IStorage, MemoryStorage>();

        services.AddTransient<DefaultMessageController>();
        services.AddTransient<InlineKeyboardController>();
        services.AddTransient<TextMessageController>();

        services.AddSingleton<ITelegramBotClient>(provider => new TelegramBotClient(
            appSettings.BotToken));
        services.AddHostedService<Bot>();
    }

    static AppSettings BuildAppSettings()
    {
        return new AppSettings()
        {
            BotToken = "6148391484:AAFggPqJXQUDEH6F-r4gz0hXQF4Fp6-FnnE"
        };
    }

    /// <summary>
    /// получает немного информации о боте
    /// </summary>
    static async void testApiAsync()
    {
        var Bot = new TelegramBotClient("6148391484:AAFggPqJXQUDEH6F-r4gz0hXQF4Fp6-FnnE");
        var me = await Bot.GetMeAsync();
        Console.WriteLine("Hello my name is " +  me.FirstName  + "\nMy username is " + me.Username + "\nMy ID is " + me.Id);
    }
}