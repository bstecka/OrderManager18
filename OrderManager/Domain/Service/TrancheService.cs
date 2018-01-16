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

        /// <summary>
        /// Initializes a new instance of the <see cref="TrancheService"/> class.
        /// </summary>
        /// <param name="DAO">The DAO.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="discountMapper">The discount mapper.</param>
        public TrancheService(ITrancheDAO DAO, IMapperBase<Tranche> mapper, IMapperBase<PercentageDiscount> discountMapper)
        {
            this.DAO = DAO;
            this.mapper = mapper;
            this.discountMapper = discountMapper;
        }

        /// <summary>
        /// Gets all tranches.
        /// </summary>
        /// <returns>Returns a list of tranche entities.</returns>
        public List<Tranche> GetAll()
        {
            return mapper.MapAllFrom(DAO.GetAll());
        }

        /// <summary>
        /// Gets the percentage discounts assigned to the given tranche.
        /// </summary>
        /// <param name="tranche">The tranche.</param>
        /// <returns>Returns a list of percentage discount entities.</returns>
        public List<PercentageDiscount> GetPercentageDiscounts(Tranche tranche)
        {
            DataRow row = mapper.MapTo(tranche).Rows[0];
            return discountMapper.MapAllFrom(DAO.GetPercentageDiscounts(row));
        }

        /// <summary>
        /// Gets the discounts that could be assigned to the given tranche.
        /// </summary>
        /// <param name="tranche">The tranche.</param>
        /// <returns>Returns a list of percentage discount entities including all viable discounts for this tranche.</returns>
        public List<PercentageDiscount> GetViableDiscounts(Tranche tranche)
        {
            DataRow row = mapper.MapTo(tranche).Rows[0];
            return discountMapper.MapAllFrom(DAO.GetViableDiscounts(row));
        }

        /// <summary>
        /// Gets tranche by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Returns a tranche entity with the id equal to the given identifier.</returns>
        public Tranche GetById(string id)
        {
            return mapper.MapFrom(DAO.GetById(id));
        }

        /// <summary>
        /// Updates the tranche with an id equal to the id of the given tranche entity to match the values of the given tranche entity.
        /// </summary>
        /// <param name="tranche">The tranche.</param>
        public void UpdateTranche(Tranche tranche)
        {
            DataTable table = mapper.MapTo(tranche);
            DAO.Update(table);
        }

        /// <summary>
        /// Inserts the tranche.
        /// </summary>
        /// <param name="tranche">The tranche.</param>
        /// <returns>Returns the identifier of the inserted tranche assigned by the database.</returns>
        public int InsertTranche(Tranche tranche)
        {
            DataTable table = mapper.MapTo(tranche);
            return DAO.Add(table);
        }

        /// <summary>
        /// Assigns the discount to tranche in the database.
        /// </summary>
        /// <param name="tranche">The tranche.</param>
        /// <param name="discount">The discount.</param>
        public void AssignDiscountToTranche(Tranche tranche, PercentageDiscount discount)
        {
            DataRow trancheRow = mapper.MapTo(tranche).Rows[0];
            DataRow discountRow = discountMapper.MapTo(discount).Rows[0];
            DAO.AssignDiscountToTranche(trancheRow, discountRow);
        }
    }
}
