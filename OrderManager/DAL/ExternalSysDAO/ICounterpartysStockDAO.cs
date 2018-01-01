using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    interface ICounterpartysStockDAO : IReadableDAO
    {
        DataTable GetPercentageDiscounts(DataTable counterpartysStock);
        DataTable GetCounterparty(DataTable counterpartysStock);
        DataTable GetStock(DataRow counterpartysStock);
        DataTable GetCounterparty(DataRow counterpartysStock);
        DataTable GetStock(DataTable counterpartysStock);
    }
}
