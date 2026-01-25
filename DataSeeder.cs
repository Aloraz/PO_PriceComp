using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using PriceComp.GUI.Database;
using PriceComp.GUI.Models;

namespace PriceComp.GUI
{
    public static class DataSeeder
    {
        public static void Initialize(List<Offer> offers)
        {
            try
            {
                // Clear previous list
                offers.Clear();

                //  SKLEPY
                var biedronka = new LocalStore("Biedronka");
                var lidl = new LocalStore("Lidl");
                var zabka = new LocalStore("Żabka");
                var auchan = new LocalStore("Auchan");
                var allegro = new OnlineStore("Allegro", 12.99m);

                // --- NAPOJE ---
                var cola2L = new Product("Napój Coca-Cola 2L", 2.0m, "l");
                var cola1L = new Product("Napój Coca-Cola 1L", 1.0m, "l");
                var cola05L = new Product("Napój Coca-Cola 0.5L (Mała)", 0.5m, "l");
                var pepsi = new Product("Napój Pepsi Max 2L", 2.0m, "l");
                var woda = new Product("Woda Źródlana Żywiec", 1.5m, "l");

                // --- NABIAŁ ---
                var maslo = new Product("Masło Extra 82%", 0.2m, "kg"); 
                var mleko = new Product("Mleko Łaciate 3.2%", 1.0m, "l");

                // --- CHEMIA 
                var persilMaly = new Product("Proszek Persil Color (Mały)", 1.5m, "kg");
                var persilXXL = new Product("Proszek Persil Color (XXL)", 5.0m, "kg");

                // --- PRZEKĄSKI ---
                var chipsyDuze = new Product("Chipsy Lay's Solone (Duże)", 0.200m, "kg"); 
                var chipsyMale = new Product("Chipsy Lay's Solone (Małe)", 0.080m, "kg"); 

                // TWORZENIE OFERT
                offers.Add(new Offer(cola2L, biedronka, 8.50m, promoPrice: 6.49m, promoDesc: "Przy zakupie 4-paku"));
                offers.Add(new Offer(cola2L, lidl, 8.50m));
                offers.Add(new Offer(pepsi, lidl, 6.99m, promoPrice: 5.50m, promoDesc: "Super Cena!"));
                offers.Add(new Offer(cola05L, zabka, 4.50m)); 
                offers.Add(new Offer(cola05L, zabka, 4.50m, promoPrice: 3.50m, promoDesc: "2 szt. z aplikacją")); 
                offers.Add(new Offer(cola1L, auchan, 5.99m));
                offers.Add(new Offer(maslo, biedronka, 7.00m, promoPrice: 5.25m, promoDesc: "3+1 Gratis"));
                offers.Add(new Offer(maslo, lidl, 6.50m, promoPrice: 4.49m, promoDesc: "Przy zakupie 3 szt."));
                offers.Add(new Offer(maslo, allegro, 4.00m));
                offers.Add(new Offer(persilMaly, zabka, 25.00m));
                offers.Add(new Offer(persilXXL, auchan, 60.00m, promoPrice: 49.99m, promoDesc: "MEGA PAKA"));
                offers.Add(new Offer(persilXXL, allegro, 45.00m));
                offers.Add(new Offer(chipsyDuze, biedronka, 8.00m, promoPrice: 5.99m, promoDesc: "Drugi produkt -50%"));
                offers.Add(new Offer(chipsyMale, biedronka, 4.50m)); 
                offers.Add(new Offer(chipsyDuze, lidl, 7.50m));
                offers.Add(new Offer(woda, biedronka, 1.89m, promoPrice: 1.49m, promoDesc: "6-pak"));
                offers.Add(new Offer(woda, lidl, 1.79m));
                offers.Add(new Offer(mleko, biedronka, 3.20m));
                offers.Add(new Offer(mleko, lidl, 3.20m));

                // Zapisz do JSON, żeby SeedToDatabase miał co czytać
                DataManager.SaveOffers(offers);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd Initialize: {ex.Message}");
            }
        }

        public static void SeedToDatabase(PriceCompContext context)
        {
            try
            {
                var offers = DataManager.LoadOffers();

                if (offers == null || offers.Count == 0)
                {
                    // If JSON is empty, force regenerate JSON first
                    var list = new List<Offer>();
                    Initialize(list);
                    offers = list;
                }

                if (offers != null && offers.Count > 0)
                {
                    var uniqueStores = new Dictionary<string, Store>(StringComparer.OrdinalIgnoreCase);
                    var uniqueProducts = new Dictionary<string, Product>(StringComparer.OrdinalIgnoreCase);

                    foreach (var offer in offers)
                    {
                        if (offer.Store != null)
                        {
                            if (!uniqueStores.TryGetValue(offer.Store.Name, out var store))
                            {
                                store = offer.Store;
                                uniqueStores[offer.Store.Name] = store;
                            }
                            offer.Store = store;
                        }

                        if (offer.Product != null)
                        {
                            var key = $"{offer.Product.Name}_{offer.Product.UnitName}_{offer.Product.Quantity}";
                            if (!uniqueProducts.TryGetValue(key, out var product))
                            {
                                product = offer.Product;
                                uniqueProducts[key] = product;
                            }
                            offer.Product = product;
                        }
                    }

                    context.Stores.AddRange(uniqueStores.Values);
                    context.Products.AddRange(uniqueProducts.Values);
                    context.Offers.AddRange(offers);
                    context.SaveChanges();
                    System.Windows.MessageBox.Show($"Baza danych zasilona pomyślnie ({offers.Count} ofert)!");
                }
            }
            catch (Exception ex)
            {
                 System.Windows.MessageBox.Show($"Błąd SeedToDatabase: {ex.Message}");
            }
        }
    }
}