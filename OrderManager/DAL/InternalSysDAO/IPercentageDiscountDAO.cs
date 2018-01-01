using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    interface IPercentageDiscountDAO : IReadableDAO, IWritableDAO
    {
        DataTable GetCounterpartysStockValidDicounts(DataTable counterpartysStock);
        DataTable GetCounterpartiesStockWithDiscount(DataTable discount);
        DataTable GetCounterpartiesStockWithDiscount(DataRow discount);

    }
}
