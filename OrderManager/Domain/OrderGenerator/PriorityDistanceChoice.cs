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
            ICounterpartysStockService counterpartysStockService, IOrderService stockService) : base(stockToOrder, counterpartyService, counterpartysStockService, stockService) { }

        public override List<Counterparty> SortCounterparties()
        {
            return counterpartyService.GetAll()
                .OrderBy(counterparty => counterparty.Distance).ToList();
        }
    }
}
