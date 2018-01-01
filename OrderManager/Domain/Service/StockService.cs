using OrderManager.DAL.InternalSysDAO;
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

        public StockService(IStockDAO DAO, IMapperBase<Entity.Stock> mapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
        }

        public List<Entity.Stock> GetAll()
        {
            return mapper.MapAllFrom(DAO.GetAll());
        }

        public Entity.Stock GetById(string id)
        {
            return mapper.MapFrom(DAO.GetById(id));
        }
        
        public List<Entity.CounterpartysStock> GetStocksCounterpartysStock(Entity.Stock stock,
            IMapperBase<Entity.CounterpartysStock> counterpartysStockMapper)
        {
            return counterpartysStockMapper.MapAllFrom(
                DAO.GetStocksCounterpartysStock(mapper.MapTo(stock)));
        }
        
    }
}
