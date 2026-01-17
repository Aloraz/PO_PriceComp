using System;

namespace projektPO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var stores = new List<Store>();
            var products = new List<Product>();
            var offers = new List<Offer>();

            stores.Add(new LocalStore("Biedronka"));
            stores.Add(new OnlineStore("Allegro", 12));

            products.Add(new Product("Mleko"));

            offers.Add(new Offer(products[0], stores[0], 3.20m));
            offers.Add(new Offer(products[0], stores[1], 2.80m));

            foreach (var offer in offers)
            {
                Console.WriteLine($"{offer.Product.Name} - {offer.Store.Name} - {offer.TotalPrice} zł");
            }
        }
    }    }