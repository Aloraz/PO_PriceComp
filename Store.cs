using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace projektPO
{
    [JsonDerivedType(typeof(LocalStore), typeDiscriminator: "local")]
    [JsonDerivedType(typeof(OnlineStore), typeDiscriminator: "online")]
    public abstract class Store
    {
        private string _name;

        public string Name
        {
            get => _name;
            protected set
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
            Name = string.Empty;
        }

        public abstract decimal GetAdditionalCost();
    }
}
