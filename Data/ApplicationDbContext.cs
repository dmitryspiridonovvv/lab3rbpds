using FitnessCenterLab3.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FitnessCenterLab3.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Trainer> Trainers => Set<Trainer>();
        public DbSet<Zone> Zones => Set<Zone>();
        public DbSet<MembershipPlan> MembershipPlans => Set<MembershipPlan>();
        public DbSet<MembershipPlanZone> MembershipPlanZones => Set<MembershipPlanZone>();
        public DbSet<MembershipSale> MembershipSales => Set<MembershipSale>();
        public DbSet<GroupClass> GroupClasses => Set<GroupClass>();
        public DbSet<ClassSignup> ClassSignups => Set<ClassSignup>();
        public DbSet<Visit> Visits => Set<Visit>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MembershipPlanZone>()
                .HasKey(pz => new { pz.MembershipPlanID, pz.ZoneID });

            // --- Зоны ---
            modelBuilder.Entity<Zone>().HasData(
                new Zone { ZoneID = 1, Name = "Тренажёрный зал" },
                new Zone { ZoneID = 2, Name = "Бассейн" },
                new Zone { ZoneID = 3, Name = "Групповые занятия" }
            );

            // --- 1000 тестовых клиентов ---
            var clients = new List<Client>();
            var rnd = new Random();

            string[] firstNames = { "Иван", "Пётр", "Сергей", "Дмитрий", "Алексей", "Никита", "Андрей", "Михаил", "Егор", "Роман" };
            string[] lastNames = { "Иванов", "Петров", "Сидоров", "Спиридонов", "Кузнецов", "Смирнов", "Попов", "Фёдоров", "Ершов", "Морозов" };
            string[] genders = { "M", "F" };

            for (int i = 1; i <= 1000; i++)
            {
                var first = firstNames[rnd.Next(firstNames.Length)];
                var last = lastNames[rnd.Next(lastNames.Length)];
                var gender = genders[rnd.Next(genders.Length)];
                var phone = $"+7 (9{rnd.Next(10, 99)}) {rnd.Next(100, 999)}-{rnd.Next(10, 99)}-{rnd.Next(10, 99)}";

                clients.Add(new Client
                {
                    ClientID = i,
                    FirstName = first,
                    LastName = last,
                    Gender = gender,
                    BirthDate = DateTime.Now.AddYears(-rnd.Next(18, 60)).AddDays(rnd.Next(0, 365)),
                    Phone = phone,
                    Email = $"{first.ToLower()}.{last.ToLower()}{i}@mail.ru",
                    RegistrationDate = DateTime.Now.AddDays(-rnd.Next(0, 365))
                });
            }

            modelBuilder.Entity<Client>().HasData(clients);
        }
    }
}
