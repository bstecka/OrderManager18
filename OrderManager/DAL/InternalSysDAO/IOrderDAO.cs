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
        DataTable GetAllByState(int stateId);
        DataTable GetCounterparty(DataRow order);
        DataTable GetUser(DataRow order);
        DataTable GetTranches(DataRow order);
        DataTable GetParentOrder(DataRow order);
        DataTable GetParentOrderById(int id);
        DataTable GetOrderState(DataRow order);
        void UpdateOrders(DataTable table);
        void UpdateOrder(DataRow row);
        void OrderCorrection(DataTable oldOrder, DataTable newOrder);
    }
}
