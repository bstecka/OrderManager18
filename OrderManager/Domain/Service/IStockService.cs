using OrderManager.Domain.Entity;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Service
{
    public interface IStockService : IEntityServiceBase<Stock>
    {
        List<CounterpartysStock> GetStocksCounterpartysStock(Stock stock);
        List<Order> GetStocksActiveOrders(Stock stock);
        int GetNumOfItemsInOrders(Stock stock);
        int GetNumOfItemsToOrder(Stock stock);
    }
}
