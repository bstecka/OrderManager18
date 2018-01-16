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

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderService"/> class.
        /// </summary>
        /// <param name="DAO">The DAO.</param>
        /// <param name="mapper">The mapper.</param>
        public OrderService(IOrderDAO DAO, IMapperBase<Order> mapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
        }

        /// <summary>
        /// Gets all orders during realization.
        /// </summary>
        /// <returns>Returns a list of order entities with a state equal to 'duringRealization'</returns>
        public List<Order> GetAllDuringRealization()
        {
            return mapper.MapAllFrom(DAO.GetAllDuringRealization());
        }

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>Returns a list of order entites.</returns>
        public List<Order> GetAll()
        {
            var orders = mapper.MapAllFrom(DAO.GetAll());
            foreach (Order order in orders)
                order.ParentOrder = GetParentOrder(order);
            return orders;
        }

        /// <summary>
        /// Gets the order by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns an order entity with the id equal to the given identifier.</returns>
        public Order GetById(string id)
        {
            var order = mapper.MapFrom(DAO.GetById(id));
            order.ParentOrder = GetParentOrder(order);
            return order;
        }

        /// <summary>
        /// Gets all orders with a state which id is equal to given state identifier.
        /// </summary>
        /// <param name="stateId">The state identifier.</param>
        /// <returns>Returns a list of order entity with the state's id equal to the given identifier.</returns>
        public List<Order> GetAllByState(int stateId)
        {
            var orders = mapper.MapAllFrom(DAO.GetAllByState(stateId));
            foreach (Order order in orders)
                order.ParentOrder = GetParentOrder(order);
            return orders;
        }

        /// <summary>
        /// Updates the order with an id equal to the id of the given order entity to match the values of the given order entity.
        /// </summary>
        /// <param name="order">The order.</param>
        public void UpdateOrder(Order order)
        {
            DataTable table = mapper.MapTo(order);
            DAO.UpdateOrders(table);
        }

        /// <summary>
        /// Inserts the order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>Returns the identifier of the inserted order assigned by the database.</returns>
        public int InsertOrder(Order order)
        {
            DataTable table = mapper.MapTo(order);
            return DAO.Add(table);
        }

        /// <summary>
        /// Gets the parent order for a given order.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <returns>Returns the parent order for the order entity, or null if the given order does not have a parent order.</returns>
        public Order GetParentOrder(Order order)
        {
            DataTable parentTable = DAO.GetParentOrderById(order.Id.GetValueOrDefault());
            if (parentTable.Rows.Count < 1)
                return null;
            else
                return mapper.MapFrom(parentTable);
        }

        /// <summary>
        /// Changes the values of an order with the id of oldOrder to values of newOrder.
        /// </summary>
        /// <param name="oldOrder">The old order.</param>
        /// <param name="newOrder">The new order.</param>
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
