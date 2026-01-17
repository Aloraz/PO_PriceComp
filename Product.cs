using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Product(string name)
        {
            Name = name;
        }
    }
}
