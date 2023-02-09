using System.Collections.Concurrent;
using UtilityBot.Models;

namespace UtilityBot.Services
{
    internal class MemoryStorage : IStorage
    {
        readonly ConcurrentDictionary<long, Session> _sessions;

        public MemoryStorage()
        {
            _sessions = new ConcurrentDictionary<long, Session>();    
        } 

        public Session GetSession(long chatId)
        {
            if(_sessions.ContainsKey(chatId))
            {
                return _sessions[chatId];
            }
            else
            {
                var newSession = new Session() { SetCommand = string.Empty };
                _sessions.TryAdd(chatId, newSession);
                return newSession;
            }
        }
    }
}