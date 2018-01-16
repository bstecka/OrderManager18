using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManager.DTO
{
    class EligibleOrdersNamesMapper : IMapperBase<string>
    {
        /// <summary>
        /// Maps from the dataTable to a list strings represeting eligible order names.
        /// </summary>
        /// <param name="tDAO">The t DAO.</param>
        /// <returns>Returns a lists of strings reoresenting eligible order names</returns>
        public List<string> MapAllFrom(DataTable tDAO)
        {
            List<string> result = new List<string>();
            foreach (DataRow row in tDAO.Rows)
                result.Add(MapFrom(tDAO,
                    tDAO.Rows.IndexOf(row)));
            return result;
        }

        /// <summary>
        /// Maps from the dataTable to a string representing an eligible order name.
        /// </summary>
        /// <param name="tDAO">The t DAO.</param>
        /// <returns>Returns a string representing an eligible order name.</returns>
        public string MapFrom(DataTable tDAO)
        {
            return MapFrom(tDAO, 0);
        }

        /// <summary>
        /// Maps from a specified row of the dataTable to a string representing an eligible order name.
        /// </summary>
        /// <param name="tDAO">The t DAO.</param>
        /// <param name="numberOfRow">The number of row.</param>
        /// <returns>Returns a string representing an eligible order name.</returns>
        public string MapFrom(DataTable tDAO, int numberOfRow)
        {
            return tDAO.Rows[numberOfRow]["Nazwa"].ToString();
        }

        /// <summary>
        /// Unneeded mapper.
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public DataTable MapTo(string tDomain)
        {
            throw new NotImplementedException();
        }
    }
}
