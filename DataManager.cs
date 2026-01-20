using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace projektPO
{
    public static class DataManager
    {
        private const string FilePath = "oferty.json";

        public static void SaveOffers(List<Offer> offers)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };

                string jsonString = JsonSerializer.Serialize(offers, options);
                File.WriteAllText(FilePath, jsonString);

                Console.WriteLine($"[INFO] Zapisano {offers.Count} ofert do pliku {FilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BŁĄD ZAPISU] {ex.Message}");
            }
        }

        public static List<Offer> LoadOffers()
        {
            if (!File.Exists(FilePath))
            {
                Console.WriteLine("[INFO] Plik nie istnieje. Zwracam pustą listę.");
                return new List<Offer>();
            }

            try
            {
                string jsonString = File.ReadAllText(FilePath);
                var offers = JsonSerializer.Deserialize<List<Offer>>(jsonString);

                Console.WriteLine($"[INFO] Wczytano {offers?.Count ?? 0} ofert z pliku.");
                return offers ?? new List<Offer>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BŁĄD ODCZYTU] {ex.Message}");
                return new List<Offer>();
            }
        }
    }
}