using System.Threading.Tasks;
using GymBookingNC19.Core.Repositories;
using GymBookingNC19.Data.Repositories;

namespace GymBookingNC19.Core
{
    public interface IUnitOfWork
    {
        IApplicationUserGymClassRepository UserGymClasses { get; }
        IGymClassesRepository GymClasses { get; }

        Task CompleteAsync();
    }
}