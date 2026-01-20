using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektPO
{
    public class OfferByUnitComparer : IComparer<Offer>
    {
        public int Compare(Offer x, Offer y)
        {
            if (x == null || y == null) return 0;
            return x.UnitPrice.CompareTo(y.UnitPrice);
        }
    }
}
