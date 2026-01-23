using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using projektPO;
using dataAccess;

namespace PriceComp.GUI
{
  
    public partial class Payment : Window
    {
        string _shopName;
        decimal _price;
        List<Product> _orderedProducts = new List<Product>();

        public Payment(string price, string shopName, List<Product> orderedProducts)
        {
            InitializeComponent();
            TxtTotalPrice.Text = $"{price}";

            _shopName = shopName;
            try
            {

                //price = price.Replace(",", ".");
                //price = price.Replace(" PLN", "");
                // decimal value = decimal.Parse(price);


                //MessageBox.Show($"Substring(0, length-3): {price.Substring(0, price.Length - 3)}");
                //price = price.Substring(0, price.Length - 3);
                //_price = decimal.Parse(price);
                price = price.Replace(" zł", "").Replace(",", ".").Replace(" zl", "");


                if (!decimal.TryParse(price, System.Globalization.NumberStyles.Any,
                                      System.Globalization.CultureInfo.InvariantCulture,
                                      out _price))
                {
                    MessageBox.Show($"Nie mogę sparsować ceny: {price}");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd przy parsowaniu ceny: {ex.Message}");
            }
            
            _orderedProducts = orderedProducts;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long card =long.Parse(TxtCardNumber.Text.Replace("-", ""));
                if (card.ToString().Length != 16)
                {
                    MessageBox.Show("Numer karty kredytowej musi zawierać 16 cyfr.");
                    return;
                }
                Client client = new Client
                {
                    Name = TxtFirstName.Text,
                    Surname = TxtLastName.Text,
                    Address = TxtStreet.Text,
                    PhoneNumber = TxtPhone.Text,
                    HouseNum = int.Parse(TxtHouseNum.Text),
                    ApartmentNum = int.Parse(TxtApartmentNum.Text),

                    CreditCardNum = long.Parse(TxtCardNumber.Text.Replace("-", "")),
                };
                Order order = new Order(client, _orderedProducts, _price, new OnlineStore(_shopName, _price));
                order.SaveToDB();

            } catch (FormatException)
            {
                MessageBox.Show("Nieprawidłowy format numeru karty kredytowej.");
                return;
            } catch(Exception ex) {

                MessageBox.Show($"Wystąpił błąd: {ex.Message}");
                return;
            }



            MessageBox.Show("Dziękujemy za zamówienie!");
            this.Close();
        }

        private void TxtCardNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox == null) return;

            
            string rawText = textBox.Text.Replace("-", "");

            string cleanText = new string(rawText.Where(char.IsDigit).ToArray());

            StringBuilder formatted = new StringBuilder();

            for (int i = 0; i < cleanText.Length; i++)
            {       
                if (i > 0 && i % 4 == 0)
                {
                    formatted.Append("-");
                }
                formatted.Append(cleanText[i]);
            }
         
            if (textBox.Text != formatted.ToString())
            {
                int cursorPosition = textBox.SelectionStart;
                int oldLength = textBox.Text.Length;

                textBox.Text = formatted.ToString();

                
                int newLength = textBox.Text.Length;
                int selectionIndex = cursorPosition + (newLength - oldLength);

                if (selectionIndex >= 0 && selectionIndex <= textBox.Text.Length)
                {
                    textBox.SelectionStart = selectionIndex;
                }
            }
        }
    }
}
