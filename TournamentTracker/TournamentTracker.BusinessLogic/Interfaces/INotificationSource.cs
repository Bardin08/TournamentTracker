using System;
using System.Threading.Tasks;

namespace TournamentTracker.BusinessLogic.Interfaces
{
    public interface INotificationSource
    {
        Task Notify(string message);        
        Task Notify(Func<string> formatter);        
    }
}
