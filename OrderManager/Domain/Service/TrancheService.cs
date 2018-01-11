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
        private IMapperBase<PercentageDiscount> discountMapper;

        public TrancheService(ITrancheDAO DAO, IMapperBase<Tranche> mapper, IMapperBase<PercentageDiscount> discountMapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
            this.discountMapper = discountMapper;
        }

        public List<Tranche> GetAll()
        {
            return mapper.MapAllFrom(DAO.GetAll());
        }

        public List<PercentageDiscount> GetPercentageDiscounts(Tranche tranche)
        {
            DataRow row = mapper.MapTo(tranche).Rows[0];
            return discountMapper.MapAllFrom(DAO.GetPercentageDiscounts(row));
        }

        public List<PercentageDiscount> GetViableDiscounts(Tranche tranche)
        {
            DataRow row = mapper.MapTo(tranche).Rows[0];
            return discountMapper.MapAllFrom(DAO.GetViableDiscounts(row));
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

        public void AssignDiscountToTranche(Tranche tranche, PercentageDiscount discount)
        {
            DataRow trancheRow = mapper.MapTo(tranche).Rows[0];
            DataRow discountRow = discountMapper.MapTo(discount).Rows[0];
            DAO.AssignDiscountToTranche(trancheRow, discountRow);
        }
    }
}
