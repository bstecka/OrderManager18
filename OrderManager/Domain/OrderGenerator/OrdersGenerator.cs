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
        private IEligibleOrdersNamesService eligibleOrdersNamesService;

        public OrdersGenerator(Dictionary<Stock, int> stockToOrder, ICounterpartyService counterpartyService, 
            IPriorityService priorityService, ICounterpartysStockService counterpartysStockService,
            IStockService stockService, IEligibleOrdersNamesService eligibleOrdersNamesService)
        {
            this.stockToOrder = stockToOrder;
            this.counterpartyService = counterpartyService;
            this.priorityService = priorityService;
            this.counterpartysStockService = counterpartysStockService;
            this.stockService = stockService;
            this.eligibleOrdersNamesService = eligibleOrdersNamesService;
        }

        public List<Order> Generate()
        {
            List<IOffersChoice> strategies = new List<IOffersChoice>();
            List<Tranche> tranches = new List<Tranche>();

            foreach (Priority priority in Enum.GetValues(typeof(Priority)))
            {
                foreach(var st in stockToOrder)
                {
                    var prio = priorityService.GetStockPriority(st.Key).First();
                    bool eq = prio.Equals(priority);
                }

                var stockToStartegy = stockToOrder.Where(tuple =>
                    priorityService.GetStockPriority(tuple.Key).First()
                    .Equals(priority));
                var dictinary = stockToStartegy.ToDictionary(i => i.Key, i => i.Value);
                strategies.Add(GetStrategy(priority, dictinary));
            }
            foreach (var strategy in strategies)
                tranches.AddRange(strategy.BestChosenOffers());

            return groupTranchesInOrders(tranches);
        }

        private List<Order> groupTranchesInOrders(List<Tranche> tranches)
        {
            List<Order> orders = new List<Order>();
            if(tranches != null || !(tranches.Count == 0))
            {
                List<string> eligibleNames = eligibleOrdersNamesService.FetchNames(
                    tranches.GroupBy(tranche => tranche.Stock.Counterparty)
                    .Select(counterparty => counterparty.First()).Count());
                int numberOfName = 0;
                foreach (var counterparty in tranches.ToLookup(tranche => tranche.Stock.Counterparty, tranche => tranche))
                {
                    List<Tranche> tranchesInOrder = new List<Tranche>();
                    foreach (Tranche trancheInOrder in counterparty)
                        tranchesInOrder.Add(trancheInOrder);
                    orders.Add(new Order(null, eligibleNames[numberOfName], counterparty.Key, DateTime.Now, ORDERSTATE.duringRealization, LoggedUser.User, tranchesInOrder));
                    numberOfName++;
                }
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
