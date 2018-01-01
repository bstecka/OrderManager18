using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManager.Domain.Entity;

namespace OrderManager.Domain.Service
{
    class PriorityPriceChoice : IOffersChoice
    {
        Dictionary<Stock, int> stockToOrder;

        public PriorityPriceChoice(Dictionary<Stock, int> stockToOrder)
        {
            this.stockToOrder = stockToOrder;
        }

        public List<Tranche> BestChosenOfferts()
        {
            throw new NotImplementedException();
        }
    }
}
