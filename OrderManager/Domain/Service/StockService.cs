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

        public StockService(IStockDAO DAO, IMapperBase<Stock> mapper, 
            IMapperBase<Order> orderMapper, IMapperBase<CounterpartysStock> counterpartysStockMapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
            this.orderMapper = orderMapper;
            this.counterpartysStockMapper = counterpartysStockMapper;
        }

        public List<Stock> GetAll()
        {
            return mapper.MapAllFrom(DAO.GetAll());
        }

        public Stock GetById(string id)
        {
            return mapper.MapFrom(DAO.GetById(id));
        }

        public int GetNumOfItemsInOrders(Stock stock)
        {
            return GetStocksActiveOrders(stock)
                .Sum(order => order.Tranches.Where(tranche => tranche.Stock.Stock.Equals(stock))
                    .Sum(tranche => tranche.NumberOfItems));
        }

        public int GetNumOfItemsToOrder(Stock stock)
        {
            int numberOfItems = (30 + stock.MinInStockRoom) - stock.NumberOfItemsInStockRoom
                        - GetNumOfItemsInOrders(stock);
            return numberOfItems > 0 ? numberOfItems : 0;
        }

        public List<Order> GetStocksActiveOrders(Stock stock)
        {
            return orderMapper.MapAllFrom(DAO.GetStocksActiveOrders(mapper.MapTo(stock)));
        }

        public List<CounterpartysStock> GetStocksCounterpartysStock(Stock stock)
        {
            return counterpartysStockMapper.MapAllFrom(
                DAO.GetStocksCounterpartysStock(mapper.MapTo(stock)));
        }
        
    }
}
