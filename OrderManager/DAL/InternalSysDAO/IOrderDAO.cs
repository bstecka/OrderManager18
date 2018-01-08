using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DAL.InternalSysDAO
{
    public interface IOrderDAO : IReadableDAO, IWritableDAO
    {
        DataTable GetAllDuringRealization();
        DataTable GetCounterparty(DataRow order);
        DataTable GetUser(DataRow order);
        DataTable GetTranches(DataRow order);
        DataTable GetParentOrder(DataRow order);
        DataTable GetParentOrderById(int id);
        DataTable GetOrderState(DataRow order);
    }
}
