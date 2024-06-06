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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Animal>()
                .HasOne(a => a.AnimalType)
                .WithMany()
                .HasForeignKey(a => a.AnimalTypesId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.Animal)
                .WithMany(a => a.Visits)
                .HasForeignKey(v => v.AnimalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Visit>()
                .HasOne(v => v.Employee)
                .WithMany()
                .HasForeignKey(v => v.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}