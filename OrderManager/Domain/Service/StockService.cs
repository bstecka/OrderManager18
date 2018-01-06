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
        private IMapperBase<Entity.Stock> mapper;
        private IMapperBase<Entity.Order> orderMapper;
        private IMapperBase<Entity.CounterpartysStock> counterpartysStockMapper;

        public StockService(IStockDAO DAO, IMapperBase<Entity.Stock> mapper, 
            IMapperBase<Entity.Order> orderMapper, IMapperBase<Entity.CounterpartysStock> counterpartysStockMapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
            this.orderMapper = orderMapper;
            this.counterpartysStockMapper = counterpartysStockMapper;
        }

        public List<Entity.Stock> GetAll()
        {
            return mapper.MapAllFrom(DAO.GetAll());
        }

        public Entity.Stock GetById(string id)
        {
            return mapper.MapFrom(DAO.GetById(id));
        }

        public int GetNumOfItemsInOrders(Entity.Stock stock)
        {
            return GetStocksActiveOrders(stock)
                .Sum(order => order.Tranches.Where(tranche => tranche.Stock.Stock.Equals(stock))
                    .Sum(tranche => tranche.NumberOfItems));
        }

        public int GetNumOfItemsToOrder(Entity.Stock stock)
        {
            int numberOfItems = (30 + stock.MinInStockRoom) - stock.NumberOfItemsInStockRoom
                        - GetNumOfItemsInOrders(stock);
            return numberOfItems > 0 ? numberOfItems : 0;
        }

        public List<Entity.Order> GetStocksActiveOrders(Entity.Stock stock)
        {
            return orderMapper.MapAllFrom(DAO.GetStocksActiveOrders(mapper.MapTo(stock)));
        }

        public List<Entity.CounterpartysStock> GetStocksCounterpartysStock(Entity.Stock stock)
        {
            return counterpartysStockMapper.MapAllFrom(
                DAO.GetStocksCounterpartysStock(mapper.MapTo(stock)));
        }
        
    }
}
