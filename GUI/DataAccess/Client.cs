using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PriceComp.GUI.DataAccess
{
    public class Client
    {
        [Key]
        public int ClientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string? ApartmentNumber { get; set; }
        public string CreditCardNumber { get; set; }
        public Client(string firstName, string lastName, string phoneNumber, string street, string houseNumber, string? apartmentNumber, string creditCardNumber)
        {
            FirstName = firstName; LastName = lastName; PhoneNumber = phoneNumber; Street = street; HouseNumber = houseNumber; ApartmentNumber = apartmentNumber; CreditCardNumber = creditCardNumber;
        }
    }
}
