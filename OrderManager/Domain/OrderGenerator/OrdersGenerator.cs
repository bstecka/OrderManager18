using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain
{
    class OrdersGenerator : IOrdersGenerator
    {
        private Dictionary<Stock, int> stockToOrder;

        public OrdersGenerator(Dictionary<Stock, int> stockToOrder)
        {
            this.stockToOrder = stockToOrder;
        }

        public List<Order> Generate(IPriorityService priorityService)
        {
            List<IOffersChoice> strategies = new List<IOffersChoice>();
            List<Tranche> tranches = new List<Tranche>();

            foreach (Priority priority in Enum.GetValues(typeof(Priority)))
                strategies.Add(priority.Strategy(
                    stockToOrder.Where(tuple =>
                    priorityService.GetStockPriority(tuple.Key, new DTO.StockMapper()).First()
                    .Equals(priority)).ToDictionary(i => i.Key, i => i.Value)));
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
            Dictionary<Stock, int> stockToOrder)
        {
            switch (priority)
            {
                case Priority.Price: return new PriorityPriceChoice(stockToOrder);
                case Priority.Distance: return null;
                case Priority.Frequency: return null;
                default: throw new ArgumentException();
            }
        }

        public static Dictionary<Stock, int> StockWithHighestPriority(this Priority priority,
            Dictionary<Stock, int> stockToOrder)
        {
            return null;
        }
    }
}
