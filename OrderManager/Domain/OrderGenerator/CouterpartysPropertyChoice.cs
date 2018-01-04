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
        protected Dictionary<Stock, int> stockToOrder;
        protected ICounterpartyService counterpartyService;
        protected ICounterpartysStockService counterpartysStockService;
        protected IStockService stockService;

        protected CouterpartysPropertyChoice(Dictionary<Stock, int> stockToOrder, 
            ICounterpartyService counterpartyService, 
            ICounterpartysStockService counterpartysStockService, IStockService stockService)
        {
            this.stockToOrder = stockToOrder;
            this.counterpartyService = counterpartyService;
            this.counterpartysStockService = counterpartysStockService;
            this.stockService = stockService;
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
                    counterpartyService.GetCounterpartysStock(
                    sortedCounterparties.FirstOrDefault(counterparty => counterpartyService.GetCounterpartysStock(counterparty)
                    .Select(counterpartysStock => counterpartysStock.Stock)
                    .Contains(stock)))
                    .FirstOrDefault(counterpartysStock => counterpartysStock.Stock.Equals(stock)),
                    stockToOrder[currentCounterpartysStock.Stock]);

            return (new DiscountCounter(bestChosenCounterparties, counterpartysStockService, stockService))
                .BestChosenDiscounts();
        }
    }
}
