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

namespace PriceComp.GUI
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : Window
    {
        
        public Payment(string price)
        {
            InitializeComponent();
            TxtTotalPrice.Text = $"{price} PLN";
        }

        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
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
