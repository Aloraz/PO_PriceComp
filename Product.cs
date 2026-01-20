using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projektPO
{
    using System;

    namespace projektPO
    {
        public class Product
        {
            private string _name;

            public string Name
            {
                get => _name;
                private set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Nazwa produktu nie może być pusta.");
                    _name = value;
                }
            }

            public decimal Quantity { get; private set; }

            public string UnitName { get; private set; }

            public Product(string name, decimal quantity = 1.0m, string unitName = "szt/kg")
            {
                Name = name;

                if (quantity <= 0) throw new ArgumentException("Ilość musi być dodatnia");
                Quantity = quantity;
                UnitName = unitName;
            }
        }
    }
}
