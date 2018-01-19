using OrderManager.Domain.Entity;
using System;
using System.Collections.Generic;

namespace OrderManager.Domain.OrderGenerator
{
    interface IOffersChoice
    {
        List<Tranche> BestChosenOffers();
    }
}
