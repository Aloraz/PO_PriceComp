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
using PriceComp.GUI.Models;


using System.IO;
using PriceComp.GUI.Database;
using System.Data.Entity;

namespace PriceComp.GUI;

public partial class MainWindow : Window
{
    private List<Offer> _allOffers = new List<Offer>();
    private ShoppingLogic.BasketResult _selectedBasket;
    private Border _selectedOfferBorder;

    private Border _lastSelectedBorder = null; 
    private readonly SolidColorBrush BaseBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#555"));
    public string _selectedPrice;

    public MainWindow()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        
        try
        {
            using (var context = new PriceComp.GUI.Database.PriceCompContext())
            {
                context.Database.CreateIfNotExists();
                context.SeedIfNotExists();
                
                
                 
                 _allOffers = context.Offers
                                     .Include("Store")
                                     .Include("Product")
                                     .ToList();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Błąd bazy danych: {ex.Message}\nInner: {ex.InnerException?.Message}\n{ex.StackTrace}");
        }
        
        
        
        UpdateGrid();
        UpdateProductsList();
        UpdateStoresCombo();
        
        if (_allOffers.Count == 0)
        {
             MessageBox.Show("Baza danych jest pusta, mimo próby seedowania.", "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        var basket = clicked.DataContext as ShoppingLogic.BasketResult;
        if (basket != null) _selectedBasket = basket; 
       
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
      // Using selected basket offers
      var offers = _selectedBasket?.SelectedOffers ?? new List<Offer>();

      Payment paymentWindow = new Payment(_selectedPrice, offers);
      paymentWindow.Show();
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
        SoundGenerator.PlaySuccess();
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

    
    public delegate bool ValidationDelegate(string input, out string errorMessage);

    private void BtnAddOffer_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var store = ComboStores.SelectedItem as Store;
            if (store == null) { MessageBox.Show("Wybierz sklep!"); return; }

            ValidationDelegate validateName = (string input, out string error) => 
            {
                if (string.IsNullOrWhiteSpace(input))
                {
                    error = "Podaj nazwę produktu!";
                    return false;
                }
                error = null;
                return true;
            };

            ValidationDelegate validatePrice = (string input, out string error) =>
            {
                if (!decimal.TryParse(input, out _))
                {
                    error = "Błędna cena! Wpisz liczbę.";
                    return false;
                }
                error = null;
                return true;
            };

         
            if (!validateName(TxtNewName.Text, out string nameError)) 
            {
                 MessageBox.Show(nameError);
                 return; 
            }

            if (!validatePrice(TxtNewPrice.Text, out string priceError)) 
            { 
                MessageBox.Show(priceError); 
                return; 
            }

            
            string name = TxtNewName.Text;
            decimal price = decimal.Parse(TxtNewPrice.Text);
            if (!decimal.TryParse(TxtNewQty.Text, out decimal qty)) qty = 1;

            string unit = ComboUnit.Text;

            var product = new Product(name, qty, unit);
            var offer = new Offer(product, store, price);

            _allOffers.Add(offer);

            UpdateGrid();
            UpdateProductsList();

            SoundGenerator.PlaySound(600, 150, 0.5); 
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
        SoundGenerator.PlaySound(500, 300, 0.4, true); 
        MessageBox.Show("Zapisano zmiany do pliku oferty.json", "Zapisano");
    }
}