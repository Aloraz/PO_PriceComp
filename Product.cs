using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace PriceComp.GUI.Models
{
    using System;

    
        public class Product : IEquatable<Product>, ICloneable
        {
            private string _name;

            [Key]
            public int ProductID { get; set; }
        public string Name
                {
                    get => _name;
                    set
                    {
                        if (string.IsNullOrWhiteSpace(value))
                            throw new ArgumentException("Nazwa produktu nie może być pusta.");
                        _name = value;
                    }
                }

                public decimal Quantity { get; set; }

                public string UnitName { get; set; }

                public Product(string name, decimal quantity = 1.0m, string unitName = "szt/kg")
                {
                    Name = name;

                    if (quantity <= 0) throw new ArgumentException("Ilość musi być dodatnia");
                    Quantity = quantity;
                    UnitName = unitName;
                }
            
            public Product()
            {
                _name = "Nowy Produkt";
                Quantity = 1;
                UnitName = "szt";
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
