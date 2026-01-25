using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using PriceComp.GUI.Models;

namespace PriceComp.GUI
{
    public static class DataManager
    {
        private const string FilePath = "oferty.json";

        private static string GetFilePath()
        {
            // Try current directory first
            if (File.Exists(FilePath)) return FilePath;

            
            string rootPath = AppDomain.CurrentDomain.BaseDirectory;
            for (int i = 0; i < 4; i++)
            {
                string checkPath = Path.Combine(rootPath, FilePath);
                if (File.Exists(checkPath)) return checkPath;
                rootPath = Path.GetDirectoryName(rootPath);
                if (rootPath == null) break;
            }

            return FilePath; 
        }

        public static void SaveOffers(List<Offer> offers)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(offers, options);
                
                string targetPath = GetFilePath();
                File.WriteAllText(targetPath, jsonString);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd zapisu JSON: {ex.Message}");
            }
        }

        public static List<Offer> LoadOffers()
        {
            string targetPath = GetFilePath();

            if (!File.Exists(targetPath))
            {
                return new List<Offer>();
            }

            try
            {
                string jsonString = File.ReadAllText(targetPath);
                var offers = JsonSerializer.Deserialize<List<Offer>>(jsonString);
                return offers ?? new List<Offer>();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Błąd odczytu JSON: {ex.Message}");
                return new List<Offer>();
            }
        }
    }
}