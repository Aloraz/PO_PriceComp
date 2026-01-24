using System;
using System.Collections.Generic;
using System.Linq;
using PriceComp.GUI;

namespace projektPO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Offer> allOffers = DataManager.LoadOffers();

            if (allOffers.Count == 0)
            {
                DataSeeder.Initialize(allOffers);
            }

            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== ASYSTENT ZAKUPOWY 2.0 (Clean Code) ===");
                Console.WriteLine("1. Pokaż wszystkie oferty");
                Console.WriteLine("2. SPRAWDŹ OKAZJE DLA PRODUKTU (Tabela)");
                Console.WriteLine("3. OBLICZ KOSZYK (Gdzie najtaniej za wszystko?)");
                Console.WriteLine("4. Zapisz i Wyjdź");
                Console.WriteLine("5. DODAJ NOWĄ OFERTĘ (Ręcznie)");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("6. RESET BAZY DANYCH");
                Console.ResetColor();
                Console.Write("\nWybór: ");

                var key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case '1': PokazBaze(allOffers); Console.ReadKey(); break;
                    case '2': WyszukajOkazje(allOffers); break;
                    case '3': ObliczKoszyk(allOffers); break;
                    case '4': DataManager.SaveOffers(allOffers); running = false; break;
                    case '5': DodajOferte(allOffers); break;
                    case '6': ResetujBaze(allOffers); break;
                }
            }
        }

        static void ObliczKoszyk(List<Offer> allOffers)
        {
            List<string> myNeed = new List<string>();
            Console.WriteLine("\n\n--- KREATOR LISTY ZAKUPÓW ---");
            Console.WriteLine("Wpisuj produkty po kolei (np. Cola, Chipsy).");
            Console.WriteLine("Wciśnij ENTER na pustym polu, aby zakończyć.");

            while (true)
            {
                Console.Write("Dodaj produkt: ");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input)) break;
                myNeed.Add(input);
            }

            if (myNeed.Count == 0) return;

            Console.WriteLine("\n--- RANKING SKLEPÓW ---");

            var nazwySklepow = allOffers.Select(o => o.Store).Distinct().ToList();
            var ranking = new List<dynamic>();

            foreach (var sklep in nazwySklepow)
            {
                decimal sumaParagonu = 0;
                decimal kosztyDodatkowe = sklep.GetAdditionalCost();
                List<string> braki = new List<string>();

                foreach (var poszukiwanyProdukt in myNeed)
                {
                    var ofertyWSklepie = allOffers
                        .Where(o => o.Store == sklep && o.Product.Name.Contains(poszukiwanyProdukt, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (ofertyWSklepie.Any())
                    {
                        var najtanszaOpcja = ofertyWSklepie.OrderBy(o => o.Price).First();
                        sumaParagonu += najtanszaOpcja.Price;
                    }
                    else
                    {
                        braki.Add(poszukiwanyProdukt);
                    }
                }

                ranking.Add(new
                {
                    Sklep = sklep,
                    Suma = sumaParagonu + kosztyDodatkowe,
                    Braki = braki,
                    KosztDostawy = kosztyDodatkowe
                });
            }

            var sklepyKompletne = ranking.Where(r => r.Braki.Count == 0).OrderBy(r => r.Suma).ToList();
            var sklepyNiekompletne = ranking.Where(r => r.Braki.Count > 0).ToList();

            if (sklepyKompletne.Count == 0)
            {
                Console.WriteLine("Żaden sklep nie ma wszystkiego z listy!");
            }
            else
            {
                int miejsce = 1;
                foreach (var wynik in sklepyKompletne)
                {
                    Console.WriteLine($"\nMIEJSCE {miejsce}: {wynik.Sklep.Name}");
                    Console.WriteLine($"  Produkty: {wynik.Suma - wynik.KosztDostawy:F2} zł");
                    if (wynik.KosztDostawy > 0) Console.WriteLine($"  + Dostawa: {wynik.KosztDostawy:F2} zł");

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"  RAZEM: {wynik.Suma:F2} zł");
                    Console.ResetColor();
                    miejsce++;
                }
            }

            if (sklepyNiekompletne.Count > 0)
            {
                Console.WriteLine("\n--- BRAKI W INNYCH SKLEPACH ---");
                foreach (var wynik in sklepyNiekompletne)
                {
                    Console.WriteLine($"{wynik.Sklep.Name}: Brakuje [{string.Join(", ", wynik.Braki)}]");
                }
            }
            Console.WriteLine("\nNaciśnij dowolny klawisz...");
            Console.ReadKey();
        }

        static void WyszukajOkazje(List<Offer> offers)
        {
            Console.Write("\n\nJaki produkt Cię interesuje? (np. Cola, Chipsy): ");
            string szukanaNazwa = Console.ReadLine();

            var pasujaceOferty = offers
                .Where(o => o.Product.Name.Contains(szukanaNazwa, StringComparison.OrdinalIgnoreCase))
                .OrderBy(o => o.UnitPrice) 
                .ToList();

            if (pasujaceOferty.Count == 0)
            {
                Console.WriteLine("Nie znaleziono takiego produktu.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\n--- WYNIKI WYSZUKIWANIA: {szukanaNazwa.ToUpper()} ---");

            Console.WriteLine(
                "{0,-35} | {1,-12} | {2,-10} | {3,-15} | {4,-12} | {5,-25}",
                "PEŁNA NAZWA", "SKLEP", "CENA", "JEDNOSTKOWO", "PROMO", "WARUNEK");

            Console.WriteLine(new string('-', 120)); 

            foreach (var o in pasujaceOferty)
            {
                string nazwa = o.Product.Name;
                if (nazwa.Length > 32) nazwa = nazwa.Substring(0, 29) + "...";

                string cenaStd = $"{o.Price} zł";

                string unitInfo = $"{o.UnitPrice:F2} zł/{o.Product.UnitName}";

                string cenaPromo = o.PromoPrice.HasValue ? $"{o.PromoPrice} zł" : "";
                string opisPromo = o.PromoDescription ?? "";

                if (o.PromoPrice.HasValue) Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine(
                    "{0,-35} | {1,-12} | {2,-10} | {3,-15} | {4,-12} | {5,-25}",
                    nazwa,
                    o.Store.Name,
                    cenaStd,
                    unitInfo, 
                    cenaPromo,
                    opisPromo);

                Console.ResetColor();
            }
            Console.WriteLine(new string('-', 120));
            Console.ReadKey();
        }
      
        static void PokazBaze(List<Offer> offers)
        {
            Console.WriteLine("\n--- BAZA OFERT ---");
            foreach (var o in offers)
            {
                Console.WriteLine($"{o.Product.Name} w {o.Store.Name}: {o.Price} zł");
            }
        }

        static void ResetujBaze(List<Offer> offers)
        {
            Console.Write("\n\nCzy na pewno chcesz usunąć bazę i przywrócić dane testowe? (t/n): ");
            var decyzja = Console.ReadKey().KeyChar;

            if (decyzja == 't' || decyzja == 'T')
            {
                try
                {
                    // 1. Usuwamy plik fizyczny
                    string sciezka = "oferty.json"; 
                    if (File.Exists(sciezka))
                    {
                        File.Delete(sciezka);
                        Console.WriteLine($"\n[INFO] Plik {sciezka} został usunięty.");
                    }

                    // 2. Czyścimy pamięć programu
                    offers.Clear();

                    // 3. Generujemy nowe dane
                    DataSeeder.Initialize(offers);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n[SUKCES] Baza została zresetowana do ustawień początkowych!");
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n[BŁĄD] Nie udało się zresetować bazy: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("\nAnulowano.");
            }
            Console.ReadKey();
        }

        static void DodajOferte(List<Offer> offers)
        {
            Console.WriteLine("\n\n--- DODAWANIE NOWEJ OFERTY ---");

            // 1. Wybór sklepu (z istniejących)
            // Pobieramy unikalne sklepy z listy, żeby użytkownik mógł wybrać
            var dostepneSklepy = offers.Select(o => o.Store).Distinct().ToList();

            Console.WriteLine("Wybierz sklep:");
            for (int i = 0; i < dostepneSklepy.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {dostepneSklepy[i].Name}");
            }

            Console.Write("Numer sklepu: ");
            if (!int.TryParse(Console.ReadLine(), out int sklepIndex) || sklepIndex < 1 || sklepIndex > dostepneSklepy.Count)
            {
                Console.WriteLine("Niepoprawny wybór sklepu!");
                Console.ReadKey();
                return;
            }
            var wybranySklep = dostepneSklepy[sklepIndex - 1];

            // 2. Dane produktu
            Console.Write("Nazwa produktu (np. Chleb): ");
            string nazwa = Console.ReadLine();

            Console.Write("Cena (np. 4,50): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal cena))
            {
                Console.WriteLine("To nie jest liczba!");
                Console.ReadKey();
                return;
            }

            Console.Write("Ilość/Waga w opakowaniu (np. 1 dla sztuki, 0,5 dla 500g): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal ilosc)) ilosc = 1.0m;

            Console.Write("Jednostka (szt, kg, l): ");
            string jednostka = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(jednostka)) jednostka = "szt";

            // 3. Tworzenie obiektów
            try
            {
                var nowyProdukt = new Product(nazwa, ilosc, jednostka);
                var nowaOferta = new Offer(nowyProdukt, wybranySklep, cena);

                // 4. Dodanie do listy
                offers.Add(nowaOferta);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[SUKCES] Oferta dodana! Pamiętaj zapisać zmiany przy wyjściu (Opcja 4).");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}