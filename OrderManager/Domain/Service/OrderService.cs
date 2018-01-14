using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Entity;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Service
{
    class OrderService : IOrderService
    {
        private IOrderDAO DAO;
        private IMapperBase<Order> mapper;

        public OrderService(IOrderDAO DAO, IMapperBase<Order> mapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
        }

        public List<Order> GetAllDuringRealization()
        {
            return mapper.MapAllFrom(DAO.GetAllDuringRealization());
        }

        public List<Order> GetAll()
        {
            var orders = mapper.MapAllFrom(DAO.GetAll());
            foreach (Order order in orders)
                order.ParentOrder = GetParentOrder(order);
            return orders;
        }

        public Order GetById(string id)
        {
            return mapper.MapFrom(DAO.GetById(id));
        }

        public List<Order> GetAllByState(int stateId)
        {
            return mapper.MapAllFrom(DAO.GetAllByState(stateId));
        }

        public void UpdateOrder(Order order)
        {
            DataTable table = mapper.MapTo(order);
            DAO.UpdateOrders(table);
        }

        public int InsertOrder(Order order)
        {
            DataTable table = mapper.MapTo(order);
            return DAO.Add(table);
        }

        public Order GetParentOrder(Order order)
        {
            DataTable parentTable = DAO.GetParentOrderById(order.Id.GetValueOrDefault()); //DLACZEGO TU BYLO 0????
            if (parentTable.Rows.Count < 1)
                return null;
            else
                return mapper.MapFrom(parentTable);
        }

        public void OrderCorrection(Order oldOrder, Order newOrder)
        {
            DataTable oldOrderTable = mapper.MapTo(oldOrder);
            DataTable newOrderTable = mapper.MapTo(newOrder);
            DAO.OrderCorrection(
                mapper.MapTo(oldOrder), 
                mapper.MapTo(newOrder));
        }
    }
}
