using Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<PrescriptionModel> Prescriptions { get; set; }
        public DbSet<MedicineModel> Medicines { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationships
            modelBuilder.Entity<UserModel>()
                .HasMany(u => u.PrescriptionList)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserEmail)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PrescriptionMedicineModel>()
                .HasKey(um => new { um.PrescriptionId, um.MedicineName});

            modelBuilder.Entity<PrescriptionMedicineModel>()
                .HasOne(um => um.PrescriptionModel)
                .WithMany(u => u.MedicineList)
                .HasForeignKey(um => um.PrescriptionId);

            modelBuilder.Entity<PrescriptionMedicineModel>()
                .HasOne(um => um.Medicine)
                .WithMany(m => m.PrescriptionMedicines)
                .HasForeignKey(um => um.MedicineName);


        }
    }
}
