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
            ICounterpartysStockService counterpartysStockService, IStockService stockService) 
            : base(stockToOrder, counterpartyService, counterpartysStockService, stockService) { }

        public override List<Counterparty> SortCounterparties()
        {
            return counterpartyService.GetAll()
                .OrderBy(counterparty => counterparty.Distance).ToList();
        }
    }
}
