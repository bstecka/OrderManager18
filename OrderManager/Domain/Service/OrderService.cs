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
    class OrderService : IOrderService
    {
        private IOrderDAO DAO;
        private IMapperBase<Order> mapper;

        public OrderService(IOrderDAO DAO, IMapperBase<Order> mapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
        }

        public List<Order> GetAllDuringRealization()
        {
            return mapper.MapAllFrom(DAO.GetAllDuringRealization());
        }

        public List<Order> GetAll()
        {
            return mapper.MapAllFrom(DAO.GetAll());
        }

        public Order GetById(string id)
        {
            return mapper.MapFrom(DAO.GetById(id));
        }

        public List<Order> GetAllByState(int stateId)
        {
            return mapper.MapAllFrom(DAO.GetAllByState(stateId));
        }
    }
}
