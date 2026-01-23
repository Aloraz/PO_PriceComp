using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using projektPO;
namespace dataAccess
{
    public class Order
    {
        Client client;
        public List<Product> products;
        public decimal totalPrice;
        public Store store;

        [Key]
        public int OrderID { get; set; }

        public virtual Client Client
        {
            get { return client; }
            set { client = value; }
        }
        public virtual List<Product> Products
        {
            get { return products; }
            set { products = value; }
        }

        public Order(Client client, List<Product> products, decimal totalPrice, Store store)
        {
            this.client = client;
            this.products = products;
            this.totalPrice = totalPrice;
            this.store = store;
        }

        public Order()
        {
            client = new Client();
            products = new List<Product>();
            totalPrice = 0;
            store = null;
        }


        public void SaveToDB()
        {
            using var db = new OrderDbContext();
            Console.WriteLine("Saving changes to the database...");
            db.Orders.Add(this);
            db.SaveChanges();
            Console.WriteLine("Changes saved successfully.");
        }
    }
}
