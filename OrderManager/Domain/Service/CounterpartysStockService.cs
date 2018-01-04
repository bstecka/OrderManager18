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

        public CounterpartysStockService(ICounterpartysStockDAO counterpartysStockDAO, IMapperBase<Entity.CounterpartysStock> counterpartysStockMapper, IMapperBase<Entity.PercentageDiscount> percentageDiscountMapper)
        {
            this.counterpartysStockDAO = counterpartysStockDAO;
            this.counterpartysStockMapper = counterpartysStockMapper;
            this.percentageDiscountMapper = percentageDiscountMapper;
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
            (Entity.CounterpartysStock counterpartysStock)
        {
            return percentageDiscountMapper.MapAllFrom(counterpartysStockDAO.GetCounterpartysStockValidDicounts(
                counterpartysStockMapper.MapTo(counterpartysStock)));
        }
    }
}
