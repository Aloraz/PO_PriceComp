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
using PriceComp.GUI.Models;
using PriceComp.GUI.Database;

namespace PriceComp.GUI
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : Window
    {
        
        private List<Offer> _offersToBuy;

        public Payment(string price, List<Offer> offers)
        {
            InitializeComponent();
            TxtTotalPrice.Text = $"{price} PLN";
            _offersToBuy = offers;
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtFirstName.Text) || string.IsNullOrWhiteSpace(TxtLastName.Text) || string.IsNullOrWhiteSpace(TxtStreet.Text))
            {
                MessageBox.Show("Uzupełnij wymagane pola!");
                return;
            }

            try
            {
                using (var context = new PriceCompContext())
                {
                    // 1. Create Client
                    var client = new Client
                    {
                        FirstName = TxtFirstName.Text,
                        LastName = TxtLastName.Text,
                        PhoneNumber = TxtPhone.Text,
                        Street = TxtStreet.Text,
                        HouseNumber = TxtHouseNum.Text,
                        ApartmentNumber = TxtApartmentNum.Text,
                        EncryptedCardNumber = TxtCardNumber.Text.Replace("-", "") 
                    };
                    context.Clients.Add(client);

                    // 2. Create Order
                    var order = new Order
                    {
                        Client = client,
                        OrderDate = DateTime.Now
                    };
                    context.Orders.Add(order);

                    // 3. Group offers to calculate Quantity
                    var groupedItems = _offersToBuy
                        .GroupBy(o => o.OfferID)
                        .Select(g => new 
                        { 
                            OfferID = g.Key, 
                            Quantity = g.Count() 
                        })
                        .ToList();

                    // 4. Create OrderDetails
                    foreach (var item in groupedItems)
                    {
                        var detail = new OrderDetails
                        {
                            Order = order,
                            OfferID = item.OfferID,
                            Quantity = item.Quantity
                        };
                        context.OrderDetails.Add(detail);
                    }

                    // 5. Save all changes (Transaction)
                    context.SaveChanges();
                }

                MessageBox.Show("Dziękujemy za zamówienie! Zapisano w bazie.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd zapisu: {ex.Message}\n{ex.InnerException?.Message}");
            }
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
