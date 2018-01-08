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
        private IMapperBase<OrderDAO> mapper;

        public OrderService(IOrderDAO DAO, IMapperBase<OrderDAO> mapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
        }

        public List<OrderDAO> GetAll()
        {
            return mapper.MapAllFrom(DAO.GetAll());
        }

        public List<OrderDAO> GetAllDuringRealization()
        {
            return mapper.MapAllFrom(DAO.GetAllDuringRealization());
        }

        public OrderDAO GetById(string id)
        {
            return mapper.MapFrom(DAO.GetById(id));
        }

        List<Entity.Order> IEntityServiceBase<Entity.Order>.GetAll()
        {
            throw new NotImplementedException();
        }

        Entity.Order IEntityServiceBase<Entity.Order>.GetById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
