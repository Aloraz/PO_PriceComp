using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using dataAccess;

namespace projektPO
{
    using System;

    
    public class Product : IEquatable<Product>, ICloneable
    {
        private string _name;

    [Key]
    public int ProductId { get; set; }
    public virtual Order Order { get; set; }

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
            
        public Product()
    {
        Name = string.Empty;
        Quantity= 0;
        UnitName = string.Empty;
    }
        public bool Equals(Product other)
        {
            if (other == null) return false;
            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase) &&
                    Quantity == other.Quantity &&
                    UnitName == other.UnitName;
        }
        public override int GetHashCode() => (Name?.ToLower(), Quantity, UnitName).GetHashCode();
        public object Clone()
        {
            return new Product(this.Name, this.Quantity, this.UnitName);
        }
    }
    
}
