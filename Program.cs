using System;
using System.Collections.Generic;
using System.Linq;
using projektPO.projektPO;

namespace projektPO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Offer> allOffers = DataManager.LoadOffers();

            if (allOffers.Count == 0)
            {
                SeedData(allOffers);
            }

            List<string> myShoppingList = new List<string>();
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== ASYSTENT ZAKUPOWY v2 (Analiza Opłacalności zł/kg) ===");
                Console.WriteLine($"Twój koszyk: [{string.Join(", ", myShoppingList)}]");
                Console.WriteLine("--------------------------------------");
                Console.WriteLine("1. Dodaj produkt do listy (np. Kurczak)");
                Console.WriteLine("2. Wyczyść listę");
                Console.WriteLine("3. OBLICZ GDZIE NAJTANIEJ (Smart Unit Price)");
                Console.WriteLine("4. Pokaż bazę ofert");
                Console.WriteLine("5. Zapisz i Wyjdź");
                Console.Write("Wybór: ");

                var key = Console.ReadKey();

                switch (key.KeyChar)
                {
                    case '1': DodajProduktDoListy(allOffers, myShoppingList); break;
                    case '2': myShoppingList.Clear(); break;
                    case '3':
                        PorownajSklepySmart(allOffers, myShoppingList);
                        Console.ReadKey();
                        break;
                    case '4': PokazBaze(allOffers); Console.ReadKey(); break;
                    case '5': DataManager.SaveOffers(allOffers); running = false; break;
                }
            }
        }

        static void PorownajSklepySmart(List<Offer> allOffers, List<string> myNeed)
        {
            if (myNeed.Count == 0) { Console.WriteLine("\nKoszyk pusty!"); return; }

            Console.WriteLine("\n\n--- ANALIZA SMART (Szukamy najlepszej ceny za KG/L) ---");

            var nazwySklepow = allOffers.Select(o => o.Store).Distinct().ToList();
            var ranking = new List<dynamic>();

            foreach (var sklep in nazwySklepow)
            {
                decimal sumaDoZaplaty = 0;
                decimal kosztyDodatkowe = sklep.GetAdditionalCost();
                bool czyMaWszystko = true;
                List<string> szczegolyWyboru = new List<string>();

                foreach (var poszukiwany in myNeed)
                {
                    var ofertyWSklepie = allOffers
                        .Where(o => o.Store == sklep && o.Product.Name.Contains(poszukiwany))
                        .ToList();

                    if (ofertyWSklepie.Any())
                    {

                        var najlepszaOpcja = ofertyWSklepie.OrderBy(o => o.UnitPrice).First();

                        sumaDoZaplaty += najlepszaOpcja.Price;

                        szczegolyWyboru.Add(
                            $"- {poszukiwany}: Wybrano '{najlepszaOpcja.Product.Name}' " +
                            $"(Paczka: {najlepszaOpcja.Product.Quantity}{najlepszaOpcja.Product.UnitName}). " +
                            $"Cena: {najlepszaOpcja.Price}zł -> WYCHODZI {najlepszaOpcja.UnitPrice:F2} zł/{najlepszaOpcja.Product.UnitName}"
                        );
                    }
                    else
                    {
                        czyMaWszystko = false;
                    }
                }

                if (czyMaWszystko)
                {
                    ranking.Add(new { Sklep = sklep, Suma = sumaDoZaplaty + kosztyDodatkowe, Detale = szczegolyWyboru });
                }
            }

            // Wyświetlanie
            var posortowanyRanking = ranking.OrderBy(r => r.Suma).ToList();

            if (posortowanyRanking.Count == 0) Console.WriteLine("Brak sklepu z pełną ofertą.");
            else
            {
                foreach (var wynik in posortowanyRanking)
                {
                    Console.WriteLine($"\nSKLEP: {wynik.Sklep.Name}");
                    foreach (string linia in wynik.Detale) Console.WriteLine("  " + linia);

                    decimal dostawa = wynik.Sklep.GetAdditionalCost();
                    if (dostawa > 0) Console.WriteLine($"  + Dostawa: {dostawa} zł");

                    Console.WriteLine($"  = RAZEM DO ZAPŁATY: {wynik.Suma:F2} zł");
                }
            }
        }

        static void SeedData(List<Offer> offers)
        {
            try
            {
                var biedronka = new LocalStore("Biedronka");
                var lidl = new LocalStore("Lidl");

                // --- SCENARIUSZ: KURCZAK (XXL vs EKO) ---

                // BIEDRONKA: Ma paczkę XXL. Cena przy kasie wysoka (25zł), ale waga duża (1.5kg).
                // Cena jednostkowa: 16.66 zł/kg
                var kurczakXXL = new Product("Kurczak XXL Pack", 1.5m, "kg");
                offers.Add(new Offer(kurczakXXL, biedronka, 25.00m));

                // BIEDRONKA: Ma też małą paczkę. Cena niska (12zł), ale waga mała (0.5kg).
                // Cena jednostkowa: 24.00 zł/kg
                var kurczakMaly = new Product("Kurczak Mały", 0.5m, "kg");
                offers.Add(new Offer(kurczakMaly, biedronka, 12.00m));


                // LIDL: Ma wersję Eko. Cena średnia (18zł), waga standard (0.8kg).
                // Cena jednostkowa: 22.50 zł/kg
                var kurczakEko = new Product("Kurczak Eko", 0.8m, "kg");
                offers.Add(new Offer(kurczakEko, lidl, 18.00m));

                // --- INNE PRODUKTY ---
                var mleko = new Product("Mleko", 1.0m, "l");
                offers.Add(new Offer(mleko, biedronka, 3.20m));
                offers.Add(new Offer(mleko, lidl, 3.20m));

                DataManager.SaveOffers(offers);
            }
            catch (Exception ex) { Console.WriteLine("Błąd: " + ex.Message); }
        }

        static void DodajProduktDoListy(List<Offer> offers, List<string> myList)
        {
            Console.Write("\nCo chcesz kupić? (np. Kurczak, Mleko): ");
            string input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input)) myList.Add(input);
        }

        static void PokazBaze(List<Offer> offers)
        {
            Console.WriteLine("\n--- BAZA OFERT ---");
            foreach (var o in offers)
            {
                Console.WriteLine($"{o.Product.Name} ({o.Product.Quantity}{o.Product.UnitName}) " +
                                  $"w {o.Store.Name}: {o.Price} zł " +
                                  $"[Jednostkowo: {o.UnitPrice:F2} zł/{o.Product.UnitName}]");
            }
        }
    }
}