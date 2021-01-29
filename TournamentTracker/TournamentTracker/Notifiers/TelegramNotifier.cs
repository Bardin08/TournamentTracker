using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Types.Enums;

using TournamentTracker.Interfaces;
using TournamentTracker.Notifiers.Options;

namespace TournamentTracker.Notifiers
{
    public class TelegramNotifier : INotificationSource
    {
        private readonly ITelegramBotClient _client;
        private readonly long _chatId;

        public TelegramNotifier(TelegramNotifierOptions options)
        {
            _client = new TelegramBotClient(options.BotToken);
            _chatId = options.ChatId;
        }

        public async Task Notify(string message)
        {
            await _client.SendTextMessageAsync(_chatId, message, ParseMode.Markdown);
        }
    }
}
