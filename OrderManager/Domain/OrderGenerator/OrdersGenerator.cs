using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManager.Domain.OrderGenerator;

namespace OrderManager.Domain
{
    class OrdersGenerator : IOrdersGenerator
    {
        private Dictionary<Stock, int> stockToOrder;
        private ICounterpartyService counterpartyService;
        private IPriorityService priorityService;

        public OrdersGenerator(Dictionary<Stock, int> stockToOrder, ICounterpartyService counterpartyService, IPriorityService priorityService)
        {
            this.stockToOrder = stockToOrder;
            this.counterpartyService = counterpartyService;
            this.priorityService = priorityService;
        }

        public List<Order> Generate()
        {
            List<IOffersChoice> strategies = new List<IOffersChoice>();
            List<Tranche> tranches = new List<Tranche>();

            foreach (Priority priority in Enum.GetValues(typeof(Priority)))
                strategies.Add(priority.Strategy(
                    stockToOrder.Where(tuple =>
                    priorityService.GetStockPriority(tuple.Key).First()
                    .Equals(priority)).ToDictionary(i => i.Key, i => i.Value),
                    counterpartyService));
            foreach (var strategy in strategies)
                tranches.AddRange(strategy.BestChosenOfferts());

            return null;
        }

        private List<Stock> StockWithPriority(Priority priority)
        {
            return null;
        }
    }

    enum Priority { Price, Frequency, Distance};

    static class PriorityMethods
    {
        public static IOffersChoice Strategy(this Priority priority, 
            Dictionary<Stock, int> stockToOrder, ICounterpartyService counterpartyService)
        {
            switch (priority)
            {
                case Priority.Price: return new PriorityPriceChoice(stockToOrder);
                case Priority.Distance: return new PriorityDistanceChoice(stockToOrder, counterpartyService);
                case Priority.Frequency: return new PriorityFrequencyChoice(stockToOrder, counterpartyService);
                default: throw new ArgumentException();
            }
        }
    }
}
