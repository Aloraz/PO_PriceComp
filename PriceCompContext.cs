using System.Data.Entity;
using System.Linq;
using PriceComp.GUI.Models;

namespace PriceComp.GUI.Database
{
    public class PriceCompContext : DbContext
    {
        public PriceCompContext() : base("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PriceComp;Integrated Security=True;MultipleActiveResultSets=true")
        {
            // Automatically recreate the database if we change the class models
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PriceCompContext>());
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
            // If empty OR if we have corrupted records (no products linked)
            if (!this.Offers.Any() || this.Offers.Any(o => o.Product == null))
            {
                Console.WriteLine("[INFO] Wykryto brak danych lub uszkodzone rekordy. Resetowanie bazy...");
                
                // Clear existing corrupted data
                this.OrderDetails.RemoveRange(this.OrderDetails);
                this.Orders.RemoveRange(this.Orders);
                this.Clients.RemoveRange(this.Clients);
                this.Offers.RemoveRange(this.Offers);
                this.Products.RemoveRange(this.Products);
                this.Stores.RemoveRange(this.Stores);
                this.SaveChanges();

                PriceComp.GUI.DataSeeder.SeedToDatabase(this);
            }
        }
    }
}
