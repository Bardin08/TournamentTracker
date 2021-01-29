using System.Threading.Tasks;

namespace TournamentTracker.Interfaces
{
    public interface INotificationSource
    {
        Task Notify(string message);        
    }
}
