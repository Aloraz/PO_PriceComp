using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace dataAccess
{
    public class Client
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int HouseNum { get; set; }
        public int ApartmentNum { get; set; }

        public long CreditCardNum { get; set; }

        [Key]
        public int ClientId { get; set; }

        public Client()
        {
            Name = string.Empty;
            Surname = string.Empty;
            Address = string.Empty;
            PhoneNumber = string.Empty;
            HouseNum = 0;
            ApartmentNum = 0;
            CreditCardNum = 0;
        }

        public Client(string name, string surname, string address, string phoneNumber, int houseNum, int apartmentNum, int creditCardNum)
        {
            Name = name;
            Surname = surname;
            Address = address;
            PhoneNumber = phoneNumber;
            HouseNum = houseNum;
            ApartmentNum = apartmentNum;
            CreditCardNum = creditCardNum;
        }

        public Client(string name, string surname, string address, string phoneNumber, int houseNum, int apartmentNum)
        {
            Name = name;
            Surname = surname;
            Address = address;
            PhoneNumber = phoneNumber;
            HouseNum = houseNum;
            ApartmentNum = apartmentNum;
        }
    }
}
