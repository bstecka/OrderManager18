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
    class PriorityService : IPriorityService
    {
        IPriorityDAO priorityDAO;
        IMapperBase<List<Priority>> priorityMapper;
        IMapperBase<Stock> stockMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PriorityService"/> class.
        /// </summary>
        /// <param name="priorityDAO">The priority DAO.</param>
        /// <param name="priorityMapper">The priority mapper.</param>
        /// <param name="stockMapper">The stock mapper.</param>
        public PriorityService(IPriorityDAO priorityDAO, IMapperBase<List<Priority>> priorityMapper, IMapperBase<Stock> stockMapper)
        {
            this.priorityDAO = priorityDAO;
            this.stockMapper = stockMapper;
            this.priorityMapper = priorityMapper;
        }

        /// <summary>
        /// Gets the list of stock priorities for the given stock.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <returns>Returns the list of priority entities corresponding to the given stock.</returns>
        public List<Priority> GetStockPriority(Stock stock)
        {
            return stock == null ? null : priorityMapper.MapFrom(
                priorityDAO.GetPriority(stockMapper.MapTo(stock)));
        }
    }
}
