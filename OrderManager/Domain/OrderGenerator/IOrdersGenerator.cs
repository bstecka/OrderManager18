using OrderManager.Domain.Entity;
using System.Collections.Generic;

namespace OrderManager.Domain
{
    public interface IOrdersGenerator
    {
        List<Order> Generate(Dictionary<Stock, int> stockToOrder);
    }
}
