using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Service
{
    public interface IOrderService : IEntityServiceBase<Order>
    {
        List<Order> GetAllDuringRealization();
        List<Order> GetAllByState(int stateId);
        void UpdateOrder(Order order);
        void InsertOrder(Order order);
    }
}
