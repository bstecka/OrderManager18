using OrderManager.Domain.Entity;
using System.Collections.Generic;

namespace OrderManager.Domain.OrderGenerator
{
    interface IOrdersGenerator
    {
        List<Order> Generate(Dictionary<Stock, int> stockToORder);
    }
}
