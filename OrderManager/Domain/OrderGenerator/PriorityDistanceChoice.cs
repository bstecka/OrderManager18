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
        public PriorityDistanceChoice(Dictionary<Stock, int> stockToOrder) : base(stockToOrder) { }

        public override List<Counterparty> SortCounterparties()
        {
            return (new CounterpartyService(new ExternalSysDAO.Counterparty(), 
                new CounterpartyMapper())).GetAll()
                .OrderBy(counterparty => counterparty.Distance).ToList();
        }
    }
}
