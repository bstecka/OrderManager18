using OrderManager.Domain.Entity;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Service
{
    interface IStockService : IEntityServiceBase<Stock>
    {
        List<CounterpartysStock> GetStocksCounterpartysStock(Stock stock, IMapperBase<Entity.CounterpartysStock> counterpartysStockMapper);
        List<Order> GetStocksActiveOrders(Stock stock, IMapperBase<Entity.Order> orderMapper);
    }
}
