using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManager.DAL.ExternalSysDAO;
using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Entity;
using OrderManager.DTO;

namespace OrderManager.Domain.Service
{
    class CounterpartysStockService : ICounterpartysStockService
    {
        private ICounterpartysStockDAO counterpartysStockDAO;
        private IMapperBase<Entity.CounterpartysStock> counterpartysStockMapper;
        private IMapperBase<Entity.PercentageDiscount> percentageDiscountMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CounterpartysStockService"/> class.
        /// </summary>
        /// <param name="counterpartysStockDAO">The counterpartys stock DAO.</param>
        /// <param name="counterpartysStockMapper">The counterpartys stock mapper.</param>
        /// <param name="percentageDiscountMapper">The percentage discount mapper.</param>
        public CounterpartysStockService(ICounterpartysStockDAO counterpartysStockDAO, IMapperBase<Entity.CounterpartysStock> counterpartysStockMapper, IMapperBase<Entity.PercentageDiscount> percentageDiscountMapper)
        {
            this.counterpartysStockDAO = counterpartysStockDAO;
            this.counterpartysStockMapper = counterpartysStockMapper;
            this.percentageDiscountMapper = percentageDiscountMapper;
        }

        /// <summary>
        /// Gets all counterparty stock.
        /// </summary>
        /// <returns>Returns a list of counterparty stock entities</returns>
        public List<Entity.CounterpartysStock> GetAll()
        {
            return counterpartysStockMapper.MapAllFrom(counterpartysStockDAO.GetAll());
        }

        /// <summary>
        /// Gets counterparty stock by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns a counterparty stock entity with a given id.</returns>
        public Entity.CounterpartysStock GetById(string id)
        {
            return counterpartysStockMapper.MapFrom(counterpartysStockDAO.GetById(id));
        }

        /// <summary>
        /// Gets all valid discounts for a given counterparty stock.
        /// </summary>
        /// <param name="counterpartysStock">The counterpartys stock.</param>
        /// <returns>Returns a list of percentage discount entities.</returns>
        public List<Entity.PercentageDiscount> GetValidDiscounts
            (Entity.CounterpartysStock counterpartysStock)
        {
            return percentageDiscountMapper.MapAllFrom(counterpartysStockDAO.GetCounterpartysStockValidDicounts(
                counterpartysStockMapper.MapTo(counterpartysStock)));
        }
    }
}
