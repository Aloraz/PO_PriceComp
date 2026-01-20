using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektPO
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

        public override decimal GetAdditionalCost()
        {
            return DeliveryCost;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj) && 
                   obj is OnlineStore other && 
                   DeliveryCost == other.DeliveryCost;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), DeliveryCost);
        }
    }
}
