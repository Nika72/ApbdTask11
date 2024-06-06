using Microsoft.EntityFrameworkCore;
using task11.Models;

namespace task11.Data
{
    public class AnimalClinicContext : DbContext
    {
        public AnimalClinicContext(DbContextOptions<AnimalClinicContext> options)
            : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<AnimalType> AnimalTypes { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Visit> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animal>()
                .HasOne(a => a.AnimalType)
                .WithMany()
                .HasForeignKey(a => a.AnimalTypesId);

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.Animal)
                .WithMany(a => a.Visits)
                .HasForeignKey(v => v.AnimalId);

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.Employee)
                .WithMany()
                .HasForeignKey(v => v.EmployeeId);

            base.OnModelCreating(modelBuilder);
        }
    }
}