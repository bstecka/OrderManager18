using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    interface IStockDAO : IReadableDAO
    {
        DataTable GetStocksCounterpartysStock(DataTable stock);
        DataTable GetStocksActiveOrders(DataTable stock);
        DataTable GetStocksCategory(DataTable stock);
    }
}
