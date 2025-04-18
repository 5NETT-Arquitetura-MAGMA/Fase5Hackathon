using HealthMed.Migrator.Data.Entities;
using HealthMed.Migrator.Data.Entities.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace HealthMed.Migrator.Data
{
    public class HealthMedDBContext : DbContext
    {
        public HealthMedDBContext(DbContextOptions<HealthMedDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<MedicalConsultation> MedicalConsultations { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.DoctorSchedules)
                .WithOne(h => h.Doctor)
                .HasForeignKey(h => h.DoctorId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<DoctorSchedule>()
                .HasIndex(h => new { h.DoctorId, h.DayOfWeek })
                .IsUnique();
            modelBuilder.Entity<MedicalConsultation>()
                .HasOne(mc => mc.Patient)
                .WithMany()
                .HasForeignKey(mc => mc.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MedicalConsultation>()
                .HasOne(mc => mc.Doctor)
                .WithMany()
                .HasForeignKey(mc => mc.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // modelBuilder.Entity<User>().HasData(
            //    new User
            //    {
            //        Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
            //        Name = "Administrador",
            //        PhoneNumber = "11999999999",
            //        EmailAddress = "admin@healthmed.com",
            //        Login = "admin",
            //        Password = "",
            //        SecurityHash = "",
            //        Type = UserType.Admin
            //    }
            //);

            base.OnModelCreating(modelBuilder);
        }

        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<HealthMedDBContext>();

            var adminUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Type == UserType.Admin);

            if (adminUser == null)
            {
                Console.WriteLine("Usuário Administrador não encontrado.");
                Console.WriteLine("Por favor, defina uma senha para o usuário Administrador.");

                string password = GetValidAdminPassword();
                string securityHash = GenerateSecurityHash();
                string hashedPassword = HashPassword(password, securityHash);

                var newAdminUser = new User
                {
                    Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
                    Name = "Administrador",
                    PhoneNumber = "11999999999",
                    EmailAddress = "admin@healthmed.com",
                    Login = "admin",
                    Password = hashedPassword,
                    SecurityHash = securityHash,
                    Type = UserType.Admin
                };

                dbContext.Users.Add(newAdminUser);
                await dbContext.SaveChangesAsync();
                Console.WriteLine("Usuário Administrador criado com sucesso.");
            }
            else
            {
                Console.WriteLine("Usuário Administrador já existe.");
            }
        }

        private static string GetValidAdminPassword()
        {
            string password;
            Regex pattern = new Regex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[^a-zA-Z\\d]).{12,}$");

            while (true)
            {
                Console.Write("Senha (mínimo 12 caracteres, uma maiúscula, uma minúscula, números e caracteres especiais): ");
                password = Console.ReadLine();

                if (pattern.IsMatch(password))
                {
                    return password;
                }
                else
                {
                    Console.WriteLine("A senha não atende aos requisitos. Por favor, tente novamente.");
                }
            }
        }

        private static string GenerateSecurityHash()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        private static string HashPassword(string password, string securityHash)
        {
            byte[] saltBytes = Convert.FromBase64String(securityHash);
            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000))
            {
                byte[] hashBytes = rfc2898DeriveBytes.GetBytes(32);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }

    public class HealthMedDbContextFactory : IDesignTimeDbContextFactory<HealthMedDBContext>
    {
        public HealthMedDBContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                     .SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<HealthMedDBContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new HealthMedDBContext(optionsBuilder.Options);
        }

        public static class DatabaseInitializer
        {
            public static async Task InitializeDatabase(IServiceProvider serviceProvider)
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<HealthMedDBContext>();

                // Aplica as migrações pendentes
                await dbContext.Database.MigrateAsync();

                // Executa o seed do usuário Admin
                await HealthMedDBContext.SeedAdminUser(serviceProvider);
            }
        }
    }
}