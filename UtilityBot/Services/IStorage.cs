using UtilityBot.Models;

namespace UtilityBot.Services
{
    public interface IStorage
    {
        Session GetSession(long chatId);
    }
}