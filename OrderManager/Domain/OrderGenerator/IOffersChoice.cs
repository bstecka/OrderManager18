using OrderManager.Domain.Entity;
using System.Collections.Generic;

namespace OrderManager.Domain.Service
{
    internal interface IOffersChoice
    {
        List<Tranche> BestChosenOfferts();
    }
}
