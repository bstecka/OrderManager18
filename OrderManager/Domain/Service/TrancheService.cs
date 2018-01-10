using OrderManager.DAL.InternalSysDAO;
using OrderManager.Domain.Entity;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Service
{
    class TrancheService : ITrancheService
    {
        private ITrancheDAO DAO;
        private IMapperBase<Tranche> mapper;

        public TrancheService(ITrancheDAO DAO, IMapperBase<Tranche> mapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
        }

        public List<Tranche> GetAll()
        {
            return mapper.MapAllFrom(DAO.GetAll());
        }

        public Tranche GetById(string id)
        {
            return mapper.MapFrom(DAO.GetById(id));
        }

        public void UpdateTranche(Tranche tranche)
        {
            DataTable table = mapper.MapTo(tranche);
            DAO.Update(table);
        }

        public int InsertTranche(Tranche tranche)
        {
            DataTable table = mapper.MapTo(tranche);
            return DAO.Add(table);
        }
    }
}
