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
    class StockService : IStockService
    {
        private IStockDAO DAO;
        private IMapperBase<Stock> mapper;
        private IMapperBase<Order> orderMapper;
        private IMapperBase<CounterpartysStock> counterpartysStockMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockService"/> class.
        /// </summary>
        /// <param name="DAO">The DAO.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="orderMapper">The order mapper.</param>
        /// <param name="counterpartysStockMapper">The counterpartys stock mapper.</param>
        public StockService(IStockDAO DAO, IMapperBase<Stock> mapper, 
            IMapperBase<Order> orderMapper, IMapperBase<CounterpartysStock> counterpartysStockMapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
            this.orderMapper = orderMapper;
            this.counterpartysStockMapper = counterpartysStockMapper;
        }

        /// <summary>
        /// Gets all stock.
        /// </summary>
        /// <returns>Returns a list of stock entities.</returns>
        public List<Stock> GetAll()
        {
            return mapper.MapAllFrom(DAO.GetAll());
        }

        /// <summary>
        /// Gets stock by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns a stock entity with a given id.</returns>
        public Stock GetById(string id)
        {
            return mapper.MapFrom(DAO.GetById(id));
        }

        /// <summary>
        /// Gets the number of items in orders of the given stock.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <returns>Returns the number of items in orders of the given stock.</returns>
        public int GetNumOfItemsInOrders(Stock stock)
        {
            return GetStocksActiveOrders(stock)
                .Sum(order => order.Tranches.Where(tranche => tranche.Stock.Stock.Equals(stock))
                    .Sum(tranche => tranche.NumberOfItems));
        }

        /// <summary>
        /// Gets the number of items of the given stock that should be ordered.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <returns>Returns the number of items of the given stock that should be ordered.</returns>
        public int GetNumOfItemsToOrder(Stock stock)
        {
            int numberOfItems = (30 + stock.MinInStockRoom) - stock.NumberOfItemsInStockRoom
                        - GetNumOfItemsInOrders(stock);
            return numberOfItems > 0 ? numberOfItems : 0;
        }

        /// <summary>
        /// Gets the stocks active orders.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <returns>Returns the list of order entities during realization right now, that conatain the given stock.</returns>
        public List<Order> GetStocksActiveOrders(Stock stock)
        {
            return orderMapper.MapAllFrom(DAO.GetStocksActiveOrders(mapper.MapTo(stock)));
        }

        /// <summary>
        /// Gets the CounterpartysStock entities corresponding to the given stock.
        /// </summary>
        /// <param name="stock">The stock.</param>
        /// <returns>Returns the list of CounterpartysStock entities corresponding to the given stock</returns>
        public List<CounterpartysStock> GetStocksCounterpartysStock(Stock stock)
        {
            return counterpartysStockMapper.MapAllFrom(
                DAO.GetStocksCounterpartysStock(mapper.MapTo(stock)));
        }

        /// <summary>
        /// Sets the possibility to generate order with a given identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <returns>Returns a boolean value representing the current state of the possibility to generate order.</returns>
        public bool SetPossibilityToGenerateOrder(int id, int value)
        {
            return DAO.SetPossibilityToGenerateOrder(id, value);
        }
    }
}
