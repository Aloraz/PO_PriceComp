using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using projektPO;
using projektPO.projektPO;
using System.IO;

namespace PriceComp.GUI;

public partial class MainWindow : Window
{
    private List<Offer> _allOffers = new List<Offer>();

    public MainWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        // DataManager szuka "oferty.json". 
        // W GUI musimy upewnić się, że patrzymy w dobre miejsce.
        // Dla uproszczenia ustawimy CurrentDirectory tam gdzie jest plik (o jeden poziom wyżej niż bin/Debug/net8.0-windows)
        // A najprościej: po prostu załadujmy i zobaczmy.
        // Jeśli plik jest w głównym folderze projektu (..), to skopiujmy go tu dla testu, albo wskażmy ścieżkę.
        
        _allOffers = DataManager.LoadOffers();
        
        // AUTO-SEEDER: Jeśli brak danych (lub pusta lista), wygeneruj je używając logiki projektu
        if (_allOffers.Count == 0)
        {
             // Wywołujemy metodę Initialize z głównego projektu (linkowaną)
             projektPO.DataSeeder.Initialize(_allOffers);
             
             // Opcjonalnie zapisujemy, by przy kolejnym uruchomieniu plik już był
             // DataManager.SaveOffers(_allOffers); // zakomentowane, by nie śmiecić w katalogu bin GUI bez potrzeby, lub odkomentuj jeśli chcesz trwałości
        }

        GridOffers.ItemsSource = _allOffers;
        
        // Lista unikalnych produktów do podpowiedzi dla użytkownika (Tab 2)
        var products = _allOffers.Select(o => o.Product.Name).Distinct().OrderBy(n => n).ToList();
        if (ListProducts != null) 
        {
            ListProducts.ItemsSource = products;
        }

        if (_allOffers.Count == 0)
        {
             MessageBox.Show("Nie udało się wygenerować danych.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
        UpdateStoresCombo();
    }

    private void UpdateStoresCombo()
    {
        if (ComboStores == null) return;
        // Pobieramy unikalne obiekty sklepów (dzięki Equals działa Distinct)
        var stores = _allOffers.Select(o => o.Store).Distinct().ToList();
        ComboStores.ItemsSource = stores;
        if (stores.Count > 0) ComboStores.SelectedIndex = 0;
    }

    private void BtnLoad_Click(object sender, RoutedEventArgs e)
    {
        LoadData();
    }

    private void BtnCalculate_Click(object sender, RoutedEventArgs e)
    {
        string input = TxtShoppingList.Text;
        if (string.IsNullOrWhiteSpace(input))
        {
            MessageBox.Show("Podaj listę zakupów!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        // Rozdzielanie po przecinku lub nowej linii
        var shoppingList = input.Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => s.Trim())
                                .Where(s => !string.IsNullOrEmpty(s))
                                .ToList();

        if (shoppingList.Count == 0) return;

        var results = ShoppingLogic.CalculateBasket(_allOffers, shoppingList);

        ListResults.ItemsSource = results;
    }

    private void ListProducts_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (ListProducts.SelectedItem is string selectedProduct)
        {
            string currentText = TxtShoppingList.Text.Trim();
            
            if (string.IsNullOrEmpty(currentText))
            {
                TxtShoppingList.Text = selectedProduct;
            }
            else
            {
                // Sprawdź czy ostatni znak to przecinek, jeśli nie to dodaj
                if (!currentText.EndsWith(","))
                {
                    currentText += ", ";
                }
                else
                {
                     currentText += " ";
                }
                TxtShoppingList.Text = currentText + selectedProduct;
            }
        }
    }

    private void BtnAddOffer_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var store = ComboStores.SelectedItem as Store;
            if (store == null) { MessageBox.Show("Wybierz sklep!"); return; }

            string name = TxtNewName.Text;
            if (string.IsNullOrWhiteSpace(name)) { MessageBox.Show("Podaj nazwę produktu!"); return; }

            if (!decimal.TryParse(TxtNewPrice.Text, out decimal price)) { MessageBox.Show("Błędna cena!"); return; }
            if (!decimal.TryParse(TxtNewQty.Text, out decimal qty)) qty = 1;

            string unit = ComboUnit.Text;

            var product = new Product(name, qty, unit);
            var offer = new Offer(product, store, price);

            _allOffers.Add(offer);

            // Odśwież widoki
            GridOffers.Items.Refresh(); // lub Reset Bindings
            // Odśwież listę produktów
            var products = _allOffers.Select(o => o.Product.Name).Distinct().OrderBy(n => n).ToList();
            ListProducts.ItemsSource = products;

            MessageBox.Show("Dodano nową ofertę!", "Sukces");
            
            // Wyczyść pola
            TxtNewName.Clear();
            TxtNewPrice.Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd: {ex.Message}", "Błąd");
        }
    }

    private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
    {
        DataManager.SaveOffers(_allOffers);
        MessageBox.Show("Zapisano zmiany do pliku oferty.json", "Zapisano");
    }
}