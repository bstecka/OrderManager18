using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
using System.Collections.Generic;
using System.Linq;

namespace OrderManager.Domain.OrderGenerator
{
    class PriorityFrequencyChoice : CouterpartysPropertyChoice
    {
        public PriorityFrequencyChoice(Dictionary<Stock, int> stockToOrder, ICounterpartyService counterpartyService,
            ICounterpartysStockService counterpartysStockService, IStockService stockService) : base(stockToOrder, counterpartyService, counterpartysStockService, stockService) { }

        /// <summary>
        /// Sorts the counterparties by the frequency of orders.
        /// </summary>
        /// <returns>Returns the list of all counterparties sorted by the frequency of orders.</returns>
        public override List<Counterparty> SortCounterparties()
        {
            return counterpartyService.GetAll()
                .OrderByDescending(counterparty => counterpartyService.GetCounterpartysOrders(counterparty).Count).ToList();
        }
    }
}
