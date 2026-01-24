using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

{
    
}

namespace PriceComp.GUI.DataAccess
{
    public class OrderDetails
    {
        public int OrderDetailsID { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
        public int OfferID { get; set; }
        public Offer Offer { get; set; }
        public decimal Quantity { get; set; }
        public ICollection<OrderDetails> Order_Details { get; set; } = new List<OrderDetails>();
        public OrderDetails(decimal quantity)
        {
            Quantity = quantity;
        }
    }
}
