using System;
using System.Collections.Generic;

namespace projektPO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== APLIKACJA PORÓWNYWARKI ===");

            // 1. Próba wczytania danych z pliku
            List<Offer> offers = DataManager.LoadOffers();

            // 2. Jeśli lista jest pusta (pierwsze uruchomienie), dodajemy dane testowe
            if (offers.Count == 0)
            {
                Console.WriteLine("Baza pusta. Tworzę przykładowe dane...");
                try
                {
                    // Tworzymy sklepy
                    var biedronka = new LocalStore("Biedronka");
                    var allegro = new OnlineStore("Allegro", 15.00m); // Dostawa 15 zł

                    // Tworzymy produkt
                    var mleko = new Product("Mleko 3.2%");
                    var laptop = new Product("Laptop Gamingowy");

                    // Tworzymy oferty
                    offers.Add(new Offer(mleko, biedronka, 3.50m));
                    offers.Add(new Offer(mleko, allegro, 3.00m)); // Taniej, ale dojdzie dostawa!
                    offers.Add(new Offer(laptop, allegro, 2500m));

                    // ZAPIS DO PLIKU (Realizacja wymogu 6)
                    DataManager.SaveOffers(offers);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd podczas tworzenia danych: {ex.Message}");
                }
            }

            // 3. Wyświetlanie danych (Logika biznesowa)
            Console.WriteLine("\n--- DOSTĘPNE OFERTY ---");
            foreach (var offer in offers)
            {
                Console.WriteLine($"PRODUKT: {offer.Product.Name}");
                Console.WriteLine($"  Sklep: {offer.Store.Name} (Typ: {offer.Store.GetType().Name})");
                Console.WriteLine($"  Cena: {offer.Price} zł");

                // Polimorfizm w akcji: GetAdditionalCost() zadziała inaczej dla Local i Online
                decimal extraCost = offer.Store.GetAdditionalCost();
                if (extraCost > 0)
                {
                    Console.WriteLine($"  + Dostawa: {extraCost} zł");
                }

                Console.WriteLine($"  SUMA: {offer.TotalPrice} zł");
                Console.WriteLine("-----------------------------");
            }

            Console.WriteLine("\nNaciśnij dowolny klawisz...");
            Console.ReadKey();
        }
    }
}