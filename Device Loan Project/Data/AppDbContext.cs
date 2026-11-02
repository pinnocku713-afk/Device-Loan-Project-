using Device_Loan_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Device_Loan_Project.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext (DbContextOptions<AppDbContext> options) : base (options) { }

        public DbSet<Device> Devices => Set<Device>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            b.Entity<Device>().HasIndex(d => d.AssetTag).IsUnique();

            b.Entity<Device>().HasData(

                new Device { Id = 1, AssetTag = "ECC-0042", Name = "Dell Latitude", Category = "Laptop", PurchaseYear = "2023", IsOnLoan = "yes" },
                new Device { Id = 2, AssetTag = "ECC-0007", Name = "HP ProBook 450", Category = "Laptop", PurchaseYear = "2019", IsOnLoan = "no" },
                new Device { Id = 3, AssetTag = "ECC-0115", Name = "iPad 9th generation", Category = "Tablet", PurchaseYear = "2022", IsOnLoan = "yes" },
                new Device { Id = 4, AssetTag = "ECC-0090", Name = "Samsung Galexy Tab A8", Category = "Tablet", PurchaseYear = "2021", IsOnLoan = "no" },
                new Device { Id = 5, AssetTag = "ECC-0023", Name = "Lenovo ThinkPad E 14", Category = "Laptop", PurchaseYear = "2017", IsOnLoan = "no" } );
        }
    }
}
