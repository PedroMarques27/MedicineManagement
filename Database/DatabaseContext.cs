using Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            modelBuilder.Entity<PrescriptionModel>()
                .HasMany(p => p.MedicineList)
                .WithOne(m => m.Prescription)
                .HasForeignKey(m => m.PrescriptionId)
                .OnDelete(DeleteBehavior.SetNull);


        }
    }
}
