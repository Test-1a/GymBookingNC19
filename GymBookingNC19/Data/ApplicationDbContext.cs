using System;
using System.Collections.Generic;
using System.Text;
using GymBookingNC19.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymBookingNC19.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GymClass>  GymClasses { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //Always first
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUserGymClass>()
                .HasKey(k => new 
                { 
                    k.ApplicationUserId,
                    k.GymClassId 
                });
        }
    }
}
