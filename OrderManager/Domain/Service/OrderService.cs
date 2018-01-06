using OrderManager.DAL.InternalSysDAO;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Service
{
    class OrderService
    {
        private IOrderDAO DAO;
        private IMapperBase<Order> mapper;

        public OrderService(IOrderDAO DAO, IMapperBase<Order> mapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
        }

        public List<Order> GetAll()
        {
            return mapper.MapAllFrom(DAO.GetAll());
        }

        public List<Order> GetAllDuringRealization()
        {
            return mapper.MapAllFrom(DAO.GetAllDuringRealization());
        }

        public Order GetById(string id)
        {
            return mapper.MapFrom(DAO.GetById(id));
        }
    }
}
