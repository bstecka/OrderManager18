using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Entity;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Service
{
    class CounterpartyService : ICounterpartyService
    {
        private ICounterpartyDAO DAO;
        private IMapperBase<Counterparty> mapper;
        private IMapperBase<CounterpartysStock> counterpartysStockMapper;
        private IMapperBase<Entity.Order> orderMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterpartyService"/> class.
        /// </summary>
        /// <param name="DAO">The DAO.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="counterpartysStockMapper">The counterpartys stock mapper.</param>
        /// <param name="orderMapper">The order mapper.</param>
        public CounterpartyService(ICounterpartyDAO DAO, IMapperBase<Counterparty> mapper,
            IMapperBase<Entity.CounterpartysStock> counterpartysStockMapper, IMapperBase<Entity.Order> orderMapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
            this.counterpartysStockMapper = counterpartysStockMapper;
            this.orderMapper = orderMapper;
        }

        /// <summary>
        /// Gets all counterparties.
        /// </summary>
        /// <returns>Returns a list of counterparty entities</returns>
        public List<Counterparty> GetAll()
        {
            return mapper.MapAllFrom(DAO.GetAll());
        }

        /// <summary>
        /// Gets counterparty by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns a counterparty entity with a given id</returns>
        public Counterparty GetById(string id)
        {
            return mapper.MapFrom(DAO.GetById(id));
        }

        /// <summary>
        /// Gets the counterpartys orders.
        /// </summary>
        /// <param name="counterparty">The counterparty.</param>
        /// <returns>Returns a list of orders corresponding to the given counterparty.</returns>
        public List<Entity.Order> GetCounterpartysOrders(Counterparty counterparty)
        {
            return orderMapper.MapAllFrom(DAO.GetCounterpartysOrders(mapper.MapTo(counterparty)));
        }

        /// <summary>
        /// Gets the counterpartys stock.
        /// </summary>
        /// <param name="counterparty">The counterparty.</param>
        /// <returns>Returns a list of counterpartys stock corresponding to the given counterparty.</returns>
        public List<Entity.CounterpartysStock> GetCounterpartysStock(Counterparty counterparty)
        {
            return counterpartysStockMapper.MapAllFrom(
                DAO.GetCounterpartysStock(mapper.MapTo(counterparty)));
        }
        
    }
}
