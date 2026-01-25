
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PriceComp.GUI.Models
{
    public class Offer : IPriceable, IComparable<Offer>
    {
        private decimal _price;
        [Key]
        public int OfferID { get; set; }
        public Product Product { get; set; }
        public Store Store { get; set; }

        public ICollection<OrderDetails> Order_Details { get; set; } = new List<OrderDetails>();

        public decimal Price
        {
            get => _price;
            set
            {
                if (value < 0) throw new InvalidPriceException("Cena nie może być ujemna.");
                _price = value;
            }
        }

        public decimal? PromoPrice { get; set; } 
        public string PromoDescription { get; set; }

        public decimal UnitPrice => Price / Product.Quantity;

        public Offer() { }

        public Offer(Product product, Store store, decimal price,
                     decimal? promoPrice = null, string promoDesc = null)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Store = store ?? throw new ArgumentNullException(nameof(store));
            Price = price;

            PromoPrice = promoPrice;
            PromoDescription = promoDesc;
        }

        public decimal TotalPrice => Price + Store.GetAdditionalCost();

        public int CompareTo(Offer other)
        {
            if (other == null) return 1;
            return this.TotalPrice.CompareTo(other.TotalPrice);
        }
    }
    public class InvalidPriceException : Exception
    {
        public InvalidPriceException(string message) : base(message) { }
    }

    public interface IPriceable
    {
        decimal TotalPrice { get; }
    }
}
