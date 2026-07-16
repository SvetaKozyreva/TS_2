using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TS_2.Models;

namespace TS_2.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Training> Trainings { get; set; }

        public DbSet<TrainingRegistration> TrainingRegistrations { get; set; }

        public DbSet<Abonement> Abonements { get; set; }

        public DbSet<UserAbonement> UserAbonements { get; set; }

        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Database/FitStudio.db");
        }
    }
}
