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
        public List<string> MapAllFrom(DataTable tDAO)
        {
            List<string> result = new List<string>();
            foreach (DataRow row in tDAO.Rows)
                result.Add(MapFrom(tDAO,
                    tDAO.Rows.IndexOf(row)));
            return result;
        }

        public string MapFrom(DataTable tDAO)
        {
            return MapFrom(tDAO, 0);
        }

        public string MapFrom(DataTable tDAO, int numberOfRow)
        {
            return tDAO.Rows[numberOfRow]["Nazwa"].ToString();
        }

        public DataTable MapTo(string tDomain)
        {
            throw new NotImplementedException();
        }
    }
}
