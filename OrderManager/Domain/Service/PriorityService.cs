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

        public PriorityService(IPriorityDAO priorityDAO, IMapperBase<List<Priority>> priorityMapper, IMapperBase<Stock> stockMapper)
        {
            this.priorityDAO = priorityDAO;
            this.stockMapper = stockMapper;
            this.priorityMapper = priorityMapper;
        }

        public List<Priority> GetStockPriority(Stock stock)
        {
            return stock == null ? null : priorityMapper.MapFrom(
                priorityDAO.GetPriority(stockMapper.MapTo(stock)));
        }
    }
}
