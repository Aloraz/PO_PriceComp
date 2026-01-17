using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektPO
{
    public class LocalStore : Store
    {
        public LocalStore(string name) : base(name) { }

        public override decimal GetAdditionalCost()
        {
            return 0;
        }
    }
}
