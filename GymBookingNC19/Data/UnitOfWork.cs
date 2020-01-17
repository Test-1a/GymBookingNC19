using GymBookingNC19.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GymBookingNC19.Data
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext context;

        public GymClassesRepository gymClassesRepository { get; private set; }
        public ApplicationUserGymClassRepository applicationUserGymClassRepository { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            gymClassesRepository = new GymClassesRepository(context);
            applicationUserGymClassRepository = new ApplicationUserGymClassRepository(context);
        }


        public async Task CompleteAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
