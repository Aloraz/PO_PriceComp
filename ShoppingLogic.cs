using System;
using System.Collections.Generic;
using System.Linq;

namespace projektPO
{
    public class ShoppingLogic
    {
        public class BasketResult
        {
            public Store Store { get; set; }
            public decimal Sum { get; set; }
            public List<string> MissingProducts { get; set; } = new List<string>();
            public decimal DeliveryCost { get; set; }
        }

        public static List<BasketResult> CalculateBasket(List<Offer> allOffers, List<string> shoppingList)
        {
            var stores = allOffers.Select(o => o.Store).Distinct().ToList();
            var results = new List<BasketResult>();

            foreach (var store in stores)
            {
                decimal totalSum = 0;
                decimal additionalCost = store.GetAdditionalCost();
                List<string> missing = new List<string>();

                foreach (var neededProduct in shoppingList)
                {
                    var offersInStore = allOffers
                        .Where(o => o.Store.Equals(store) && o.Product.Name.Contains(neededProduct, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (offersInStore.Any())
                    {
                        var cheapestOption = offersInStore.OrderBy(o => o.Price).First();
                        totalSum += cheapestOption.Price;
                    }
                    else
                    {
                        missing.Add(neededProduct);
                    }
                }

                results.Add(new BasketResult
                {
                    Store = store,
                    Sum = totalSum + additionalCost,
                    DeliveryCost = additionalCost,
                    MissingProducts = missing
                });
            }

            return results.OrderBy(r => r.Sum).ToList();
        }

        public static List<Offer> FindDeals(List<Offer> offers, string query)
        {
            return offers
                .Where(o => o.Product.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .OrderBy(o => o.UnitPrice)
                .ToList();
        }
    }
}
