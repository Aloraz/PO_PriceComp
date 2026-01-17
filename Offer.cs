using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektPO
{
    public class Offer
    {
        private decimal _price;

        public Product Product { get; private set; }
        public Store Store { get; private set; }

        public decimal Price
        {
            get => _price;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Cena musi być większa od zera.");
                _price = value;
            }
        }

        public Offer(Product product, Store store, decimal price)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Store = store ?? throw new ArgumentNullException(nameof(store));
            Price = price;
        }

        public decimal TotalPrice => Price + Store.GetAdditionalCost();
    }
}
