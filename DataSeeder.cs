using System;
using System.Collections.Generic;
using projektPO.projektPO;

namespace projektPO
{
    public static class DataSeeder
    {
        public static void Initialize(List<Offer> offers)
        {
            try
            {
                Console.WriteLine("[INFO] Generowanie dużej bazy danych...");

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
                var maslo = new Product("Masło Extra 82%", 0.2m, "kg"); // 200g to 0.2kg
                var mleko = new Product("Mleko Łaciate 3.2%", 1.0m, "l");

                // --- CHEMIA 
                var persilMaly = new Product("Proszek Persil Color (Mały)", 1.5m, "kg");
                var persilXXL = new Product("Proszek Persil Color (XXL)", 5.0m, "kg");

                // --- PRZEKĄSKI ---
                var chipsyDuze = new Product("Chipsy Lay's Solone (Duże)", 0.200m, "kg"); // 200g
                var chipsyMale = new Product("Chipsy Lay's Solone (Małe)", 0.080m, "kg"); // 80g

                // ============================================================
                // TWORZENIE OFERT (Ceny, Promocje, Pułapki)
                // ============================================================

                // --- SCENARIUSZ 1: COCA-COLA (Warianty) ---

                // Biedronka: Opłaca się w wielopaku
                offers.Add(new Offer(cola2L, biedronka, 8.50m, promoPrice: 6.49m, promoDesc: "Przy zakupie 4-paku"));

                // Lidl: Standardowa cena, ale Pepsi tańsza
                offers.Add(new Offer(cola2L, lidl, 8.50m));
                offers.Add(new Offer(pepsi, lidl, 6.99m, promoPrice: 5.50m, promoDesc: "Super Cena!"));

                // Żabka: Małe pojemności (drogie jednostkowo, ale niska cena przy kasie)
                offers.Add(new Offer(cola05L, zabka, 4.50m)); // Aż 9 zł za litr!
                offers.Add(new Offer(cola05L, zabka, 4.50m, promoPrice: 3.50m, promoDesc: "2 szt. z aplikacją")); // 7 zł za litr

                // Auchan: Średnia butelka
                offers.Add(new Offer(cola1L, auchan, 5.99m));


                // --- SCENARIUSZ 2: WOJNA MASŁOWA (Promocje ilościowe) ---

                // Biedronka: 3+1 Gratis
                // Cena std: 7.00. Kupujesz 4, płacisz za 3. (21zł / 4 = 5.25 zł/szt)
                offers.Add(new Offer(maslo, biedronka, 7.00m, promoPrice: 5.25m, promoDesc: "3+1 Gratis"));

                // Lidl: Przy zakupie 3 sztuk -33%
                offers.Add(new Offer(maslo, lidl, 6.50m, promoPrice: 4.49m, promoDesc: "Przy zakupie 3 szt."));

                // Allegro: Tanie masło, ale dojdzie dostawa (dobre tylko przy hurtowych zakupach)
                offers.Add(new Offer(maslo, allegro, 4.00m));


                // --- SCENARIUSZ 3: CHEMIA (Pułapka Unit Pricing) ---

                // Żabka: Mały proszek. Cena przy kasie niska (25 zł), ale za kg wychodzi 16.66 zł
                offers.Add(new Offer(persilMaly, zabka, 25.00m));

                // Auchan: Wielki wór. Cena przy kasie wysoka (60 zł), ale za kg wychodzi 12.00 zł
                offers.Add(new Offer(persilXXL, auchan, 60.00m, promoPrice: 49.99m, promoDesc: "MEGA PAKA"));

                // Allegro: Jeszcze większa promocja na duży wór, ale + dostawa
                offers.Add(new Offer(persilXXL, allegro, 45.00m));


                // --- SCENARIUSZ 4: PRZEKĄSKI (Lays) ---

                // Biedronka
                offers.Add(new Offer(chipsyDuze, biedronka, 8.00m, promoPrice: 5.99m, promoDesc: "Drugi produkt -50%"));
                offers.Add(new Offer(chipsyMale, biedronka, 4.50m)); // Bardzo nieopłacalne

                // Lidl
                offers.Add(new Offer(chipsyDuze, lidl, 7.50m));


                // --- INNE ---
                offers.Add(new Offer(woda, biedronka, 1.89m, promoPrice: 1.49m, promoDesc: "6-pak"));
                offers.Add(new Offer(woda, lidl, 1.79m));
                offers.Add(new Offer(mleko, biedronka, 3.20m));
                offers.Add(new Offer(mleko, lidl, 3.20m));

                // Zapis
                DataManager.SaveOffers(offers);
                Console.WriteLine("[SUKCES] Baza danych została wygenerowana (Produkty, Promocje, Warianty)!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BŁĄD SEEDOWANIA] {ex.Message}");
            }
        }
    }
}