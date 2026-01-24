using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace PriceComp.GUI.DataAccess
{
    public class Order
    {
        [Key]
        public int OrderID  { get; set; }
        public int ClientID { get; set; }
        public Client Client { get; set; }
        DateTime OrderDate { get; set; }

        public Order()
        {
            OrderDate = DateTime.Now;
        }
        public Order(DateTime orderDate)
        {
            OrderDate = orderDate;
        }

    }
}
