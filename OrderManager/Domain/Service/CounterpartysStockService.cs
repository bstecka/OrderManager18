using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManager.DAL.InternalSysDAO;
using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Entity;
using OrderManager.DTO;

namespace OrderManager.Domain.Service
{
    class CounterpartysStockService : ICounterpartysStockService
    {
        private ICounterpartysStockDAO counterpartysStockDAO;
        private IMapperBase<Entity.CounterpartysStock> counterpartysStockMapper;

        public CounterpartysStockService(ICounterpartysStockDAO counterpartysStockDAO, IMapperBase<Entity.CounterpartysStock> counterpartysStockMapper)
        {
            this.counterpartysStockDAO = counterpartysStockDAO;
            this.counterpartysStockMapper = counterpartysStockMapper;
        }

        public List<Entity.CounterpartysStock> GetAll()
        {
            return counterpartysStockMapper.MapAllFrom(counterpartysStockDAO.GetAll());
        }

        public Entity.CounterpartysStock GetById(string id)
        {
            return counterpartysStockMapper.MapFrom(counterpartysStockDAO.GetById(id));
        }

        public List<Entity.PercentageDiscount> GetValidDiscounts
            (IPercentageDiscountDAO percentageDiscountDAO,
            IMapperBase<Entity.PercentageDiscount> percentageDiscountMapper,
            Entity.CounterpartysStock counterpartysStock)
        {
            return percentageDiscountMapper.MapAllFrom(percentageDiscountDAO.GetCounterpartysStockValidDicounts(
                counterpartysStockMapper.MapTo(counterpartysStock)));
        }
    }
}
