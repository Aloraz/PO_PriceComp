using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektPO
{
    internal class Order
    {
        Client client;
        public List<Product> products;
        public decimal totalPrice;
        Store store;

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
    }
}
