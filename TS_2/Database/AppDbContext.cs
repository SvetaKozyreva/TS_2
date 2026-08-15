using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TS_2.Models;
using System.IO;

namespace TS_2.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Training> Trainings { get; set; }

        public DbSet<TrainingRegistration> TrainingRegistrations { get; set; }

        public DbSet<Abonement> Abonement { get; set; }

        public DbSet<UserAbonement> UserAbonement { get; set; }

        public DbSet<Event> Event { get; set; }
        public DbSet<EventsRegistration> EventsRegistration { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Database",
                "TS.db");

            optionsBuilder.UseSqlite($"Data Source={path}");
        }
    }
}