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

        public PriorityService(IPriorityDAO priorityDAO, IMapperBase<List<Priority>> priorityMapper)
        {
            this.priorityDAO = priorityDAO;
            this.priorityMapper = priorityMapper;
        }

        public List<Priority> GetStockPriority(Stock stock, StockMapper stockMapper)
        {
            return priorityMapper.MapFrom(
                priorityDAO.GetPriority(stockMapper.MapTo(stock)));
        }
    }
}
