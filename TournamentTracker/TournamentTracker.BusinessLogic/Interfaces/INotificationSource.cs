using System.Threading.Tasks;

namespace TournamentTracker.BusinessLogic.Interfaces
{
    public interface INotificationSource
    {
        Task Notify(string message);        
    }
}
