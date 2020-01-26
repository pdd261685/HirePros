using Microsoft.EntityFrameworkCore;
using HirePros.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HirePros.Data
{
    public class ServiceDbContext : DbContext
    {
        public DbSet<Professional> Professionals { get; set; }
        public DbSet<Services> Services { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserProf> UserProfs { get; set; }

        public ServiceDbContext(DbContextOptions<ServiceDbContext> options) : base(options)
        {
        }

        //Setup primary key associated with the new joint table
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProf>().HasKey(u => new { u.UserID, u.ProfessionalID });//Determines the primary key tahts composite for the joint class UserProf
                                                                                            //Consists of a composite that consists of User ID and Professional ID
                                                                                            //Pair of the 2 IDs will be unique and pair will occur only once
        }

    }
}

