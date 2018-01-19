using System;
using System.Collections.Generic;
using System.Linq;
using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;

namespace OrderManager.Domain.OrderGenerator
{
    class PriorityDistanceChoice : CouterpartysPropertyChoice
    {
        public PriorityDistanceChoice(Dictionary<Stock, int> stockToOrder, ICounterpartyService counterpartyService,
            ICounterpartysStockService counterpartysStockService, IStockService stockService) : base(stockToOrder, counterpartyService, counterpartysStockService, stockService) { }

        /// <summary>
        /// Sorts the counterparties by distance.
        /// </summary>
        /// <returns>Returns the list of all counterparties sorted by distance.</returns>
        public override List<Counterparty> SortCounterparties()
        {
            return counterpartyService.GetAll()
                .OrderBy(counterparty => counterparty.Distance).ToList();
        }
    }
}
