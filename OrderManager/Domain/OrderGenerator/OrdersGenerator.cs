using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using OrderManager.Domain.OrderGenerator;

namespace OrderManager.Domain
{
    class OrdersGenerator : IOrdersGenerator
    {
        private Dictionary<Stock, int> stockToOrder;
        private ICounterpartyService counterpartyService;
        private IPriorityService priorityService;
        private ICounterpartysStockService counterpartysStockService;
        private IStockService stockService;

        public OrdersGenerator(Dictionary<Stock, int> stockToOrder, ICounterpartyService counterpartyService, 
            IPriorityService priorityService, ICounterpartysStockService counterpartysStockService, IStockService stockService)
        {
            this.stockToOrder = stockToOrder;
            this.counterpartyService = counterpartyService;
            this.priorityService = priorityService;
            this.counterpartysStockService = counterpartysStockService;
            this.stockService = stockService;
        }

        public List<Order> Generate()
        {
            List<IOffersChoice> strategies = new List<IOffersChoice>();
            List<Tranche> tranches = new List<Tranche>();

            foreach (Priority priority in Enum.GetValues(typeof(Priority)))
                strategies.Add(GetStrategy(priority, 
                    stockToOrder.Where(tuple =>
                    priorityService.GetStockPriority(tuple.Key).First()
                    .Equals(priority)).ToDictionary(i => i.Key, i => i.Value)));
            foreach (var strategy in strategies)
                tranches.AddRange(strategy.BestChosenOfferts());

            return groupTranchesInOrders(tranches);
        }

        private List<Order> groupTranchesInOrders(List<Tranche> tranches)
        {
            List<Order> orders = new List<Order>();
            foreach(var counterparty in tranches.ToLookup(tranche => tranche.Stock.Counterparty, tranche => tranche))
            {
                List<Tranche> tranchesInOrder = new List<Tranche>();
                foreach (Tranche trancheInOrder in counterparty)
                    tranchesInOrder.Add(trancheInOrder);
                orders.Add(new Order(null, "todo", counterparty.Key, DateTime.Now, "duringRealization", null, tranchesInOrder));
            }
            return orders;
        }

        private IOffersChoice GetStrategy(Priority priority, Dictionary<Stock, int> stockToOrder)
        {
            switch (priority)
            {
                case Priority.Price: return new PriorityPriceChoice(stockToOrder, counterpartysStockService, stockService);
                case Priority.Distance: return new PriorityDistanceChoice(stockToOrder, counterpartyService, counterpartysStockService, stockService);
                case Priority.Frequency: return new PriorityFrequencyChoice(stockToOrder, counterpartyService, counterpartysStockService, stockService);
                default: throw new ArgumentException();
            }
        }
    }

    enum Priority { Price, Frequency, Distance};
    
}
