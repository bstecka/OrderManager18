using OrderManager.DAL.InternalSysDAO;
using OrderManager.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.Domain.Service
{
    class EligibleORersNamesService : IEligibleOrdersNamesService
    {
        private IEligibleOrdersNamesDAO eligibleOrdersNamesDAO;
        private IMapperBase<string> mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="EligibleORersNamesService"/> class.
        /// </summary>
        /// <param name="eligibleORersNamesDAO">The eligible o rers names DAO.</param>
        /// <param name="mapper">The mapper.</param>
        public EligibleORersNamesService(IEligibleOrdersNamesDAO eligibleORersNamesDAO, IMapperBase<string> mapper)
        {
            this.eligibleOrdersNamesDAO = eligibleORersNamesDAO;
            this.mapper = mapper;
        }

        /// <summary>
        /// Fetches eligible names for orders.
        /// </summary>
        /// <param name="numberOfNames">The number of names.</param>
        /// <returns>Returns a list of strings representing eligible names of orders, with a size equal to given number of names.</returns>
        /// <exception cref="Exception">Brak wolnych nazw zamówień</exception>
        public List<string> FetchNames(int numberOfNames)
        {
            var names = mapper.MapAllFrom(eligibleOrdersNamesDAO.FetchNames(numberOfNames));
            if (names.Count() < numberOfNames)
                throw new Exception("Brak wolnych nazw zamówień");
            return names;
        }
    }
}
