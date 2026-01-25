using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PriceComp.GUI.Models
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
        public string ApartmentNumber { get; set; }
        
        // In a real application, never store card numbers in plain text!
        public string EncryptedCardNumber { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
