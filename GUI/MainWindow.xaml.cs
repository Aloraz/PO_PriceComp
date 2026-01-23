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

using System.IO;

namespace PriceComp.GUI;

public partial class MainWindow : Window
{
    private List<Offer> _allOffers = new List<Offer>();
    private Border _selectedOfferBorder;

    private Border _lastSelectedBorder = null; 
    private readonly SolidColorBrush BaseBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555"));
    public string _selectedPrice;

    public string _selectedStore;
    public List<Product> _selectedProducts= new List<Product>();

    public MainWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        _allOffers = DataManager.LoadOffers();
        
        if (_allOffers.Count == 0)
        {
             projektPO.DataSeeder.Initialize(_allOffers);
        }

        UpdateGrid();
        UpdateProductsList();
        UpdateStoresCombo();
        
        if (_allOffers.Count == 0)
        {
             MessageBox.Show("Nie udało się wygenerować danych.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void UpdateProductsList()
    {
        if (ListProducts == null) return;
        var products = _allOffers.Select(o => o.Product.Name).Distinct().OrderBy(n => n).ToList();
        ListProducts.ItemsSource = products;
    }


    private void UpdateStoresCombo()
    {
        if (ComboStores == null) return;
        var stores = _allOffers.Select(o => o.Store).Distinct().ToList();
        ComboStores.ItemsSource = stores;
        if (stores.Count > 0) ComboStores.SelectedIndex = 0;
    }

    private void ChooseOffer(object sender, RoutedEventArgs e)
    {
        var clicked = sender as Border; 
        if (clicked == null) return; 
       
        if (_lastSelectedBorder == clicked && clicked.BorderBrush == Brushes.Blue) 
        {
            clicked.BorderBrush = BaseBrush; 
            _lastSelectedBorder = null; 
            return; 
        } 
        if (_lastSelectedBorder != null) 
        {
            _lastSelectedBorder.BorderBrush = BaseBrush; 
        } 
        clicked.BorderBrush = Brushes.Blue; 
        _lastSelectedBorder = clicked; 
        
        var textboxPrice = clicked.FindName("Price_of_basket") as TextBlock;
        var shop =(clicked.FindName("SelectedStore") as TextBlock);
        _selectedStore = shop.Text;
        MessageBox.Show($"Wybrano sklep: {_selectedStore}", "Sklep wybrany");

        if ((!textboxPrice.Text.StartsWith("0")) && textboxPrice.Text!=null)
        {
            _selectedPrice = textboxPrice.Text;
            BtnGoToPayment2.IsEnabled = true;
           BtnGoToPayment2.Background = Brushes.Green;
        } else
        {
            BtnGoToPayment2.IsEnabled = false;
            
            BtnGoToPayment2.Background= (Brush)new BrushConverter().ConvertFromString("#FFA1A9AE");

        }

    }
    private void BtnGoToPayment(object sender, RoutedEventArgs e)
    {
        try
        {
           BtnCalculate_Click(sender, e);
           //int newPrice=(int) _selectedPrice.Substring(0, _selectedPrice.Length-3);
           MessageBox.Show($"Typeof _selectedPrice: {_selectedPrice.GetType()}");
           Payment paymentWindow = new Payment(_selectedPrice, _selectedStore, _selectedProducts);



            paymentWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd at BtnGoToPayment: {ex.Message}", "Błąd");


        }
    }
    private void BtnCalculate_Click(object sender, RoutedEventArgs e)
    {
        string input = TxtShoppingList.Text;
        if (string.IsNullOrWhiteSpace(input))
        {
            MessageBox.Show("Podaj listę zakupów!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

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
            
            try
            {
                _selectedProducts.Add(new Product(selectedProduct));
                MessageBox.Show($"Dodano {selectedProduct} do listy zakupów.", "Produkt dodany");
            } catch (Exception ex)
            {
                MessageBox.Show($"Błąd: {ex.Message}", "Błąd");
            }
            

            if (string.IsNullOrEmpty(currentText))
            {
                TxtShoppingList.Text = selectedProduct;
                
            }
            else
            {
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

            UpdateGrid();
            UpdateProductsList();

            MessageBox.Show("Dodano nową ofertę! Pamiętaj zapisać zmiany.", "Sukces");
            
            TxtNewName.Clear();
            TxtNewPrice.Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd: {ex.Message}", "Błąd");
        }
    }

    private void UpdateGrid()
    {
        GridOffers.ItemsSource = null;
        GridOffers.ItemsSource = _allOffers;
    }

    private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
    {
        DataManager.SaveOffers(_allOffers);
        MessageBox.Show("Zapisano zmiany do pliku oferty.json", "Zapisano");
    }
}