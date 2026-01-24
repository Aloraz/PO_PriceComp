using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceComp.GUI
{
    public class OnlineStore : Store
    {
        private decimal _deliveryCost;

        public decimal DeliveryCost
        {
            get => _deliveryCost;
            private set
            {
                if (value < 0)
                    throw new ArgumentException("Koszt dostawy nie może być ujemny.");
                _deliveryCost = value;
            }
        }

        public OnlineStore(string name, decimal deliveryCost) : base(name)
        {
            DeliveryCost = deliveryCost;
        }

        public OnlineStore():base()
        {
            DeliveryCost = 0;
        }

        public override decimal GetAdditionalCost()
        {
            return DeliveryCost;
        }
    }
}
