using System.Collections.Generic;
using System.Threading.Tasks;
using GymBookingNC19.Core.Models;

namespace GymBookingNC19.Core.Repositories
{
    public interface IApplicationUserGymClassRepository
    {
        void Add(ApplicationUserGymClass book);
        Task<List<GymClass>> GetAllBookingsAsync(string userId);
        void Remove(ApplicationUserGymClass attending);
    }
}