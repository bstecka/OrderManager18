using OrderManager.Domain.Entity;
using OrderManager.Domain.Service;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.OrderGenerator
{
    class PriorityFrequencyChoice : CouterpartysPropertyChoice
    {
        public PriorityFrequencyChoice(Dictionary<Stock, int> stockToOrder, ICounterpartyService counterpartyService) : base(stockToOrder, counterpartyService) { }

        public override List<Counterparty> SortCounterparties()
        {
            return counterpartyService.GetAll();
                //.OrderBy(counterparty => counterparty).ToList();
        }
    }
}
