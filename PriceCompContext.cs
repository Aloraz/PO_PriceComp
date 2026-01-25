using System.Data.Entity;
using System.Linq;
using PriceComp.GUI.Models;

namespace PriceComp.GUI.Database
{
    public class PriceCompContext : DbContext
    {
        public PriceCompContext() : base("name=PriceCompDb")
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderDetails>()
                .HasKey(od => new { od.OrderID, od.OfferID });

            modelBuilder.Entity<OrderDetails>()
                .HasRequired(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID);

            modelBuilder.Entity<OrderDetails>()
                .HasRequired(od => od.Offer)
                .WithMany()
                .HasForeignKey(od => od.OfferID);
        }

        public void SeedIfNotExists()
        {
            if (!this.Offers.Any())
            {
                PriceComp.GUI.DataSeeder.SeedToDatabase(this);
            }
        }
    }
}
