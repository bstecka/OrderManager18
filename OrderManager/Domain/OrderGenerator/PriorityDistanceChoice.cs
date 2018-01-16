using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
using OrderManager.DTO;

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
