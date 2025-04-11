using HealthMed.Migrator.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

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
            base.OnModelCreating(modelBuilder);
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
    }
}