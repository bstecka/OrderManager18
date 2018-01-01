using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    interface ITrancheDAO : IReadableDAO, IWritableDAO
    {
        DataTable GetCounterpartysStock(DataRow tranche);
        DataTable GetPercentageDiscounts(DataRow tranche);
    }
}
