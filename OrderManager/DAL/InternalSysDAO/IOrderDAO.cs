using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    interface IOrderDAO : IReadableDAO, IWritableDAO
    {
        DataTable GetCounterparty(DataRow order);
        DataTable GetUser(DataRow order);
        DataTable GetTranches(DataRow order);
        DataTable GetParentOrder(DataRow order);
        DataTable GetOrderState(DataRow order);
    }
}
