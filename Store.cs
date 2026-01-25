using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace PriceComp.GUI.Models
{
    [JsonDerivedType(typeof(LocalStore), typeDiscriminator: "local")]
    [JsonDerivedType(typeof(OnlineStore), typeDiscriminator: "online")]
    public abstract class Store
    {
        private string _name;

        [Key]
        public int StoreID { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Nazwa sklepu nie może być pusta.");
                _name = value;
            }
        }

        protected Store(string name)
        {
            Name = name;
        }

        protected Store() 
        {
            _name = "Nowy Sklep";
        }

        public abstract decimal GetAdditionalCost();
    }
}
