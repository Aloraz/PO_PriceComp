using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriceComp.GUI.Models
{
    public class OrderDetails
    {
        [Key, Column(Order = 0)]
        public int OrderID { get; set; }

        [Key, Column(Order = 1)]
        public int OfferID { get; set; }

        public int Quantity { get; set; }

        
        [ForeignKey("OrderID")]
        public virtual Order Order { get; set; }

        [ForeignKey("OfferID")]
        public virtual Offer Offer { get; set; }
    }
}
