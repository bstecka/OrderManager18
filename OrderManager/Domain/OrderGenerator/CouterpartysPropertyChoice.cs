using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.OrderGenerator
{
    abstract class CouterpartysPropertyChoice : IOffersChoice
    {
        Dictionary<Stock, int> stockToOrder;

        protected CouterpartysPropertyChoice(Dictionary<Stock, int> stockToOrder)
        {
            this.stockToOrder = stockToOrder;
        }

        public abstract List<Counterparty> SortCounterparties();

        public List<Tranche> BestChosenOfferts()
        {
            if (stockToOrder.Count == 0) return new List<Tranche>();
            List<Counterparty> sortedCounterparties = SortCounterparties();
            Dictionary<CounterpartysStock, int> bestChosenCounterparties = new Dictionary<CounterpartysStock, int>();
            CounterpartysStock currentCounterpartysStock;

            foreach (Stock stock in stockToOrder.Keys)
                bestChosenCounterparties.Add(
                    currentCounterpartysStock = 
                    sortedCounterparties.FirstOrDefault(counterparty => counterparty.Stock
                    .Select(counterpartysStock => counterpartysStock.Stock)
                    .Contains(stock))
                    .Stock.FirstOrDefault(counterpartysStock => counterpartysStock.Stock.Equals(stock)),
                    stockToOrder[currentCounterpartysStock.Stock]);

            return (new DiscountCounter(bestChosenCounterparties)).BestChosenDiscounts();
        }
    }
}
