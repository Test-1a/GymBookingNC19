using GymBookingNC19.Core;
using GymBookingNC19.Core.Repositories;
using GymBookingNC19.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBookingNC19.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext context;

        public IGymClassesRepository GymClasses { get; private set; }
        public IApplicationUserGymClassRepository UserGymClasses { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            GymClasses = new GymClassesRepository(context);
            UserGymClasses = new ApplicationUserGymClassRepository(context);
        }


        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
